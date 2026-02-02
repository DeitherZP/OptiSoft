using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ClienteService
    {
        private readonly OptiSoftDbContext _db;

        public ClienteService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<Cliente>> ObtenerClienteAsync()
        {
            return await _db.Cliente
                            .Where(c => c.Nombre != null && c.Nombre != "")
                            .OrderBy(c => c.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
