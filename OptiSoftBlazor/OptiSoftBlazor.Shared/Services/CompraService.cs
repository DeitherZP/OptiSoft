using Microsoft.EntityFrameworkCore;
using Microsoft.Fast.Components.FluentUI;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class CompraService
    {
        private readonly IDbContextFactory<OptiSoftDbContext> _contextFactory;

        public CompraService(IDbContextFactory<OptiSoftDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Obtener todas las compras
        public async Task<List<Compra>> ObtenerPedidosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

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

            if (compra.idCompra == 0)
            {
                await db.Compra.AddAsync(compra);
            }
            else
            {
                var compraDb = await db.Compra
                                        .FirstOrDefaultAsync(c => c.idCompra == compra.idCompra);

                if (compraDb == null)
                    throw new Exception("La compra no existe");

                compraDb.Numero = compra.Numero;
                compraDb.Fecha = compra.Fecha;
                compraDb.idCliente = compra.Cliente.IdCliente;

                db.Compra.Update(compraDb);
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarCompraAsync(int idCompra)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var compra = await db.Compra
                .FirstOrDefaultAsync(c => c.idCompra == idCompra);

            db.Compra.Remove(compra);
            await db.SaveChangesAsync();
        }
    }
}
