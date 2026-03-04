using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;
using OptiSoftBlazor.Shared.Helpers;

namespace OptiSoftBlazor.Web.Services
{
    public class TenantDbContextFactory : ITenantDbContextFactory
    {
        private readonly ITenantService _tenantService;
        private string? _cachedConnectionString;

        public TenantDbContextFactory(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public async Task<OptiSoftDbContext> CreateDbContextAsync()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            if (string.IsNullOrWhiteSpace(_cachedConnectionString))
            {
                _cachedConnectionString = await _tenantService.GetCurrentConnectionStringAsync();
                Console.WriteLine($"GetConnectionString: {sw.ElapsedMilliseconds}ms");

                if (!ScEncripter.IsInitialized)
                {
                    ScEncripter.Initialize(_cachedConnectionString ?? "");
                }
            }
            else
            {
                Console.WriteLine("Usando connection string cacheada");
            }

            var ctx = new OptiSoftDbContext(
                new DbContextOptionsBuilder<OptiSoftDbContext>()
                    .UseSqlServer(_cachedConnectionString) 
                    .Options
            );

            Console.WriteLine($"CreateDbContext total: {sw.ElapsedMilliseconds}ms");
            return ctx;
        }
    }

}
