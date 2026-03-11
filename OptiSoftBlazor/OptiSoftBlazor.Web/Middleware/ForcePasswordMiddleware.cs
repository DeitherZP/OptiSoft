using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OptiSoftBlazor.Web.Middleware
{
    public class ForcePasswordMiddleware
    {
        private readonly RequestDelegate _next;

        public ForcePasswordMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value ?? "";

                var excludedPaths = new[]
                {
                    "/Account/Login",
                    "/Account/Logout",
                    "/Account/PasswordChanged",
                    "/_blazor",
                    "/_framework",
                    "/_vs",
                    "/css/",
                    "/js/",
                    "/images/",
                    "/api/",
                    "/favicon.ico",
                    "/.well-known"
                };

                bool isExcluded = excludedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

                if (!isExcluded && context.User.Identity?.IsAuthenticated == true)
                {
                    var username = context.User.Identity.Name;
                    var tenantName = context.User.FindFirst("TenantName")?.Value;

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(tenantName))
                    {
                        // Scope explícito para evitar problemas con servicios Scoped
                        await using var scope = context.RequestServices.CreateAsyncScope();
                        var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();
                        var connectionString = await tenantService.GetConnectionStringAsync(tenantName);

                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            var options = new DbContextOptionsBuilder<OptiSoftDbContext>()
                                .UseSqlServer(connectionString)
                                .Options;

                            await using var db = new OptiSoftDbContext(options);

                            var user = await db.ApplicationUser
                                .Where(u => u.UserName == username)
                                .Select(u => new { u.ForcePasswordChange })
                                .FirstOrDefaultAsync();

                            if (user?.ForcePasswordChange == true)
                            {
                                context.Response.Redirect("/Account/PasswordChanged");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nunca cortar la pipeline, solo loguear
                Console.WriteLine($"[ForcePasswordMiddleware] ERROR: {ex.Message}");
            }

            // SIEMPRE continuar, pase lo que pase
            await _next(context);
        }
    }
}