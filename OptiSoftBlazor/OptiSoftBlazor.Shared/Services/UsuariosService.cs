using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Data.Users;
using OptiSoftBlazor.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class UsuariosService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public UsuariosService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<ApplicationUser>> ObtenerUsuariosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var tenant = TenantInfo.GetTenantName();

            return await db.ApplicationUser
                           .Where(a => a.UserName.EndsWith("@" + tenant))
                           .Include(a => a.Personal)
                                .ThenInclude(p => p.RolSucursal)
                           .OrderBy(a => a.UserName)
                           .AsNoTracking()
                           .ToListAsync();
        }

        public async Task GuardarUsuarioAsync(ApplicationUser user)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentException("El nombre de usuario es obligatorio");

            // Crear el hasher
            var hasher = new PasswordHasher<ApplicationUser>();

            // Solo hasheamos si la contraseña no está vacía
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);
            }

            var userDb = await db.ApplicationUser
                         .FirstOrDefaultAsync(a => a.Id == user.Id);

            if (userDb == null)
            {
                await db.ApplicationUser.AddAsync(user);
            }
            else
            {
                userDb.UserName = user.UserName;
                userDb.ForcePasswordChange = user.ForcePasswordChange;
                userDb.idPersonal = user.idPersonal;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                    userDb.PasswordHash = user.PasswordHash;

                db.ApplicationUser.Update(userDb);
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarUsuarioAsync(string idUser)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var user = await db.ApplicationUser
                .FirstOrDefaultAsync(a => a.Id == idUser);

            if (user == null)
                throw new Exception("El artículo no existe");

            db.ApplicationUser.Remove(user);
            await db.SaveChangesAsync();
        }

        public async Task<bool> CambiarPasswordAsync(string username, string newPassword)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var user = await db.ApplicationUser
                .FirstOrDefaultAsync(a => a.UserName == username);

            if (user == null) return false;

            var hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            user.ForcePasswordChange = false;

            db.ApplicationUser.Update(user);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
