using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.Tenant;
using OptiSoftBlazor.Shared.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptiSoftBlazor.Web.Services
{
    public class TenantService : ITenantService
    {
        private readonly IDbContextFactory<TenantDbContext> _tenantDbContextFactory;
        private readonly AuthenticationStateProvider _authStateProvider;

        public TenantService(
            IDbContextFactory<TenantDbContext> tenantDbContextFactory,
            AuthenticationStateProvider authStateProvider)
        {
            _tenantDbContextFactory = tenantDbContextFactory;
            _authStateProvider = authStateProvider;
        }

        public async Task<string?> GetCurrentTenantNameAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user?.FindFirst("TenantName")?.Value;
        }

        public async Task<string?> GetCurrentConnectionStringAsync()
        {
            var tenantName = await GetCurrentTenantNameAsync();
            if (string.IsNullOrWhiteSpace(tenantName))
                return null;

            // 👇 Cada llamada crea su propio contexto, sin colisiones
            await using var context = await _tenantDbContextFactory.CreateDbContextAsync();
            var tenant = await context.Tenant
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == tenantName);

            return tenant?.ConnectionString;
        }

        public async Task<Tenant?> GetTenantByNameAsync(string tenantName)
        {
            await using var context = await _tenantDbContextFactory.CreateDbContextAsync();
            return await context.Tenant.FirstOrDefaultAsync(t => t.Name == tenantName);
        }

        public async Task<string?> GetConnectionStringAsync(string tenantName)
        {
            await using var context = await _tenantDbContextFactory.CreateDbContextAsync();
            return await context.Tenant
                .Where(t => t.Name == tenantName)
                .Select(t => t.ConnectionString)
                .FirstOrDefaultAsync();
        }
    }
}
