using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class SeteoService
    {
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public SeteoService(IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Seteo?> ObtenerSeteoAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Seteo
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }
    }
}
