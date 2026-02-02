using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ConsultaService
    {
        private readonly OptiSoftDbContext _db;

        public ConsultaService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<Consulta>> ObtenerConsultasAsync()
        {
            return await _db.Consulta
                            .Include(c => c.Cliente)
                            .Include(c => c.Pedido)
                            .Include(c => c.Profesional)
                            .OrderByDescending(c => c.Fecha)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
