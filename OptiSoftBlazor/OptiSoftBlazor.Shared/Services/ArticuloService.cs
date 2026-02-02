using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ArticuloService
    {
        private readonly OptiSoftDbContext _db;

        public ArticuloService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<Articulo>> ObtenerArticulosAsync()
        {
            return await _db.Articulo
                            .Where(a => a.Nombre != null && a.Nombre != "")
                            .OrderBy(a => a.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
