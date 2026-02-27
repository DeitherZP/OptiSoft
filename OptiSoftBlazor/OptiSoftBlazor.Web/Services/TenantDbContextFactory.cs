using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;

namespace OptiSoftBlazor.Web.Services
{
    public class TenantDbContextFactory : ITenantDbContextFactory
    {
        private readonly ITenantService _tenantService;

        public TenantDbContextFactory(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public async Task<OptiSoftDbContext> CreateDbContextAsync()
        {
            var conn = await _tenantService.GetCurrentConnectionStringAsync();

            if (string.IsNullOrWhiteSpace(conn))
                throw new Exception("Tenant no inicializado");

            return new OptiSoftDbContext(
                new DbContextOptionsBuilder<OptiSoftDbContext>()
                    .UseSqlServer(conn)
                    .Options
            );
        }
    }

}
