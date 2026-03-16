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

        public async Task<List<RolePermissionView>> ObtenerRolesAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var roles = await db.Roles
                                .AsNoTracking()
                                .ToListAsync();

            var permisos = await db.RoleScreenPermission
                                   .AsNoTracking()
                                   .ToListAsync();

            var result = roles.Select(r =>
            {
                var p = permisos.FirstOrDefault(x => x.RoleId == r.Id);

                return new RolePermissionView
                {
                    RoleId = r.Id,
                    RoleName = r.Name ?? "",
                    CanView = p?.CanView ?? false,
                    CanCreate = p?.CanCreate ?? false,
                    CanEdit = p?.CanEdit ?? false,
                    CanDelete = p?.CanDelete ?? false
                };
            }).ToList();

            return result;
        }

        public async Task<List<RolePermissionView>> ObtenerPermisosRolAsync(string roleId)
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var screens = await db.AppScreen
                .AsNoTracking()
                .ToListAsync();

            var permisosRol = await db.RoleScreenPermission
                .Where(r => r.RoleId == roleId)
                .AsNoTracking()
                .ToListAsync();

            var resultado = screens.Select(s =>
            {
                var permiso = permisosRol.FirstOrDefault(p => p.ScreenId == s.Id);

                return new RolePermissionView
                {
                    RoleId = roleId,
                    ScreenId = s.Id,
                    ScreenName = s.CodePage,

                    CanView = permiso?.CanView ?? false,
                    CanCreate = permiso?.CanCreate ?? false,
                    CanEdit = permiso?.CanEdit ?? false,
                    CanDelete = permiso?.CanDelete ?? false
                };
            })
            .OrderBy(x => x.ScreenName)
            .ToList();

            return resultado;
        }
    }
}
