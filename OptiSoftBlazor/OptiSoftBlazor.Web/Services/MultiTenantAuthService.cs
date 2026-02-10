using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;
using System.Security.Claims;

namespace OptiSoftBlazor.Web.Services
{
    public class MultiTenantAuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITenantService _tenantService;
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public MultiTenantAuthService(
            SignInManager<IdentityUser> signInManager,
            ITenantService tenantService,
            IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _signInManager = signInManager;
            _tenantService = tenantService;
            _contextFactory = contextFactory;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                // 1. Validar formato usuario@tenant
                if (string.IsNullOrWhiteSpace(username) || !username.Contains("@"))
                {
                    Console.WriteLine("❌ Formato de usuario inválido. Debe ser: usuario@tenant");
                    return false;
                }

                // 2. Extraer usuario y tenant
                var parts = username.Split('@');
                if (parts.Length != 2)
                {
                    Console.WriteLine("❌ Formato de usuario inválido. Debe ser: usuario@tenant");
                    return false;
                }

                var user = parts[0];
                var tenantName = parts[1];

                Console.WriteLine($"🔍 Buscando tenant: {tenantName}");

                // 3. Buscar el tenant en la DB central
                var tenant = await _tenantService.GetTenantByNameAsync(tenantName);
                if (tenant == null)
                {
                    Console.WriteLine($"❌ Tenant '{tenantName}' no encontrado");
                    return false;
                }

                Console.WriteLine($"✅ Tenant encontrado: {tenant.Name}");
                Console.WriteLine($"🔗 Connection String: {tenant.ConnectionString?.Substring(0, Math.Min(50, tenant.ConnectionString.Length))}...");

                // 4. Guardar el tenant actual en el servicio
                _tenantService.CurrentTenantName = tenant.Name;
                _tenantService.CurrentConnectionString = tenant.ConnectionString;

                // 5. Crear un DbContext temporal para la DB del tenant
                var optionsBuilder = new DbContextOptionsBuilder<OptiSoftDbContext>();
                optionsBuilder.UseSqlServer(tenant.ConnectionString);

                using var tenantContext = new OptiSoftDbContext(optionsBuilder.Options);

                // 6. Crear UserManager temporal para la DB del tenant
                var userStore = new Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore<IdentityUser>(tenantContext);
                var userManager = new UserManager<IdentityUser>(
                    userStore,
                    null,
                    new PasswordHasher<IdentityUser>(),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

                // 7. Buscar el usuario en la DB del tenant con el username completo (usuario@tenant)
                var identityUser = await userManager.FindByNameAsync(username);
                if (identityUser == null)
                {
                    Console.WriteLine($"❌ Usuario '{username}' no encontrado en la base de datos del tenant");
                    return false;
                }

                Console.WriteLine($"✅ Usuario encontrado: {identityUser.UserName}");

                // 8. Verificar la contraseña
                var passwordValid = await userManager.CheckPasswordAsync(identityUser, password);
                if (!passwordValid)
                {
                    Console.WriteLine("❌ Contraseña incorrecta");
                    return false;
                }

                Console.WriteLine("✅ Contraseña válida");

                // 9. SignIn manual (sin usar SignInManager que usa DB central)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                    new Claim(ClaimTypes.Name, identityUser.UserName!),
                    new Claim("TenantName", tenantName)
                };

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _signInManager.Context.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

                Console.WriteLine($"✅ Login exitoso para {username} en tenant {tenantName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en LoginAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            // Limpiar la información del tenant actual
            _tenantService.CurrentTenantName = null;
            _tenantService.CurrentConnectionString = null;

            Console.WriteLine("✅ Logout exitoso");
        }
    }
}
