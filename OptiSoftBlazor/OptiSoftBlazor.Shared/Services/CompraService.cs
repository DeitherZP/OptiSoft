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
        private readonly OptiSoftDbContext _db;

        public CompraService(OptiSoftDbContext db)
        {
            _db = db;
        }

        // Obtener todas las compras
        public async Task<List<Compra>> ObtenerPedidosAsync()
        {
            return await _db.Compra
                            .Include(c => c.Cliente)
                            .Where(c => c.IdTipoFactura == 13)
                            .OrderByDescending(c => c.Fecha)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task GuardarCompraAsync(Compra compra)
        {
            if (compra == null)
                throw new ArgumentNullException(nameof(compra));

            if (string.IsNullOrWhiteSpace(compra.Numero))
                throw new ArgumentException("El número es obligatorio");

            if (!compra.Fecha.HasValue)
                compra.Fecha = DateTime.Now;

            if (compra.Cliente == null)
                throw new ArgumentException("Debe seleccionar un cliente");

            _db.Attach(compra.Cliente);

            if (compra.IdCompra == 0)
            {
                await _db.Compra.AddAsync(compra);
            }
            else
            {
                var compraDb = await _db.Compra
                                        .FirstOrDefaultAsync(c => c.IdCompra == compra.IdCompra);

                if (compraDb == null)
                    throw new Exception("La compra no existe");

                compraDb.Numero = compra.Numero;
                compraDb.Fecha = compra.Fecha;
                compraDb.IdCliente = compra.Cliente.IdCliente;

                _db.Compra.Update(compraDb);
            }

            await _db.SaveChangesAsync();
        }

        public async Task EliminarCompraAsync(int idCompra)
        {
            var compra = await _db.Compra
                .FirstOrDefaultAsync(c => c.IdCompra == idCompra);

            _db.Compra.Remove(compra);
            await _db.SaveChangesAsync();
        }
    }
}
