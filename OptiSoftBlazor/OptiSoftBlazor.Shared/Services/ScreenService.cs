using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.RolePermission;
using OptiSoftBlazor.Shared.Data.RolePermission.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ScreenService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public ScreenService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<AppScreen>> ObtenerScreensAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.AppScreen
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
