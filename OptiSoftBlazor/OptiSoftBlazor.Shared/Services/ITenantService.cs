using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public interface ITenantService
    {
        Task<string?> GetConnectionStringAsync(string tenantName);
        Task<Data.Tenant.Tenant?> GetTenantByNameAsync(string tenantName);
        Task<string?> GetCurrentTenantNameAsync();
        Task<string?> GetCurrentConnectionStringAsync();
    }
}
