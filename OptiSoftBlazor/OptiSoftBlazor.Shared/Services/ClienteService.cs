using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ClienteService
    {
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public ClienteService(IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Cliente>> ObtenerClienteAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Cliente
                            .Where(c => c.Nombre != null && c.Nombre != "")
                            .OrderBy(c => c.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
