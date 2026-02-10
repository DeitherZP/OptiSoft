using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class PersonalService
    {
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public PersonalService(IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Personal>> ObtenerPersonalAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Personal
                            .Where(a => a.Nombre != null && a.Nombre != "")
                            .OrderBy(a => a.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
