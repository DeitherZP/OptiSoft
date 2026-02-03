using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class SeteoService
    {
        private readonly OptiSoftDbContext _db;

        public SeteoService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<Seteo?> ObtenerSeteoAsync()
        {
            return await _db.Seteo
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }
    }
}
