using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.Tenant;
using OptiSoftBlazor.Shared.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptiSoftBlazor.Web.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantDbContext _tenantDbContext;
        private readonly AuthenticationStateProvider _authStateProvider;

        public TenantService(
        TenantDbContext tenantDbContext,
        AuthenticationStateProvider authStateProvider)
        {
            _tenantDbContext = tenantDbContext;
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

            var tenant = await _tenantDbContext.Tenant
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == tenantName);

            return tenant?.ConnectionString;
        }

        // Mantén tus métodos existentes
        public Task<Tenant?> GetTenantByNameAsync(string tenantName) =>
            _tenantDbContext.Tenant.FirstOrDefaultAsync(t => t.Name == tenantName);

        public Task<string?> GetConnectionStringAsync(string tenantName) =>
            _tenantDbContext.Tenant
                .Where(t => t.Name == tenantName)
                .Select(t => t.ConnectionString)
                .FirstOrDefaultAsync();
    }
}
