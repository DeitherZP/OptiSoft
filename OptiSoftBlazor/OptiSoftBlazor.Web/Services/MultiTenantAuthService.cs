using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Data.Users;
using OptiSoftBlazor.Shared.Services;
using System.Security.Claims;

namespace OptiSoftBlazor.Web.Services
{
    public class MultiTenantAuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITenantService _tenantService;

        public MultiTenantAuthService(
            SignInManager<IdentityUser> signInManager,
            ITenantService tenantService)
        {
            _signInManager = signInManager;
            _tenantService = tenantService;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                // Validar formato usuario@tenant
                if (string.IsNullOrWhiteSpace(username) || !username.Contains("@"))
                {
                    Console.WriteLine("Formato de usuario inválido. Debe ser: usuario@tenant");
                    return false;
                }

                var parts = username.Split('@');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Formato de usuario inválido. Debe ser: usuario@tenant");
                    return false;
                }

                var tenantName = parts[1];

                // Buscar el tenant en la DB central
                var tenant = await _tenantService.GetTenantByNameAsync(tenantName);
                if (tenant == null)
                {
                    Console.WriteLine($"Tenant '{tenantName}' no encontrado");
                    return false;
                }

                Console.WriteLine($"Tenant encontrado: {tenant.Name}");
                Console.WriteLine($"Connection String: {tenant.ConnectionString?.Substring(0, Math.Min(50, tenant.ConnectionString.Length))}...");

                // Crear DbContext temporal para la DB del tenant
                var optionsBuilder = new DbContextOptionsBuilder<OptiSoftDbContext>();
                optionsBuilder.UseSqlServer(tenant.ConnectionString);

                using var tenantContext = new OptiSoftDbContext(optionsBuilder.Options);

                // Buscar usuario por NormalizedUserName
                var normalizedUsername = username.ToUpper();

                var user = await tenantContext.ApplicationUser
                    .FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    Console.WriteLine($"Usuario '{username}' no encontrado en la base de datos del tenant");
                    return false;
                }

                Console.WriteLine($"Usuario encontrado: {user.UserName}");

                // Verificar la contraseña
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result != PasswordVerificationResult.Success)
                {
                    Console.WriteLine("Contraseña incorrecta");
                    return false;
                }

                Console.WriteLine("Contraseña válida");

                // SignIn manual
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim("TenantName", tenantName)
                };

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _signInManager.Context.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

                Console.WriteLine($"Login exitoso para {username} en tenant {tenantName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en LoginAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            Console.WriteLine("Logout exitoso");
        }
    }
}