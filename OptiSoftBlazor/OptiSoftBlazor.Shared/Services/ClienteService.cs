using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ClienteService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public ClienteService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Cliente>> ObtenerClienteAsync()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            using var db = await _contextFactory.CreateDbContextAsync();
            Console.WriteLine($"CreateDbContext: {sw.ElapsedMilliseconds}ms");

            var result = await db.Cliente
                .Where(c => c.Nombre != null && c.Nombre != "")
                .OrderBy(c => c.Nombre)
                .AsNoTracking()
                .ToListAsync();

            Console.WriteLine($"ToListAsync: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Clientes obtenidos: {result.Count}");

            return result;
        }
    }
}
