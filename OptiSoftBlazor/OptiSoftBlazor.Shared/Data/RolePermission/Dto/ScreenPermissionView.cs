using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.RolePermission.Dto
{
    public class ScreenPermissionView
    {
        public int ScreenId { get; set; }

        public string ScreenName { get; set; } = "";

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

    }
}
