using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;

namespace OptiSoftBlazor.Web.Authorization
{
    public class ForcePasswordChangeRequirement : IAuthorizationRequirement { }

    public class ForcePasswordChangeHandler : AuthorizationHandler<ForcePasswordChangeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForcePasswordChangeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ForcePasswordChangeRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Sin contexto HTTP (pre-render SSR sin circuito) → dejar pasar
            if (httpContext is null)
            {
                context.Succeed(requirement);
                return;
            }

            var user = context.User;

            // No autenticado → dejar que el middleware de auth lo maneje
            if (user.Identity?.IsAuthenticated != true)
            {
                context.Succeed(requirement);
                return;
            }

            var username = user.Identity.Name;
            var tenantName = user.FindFirst("TenantName")?.Value;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(tenantName))
            {
                context.Succeed(requirement);
                return;
            }

            try
            {
                await using var scope = httpContext.RequestServices.CreateAsyncScope();
                var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();
                var connectionString = await tenantService.GetConnectionStringAsync(tenantName);

                if (string.IsNullOrEmpty(connectionString))
                {
                    context.Succeed(requirement);
                    return;
                }

                var options = new DbContextOptionsBuilder<OptiSoftDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                await using var db = new OptiSoftDbContext(options);

                var forceChange = await db.ApplicationUser
                    .Where(u => u.UserName == username)
                    .Select(u => u.ForcePasswordChange)
                    .FirstOrDefaultAsync();

                if (forceChange)
                {
                    // Falla el requirement → ASP.NET Core invocará el
                    // ForbidAsync del esquema de autenticación, que a su vez
                    // ejecutará el OnRedirectToAccessDenied configurado abajo.
                    context.Fail();
                    return;
                }
            }
            catch (Exception ex)
            {
                // Nunca bloquear al usuario por un error de infraestructura
                Console.WriteLine($"[ForcePasswordChangeHandler] ERROR: {ex.Message}");
            }

            context.Succeed(requirement);
        }
    }
}
