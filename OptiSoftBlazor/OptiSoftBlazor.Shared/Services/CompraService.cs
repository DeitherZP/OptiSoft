using Microsoft.EntityFrameworkCore;
using Microsoft.Fast.Components.FluentUI;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Pages.Optica;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class CompraService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public CompraService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Obtener todas las compras
        public async Task<List<Compra>> ObtenerPedidosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var connection = db.Database.GetConnectionString();
            Console.WriteLine($"🔍 CompraService usando conexión: {connection}");

            return await db.Compra
                            .Include(c => c.Cliente)
                            .Where(c => c.IdTipoFactura == 13)
                            .OrderByDescending(c => c.Fecha)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task GuardarCompraAsync(Compra compra)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            if (compra == null)
                throw new ArgumentNullException(nameof(compra));

            if (string.IsNullOrWhiteSpace(compra.Numero))
                throw new ArgumentException("El número es obligatorio");

            if (!compra.Fecha.HasValue)
                compra.Fecha = DateTime.Now;

            if (compra.Cliente == null)
                throw new ArgumentException("Debe seleccionar un cliente");

            db.Attach(compra.Cliente);

            if (compra.IdCompra == 0)
            {
                await db.Compra.AddAsync(compra);
            }
            else
            {
                var compraDb = await db.Compra
                                        .FirstOrDefaultAsync(c => c.IdCompra == compra.IdCompra);

                if (compraDb == null)
                    throw new Exception("La compra no existe");

                compraDb.Numero = compra.Numero;
                compraDb.Fecha = compra.Fecha;
                compraDb.Laboratorio = compra.Laboratorio;
                compraDb.IdCliente = compra.Cliente.IdCliente;
                compraDb.Finalizado = compra.Finalizado;

                db.Compra.Update(compraDb);
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarCompraAsync(int idCompra)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var compra = await db.Compra
                .FirstOrDefaultAsync(c => c.IdCompra == idCompra);

            if (compra == null)
                throw new Exception("La compra no existe");

            db.Compra.Remove(compra);
            await db.SaveChangesAsync();
        }
    }
}
