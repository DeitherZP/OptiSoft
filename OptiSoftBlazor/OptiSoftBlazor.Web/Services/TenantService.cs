using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.Tenant;
using OptiSoftBlazor.Shared.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptiSoftBlazor.Web.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantDbContext _tenantDbContext;

        public TenantService(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }

        public string? CurrentTenantName { get; set; }
        public string? CurrentConnectionString { get; set; }

        /// <summary>
        /// Obtiene la cadena de conexión para un tenant específico
        /// </summary>
        public async Task<string?> GetConnectionStringAsync(string tenantName)
        {
            if (string.IsNullOrWhiteSpace(tenantName))
                return null;

            var tenant = await _tenantDbContext.Tenant
                .FirstOrDefaultAsync(t => t.Name == tenantName);

            return tenant?.ConnectionString;
        }

        /// <summary>
        /// Obtiene el tenant completo por nombre
        /// </summary>
        public async Task<Tenant?> GetTenantByNameAsync(string tenantName)
        {
            if (string.IsNullOrWhiteSpace(tenantName))
                return null;

            return await _tenantDbContext.Tenant
                .FirstOrDefaultAsync(t => t.Name == tenantName);
        }
    }
}
