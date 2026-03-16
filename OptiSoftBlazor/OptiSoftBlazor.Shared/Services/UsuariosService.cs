using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Data.RolePermission.Dto;
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

        public async Task GuardarUsuarioAsync(ApplicationUser user, List<RoleView> roles)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentException("El nombre de usuario es obligatorio");

            var hasher = new PasswordHasher<ApplicationUser>();

            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);
            }

            var userDb = await db.ApplicationUser
                .FirstOrDefaultAsync(a => a.Id == user.Id);

            if (userDb == null)
            {
                await db.ApplicationUser.AddAsync(user);
                await db.SaveChangesAsync();

                userDb = user;
            }
            else
            {
                userDb.UserName = user.UserName;
                userDb.ForcePasswordChange = user.ForcePasswordChange;
                userDb.idPersonal = user.idPersonal;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                    userDb.PasswordHash = user.PasswordHash;

                db.ApplicationUser.Update(userDb);
                await db.SaveChangesAsync();
            }

            // 🔹 Manejo de roles del usuario

            var rolesActuales = await db.UserRoles
                .Where(r => r.UserId == userDb.Id)
                .ToListAsync();

            db.UserRoles.RemoveRange(rolesActuales);

            foreach (var role in roles)
            {
                db.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = userDb.Id,
                    RoleId = role.RoleId
                });
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarUsuarioAsync(string idUser)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var user = await db.ApplicationUser
                .FirstOrDefaultAsync(a => a.Id == idUser);

            if (user == null)
                throw new Exception("El usuario no existe");

            var roles = await db.UserRoles
                .Where(r => r.UserId == idUser)
                .ToListAsync();

            db.UserRoles.RemoveRange(roles);

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

        public async Task<List<RoleView>> ObtenerRolesUsuarioAsync(string userId)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var roleIds = await db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var roles = await db.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => new RoleView
                {
                    RoleId = r.Id,
                    RoleName = r.Name
                })
                .OrderBy(r => r.RoleName)
                .ToListAsync();

            return roles;
        }

        public async Task<List<ScreenPermissionView>> ObtenerPermisosUsuarioAsync(string userId)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var roleIds = await db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var permisos = await db.RoleScreenPermission
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.AppScreen)
                .Select(rp => new ScreenPermissionView
                {
                    ScreenName = rp.AppScreen.Route,
                    CanView = rp.CanView,
                    CanCreate = rp.CanCreate,
                    CanEdit = rp.CanEdit,
                    CanDelete = rp.CanDelete
                })
                .ToListAsync();

            return permisos;
        }
    }
}
