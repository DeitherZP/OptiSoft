using OptiSoftBlazor.Shared.Data.RolePermission.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Helpers
{
    public static class Permisos
    {
        private static Dictionary<string, ScreenPermissionView> _permissions = new();

        public static void SetPermissions(List<ScreenPermissionView> permissions)
        {
            _permissions = permissions.ToDictionary(p => p.ScreenName);
        }

        public static bool CanView(string screenName)
        {
            return _permissions.TryGetValue(screenName, out var p) && p.CanView;
        }

        public static bool CanCreate(string screenName)
        {
            return _permissions.TryGetValue(screenName, out var p) && p.CanCreate;
        }

        public static bool CanEdit(string screenName)
        {
            return _permissions.TryGetValue(screenName, out var p) && p.CanEdit;
        }

        public static bool CanDelete(string screenName)
        {
            return _permissions.TryGetValue(screenName, out var p) && p.CanDelete;
        }
    }
}
