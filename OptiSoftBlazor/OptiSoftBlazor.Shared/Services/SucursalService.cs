using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.Aditionals;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class SucursalService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public SucursalService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<RolSucursal>> ObtenerSucursalesAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.RolSucursal
                .Where(c => c.Sucursal != null && c.Sucursal != "")
                .OrderBy(c => c.Sucursal)
                .AsNoTracking()
                .ToListAsync();
        }
        
    }
}

