using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public interface ITenantDbContextFactory
    {
        Task<OptiSoftDbContext> CreateDbContextAsync();
    }
}
