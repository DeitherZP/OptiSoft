using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class DetCompraService
    {
        private readonly OptiSoftDbContext _db;

        public DetCompraService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<DetCompra>> ObtenerDetComprasAsync(int idCompra)
        {
            return await _db.DetCompra
                            .Where(dc => dc.IdCompra == idCompra)
                            .Include(dc => dc.Articulo)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task GuardarDetallesAsync(int idCompra, List<DetCompra> detalles)
        {
            if (detalles == null)
                return;

            var existentes = await _db.DetCompra
                                      .Where(d => d.IdCompra == idCompra)
                                      .ToListAsync();

            foreach (var det in detalles)
            {
                det.IdCompra = idCompra;

                // Decide si actualizar o insertar
                if (det.IdDetCompra > 0)
                {
                    // UPDATE
                    var existente = existentes.FirstOrDefault(e => e.IdDetCompra == det.IdDetCompra);

                    if (existente != null)
                    {
                        existente.IdArticulo = det.IdArticulo;
                        existente.Cantidad = det.Cantidad;
                    }
                }
                else
                {
                    // INSERT
                    _db.DetCompra.Add(new DetCompra
                    {
                        IdCompra = idCompra,
                        IdArticulo = det.IdArticulo,
                        Cantidad = det.Cantidad
                    });
                }
            }

            // Elimina lo que ya no estan en la UI
            var idsUI = detalles
                .Where(d => d.IdDetCompra > 0)
                .Select(d => d.IdDetCompra)
                .ToList();

            var aEliminar = existentes
                .Where(e => !idsUI.Contains(e.IdDetCompra))
                .ToList();

            _db.DetCompra.RemoveRange(aEliminar);

            await _db.SaveChangesAsync();
        }

        public async Task EliminarDetCompraAsync(int idCompra)
        {
            var detalles = _db.DetCompra
                .Where(d => d.IdCompra == idCompra);

            _db.DetCompra.RemoveRange(detalles);
            await _db.SaveChangesAsync();
        }
    }
}
