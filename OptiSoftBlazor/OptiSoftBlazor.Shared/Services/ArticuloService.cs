using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ArticuloService
    {
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public ArticuloService(IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Articulo>> ObtenerArticulosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Articulo
                            .Where(a => a.Nombre != null && a.Nombre != "")
                            .OrderBy(a => a.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
