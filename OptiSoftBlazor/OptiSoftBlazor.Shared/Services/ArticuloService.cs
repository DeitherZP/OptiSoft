using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ArticuloService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public ArticuloService(ITenantDbContextFactory contextFactory)
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

        public async Task GuardarArticuloAsync(Articulo articulo)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            if (articulo == null)
                throw new ArgumentNullException(nameof(articulo));

            if (string.IsNullOrWhiteSpace(articulo.Codigo))
                throw new ArgumentException("El código es obligatorio");

            if (string.IsNullOrWhiteSpace(articulo.Nombre))
                throw new ArgumentException("El nombre es obligatorio");

            if(articulo.IdArticulo == 0)
            {
                await db.Articulo.AddAsync(articulo);
            }
            else
            {
                var articuloDb = await db.Articulo
                                            .FirstOrDefaultAsync(art => art.IdArticulo == articulo.IdArticulo);

                if (articuloDb == null)
                    throw new Exception("El artículo no existe");

                articuloDb.Codigo = articulo.Codigo;
                articuloDb.Nombre = articulo.Nombre;
                articuloDb.Precio2 = articulo.Precio2;
                articuloDb.Iva = articulo.Iva;

                db.Articulo.Update(articuloDb);
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarArticuloAsync(int idArticulo)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var articulo = await db.Articulo
                .FirstOrDefaultAsync(art => art.IdArticulo == idArticulo);

            if (articulo == null)
                throw new Exception("El artículo no existe");

            db.Articulo.Remove(articulo);
            await db.SaveChangesAsync();
        }
    }
}
