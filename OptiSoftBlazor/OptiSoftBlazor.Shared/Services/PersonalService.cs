using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class PersonalService
    {
        private readonly OptiSoftDbContext _db;

        public PersonalService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<Personal>> ObtenerPersonalAsync()
        {
            return await _db.Personal
                            .Where(a => a.Nombre != null && a.Nombre != "")
                            .OrderBy(a => a.Nombre)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
