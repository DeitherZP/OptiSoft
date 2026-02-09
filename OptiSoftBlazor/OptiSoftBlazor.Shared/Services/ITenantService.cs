using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public interface ITenantService
    {
        Task<string?> GetConnectionStringAsync(string tenantName);
        Task<Data.Tenant.Tenant?> GetTenantByNameAsync(string tenantName);
        string? CurrentTenantName { get; set; }
        string? CurrentConnectionString { get; set; }
    }
}
