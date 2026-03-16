using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data.RolePermission;
using OptiSoftBlazor.Shared.Data.RolePermission.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class RolesService
    {
        private readonly ITenantDbContextFactory _contextFactory;

        public RolesService(ITenantDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<RoleView>> ObtenerRolesAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Roles
                .AsNoTracking()
                .Select(r => new RoleView
                {
                    RoleId = r.Id,
                    RoleName = r.Name ?? ""
                })
                .ToListAsync();
        }

        public async Task<List<ScreenPermissionView>> ObtenerPermisosRolAsync(string roleId)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var screens = await db.AppScreen
                                  .Where(a => !string.IsNullOrEmpty(a.CodePage))
                                  .ToListAsync();

            var permisos = await db.RoleScreenPermission
                                   .Where(x => x.RoleId == roleId)
                                   .ToListAsync();

            return screens.Select(s =>
            {
                var p = permisos.FirstOrDefault(x => x.ScreenId == s.Id);

                return new ScreenPermissionView
                {
                    ScreenId = s.Id,
                    ScreenName = s.CodePage,
                    CanView = p?.CanView ?? false,
                    CanCreate = p?.CanCreate ?? false,
                    CanEdit = p?.CanEdit ?? false,
                    CanDelete = p?.CanDelete ?? false
                };
            }).ToList();
        }

        public async Task GuardarRolAsync(string? roleId, string roleName, List<ScreenPermissionView> permisos)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            IdentityRole role;

            if (string.IsNullOrEmpty(roleId))
            {
                role = new IdentityRole
                {
                    Name = roleName
                };

                db.Roles.Add(role);
                await db.SaveChangesAsync();
            }
            else
            {
                role = await db.Roles.FirstAsync(r => r.Id == roleId);
                role.Name = roleName;

                db.Roles.Update(role);
                await db.SaveChangesAsync();
            }

            var roleIdFinal = role.Id;

            var permisosExistentes = await db.RoleScreenPermission
                .Where(x => x.RoleId == roleIdFinal)
                .ToListAsync();

            foreach (var p in permisos)
            {
                var existente = permisosExistentes
                    .FirstOrDefault(x => x.ScreenId == p.ScreenId);

                if (existente == null)
                {
                    db.RoleScreenPermission.Add(new RoleScreenPermission
                    {
                        RoleId = roleIdFinal,
                        ScreenId = p.ScreenId,
                        CanView = p.CanView,
                        CanCreate = p.CanCreate,
                        CanEdit = p.CanEdit,
                        CanDelete = p.CanDelete
                    });
                }
                else
                {
                    existente.CanView = p.CanView;
                    existente.CanCreate = p.CanCreate;
                    existente.CanEdit = p.CanEdit;
                    existente.CanDelete = p.CanDelete;

                    db.RoleScreenPermission.Update(existente);
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task EliminarRolAsync(string roleId)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new Exception("El rol no existe.");

            var permisos = await db.RoleScreenPermission
                .Where(x => x.RoleId == roleId)
                .ToListAsync();

            if (permisos.Any())
                db.RoleScreenPermission.RemoveRange(permisos);

            db.Roles.Remove(role);

            await db.SaveChangesAsync();
        }
    }
}
