using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class PermissionService
    {
        public bool CanView(string screenKey)
        {
            return false;
        }

        public bool CanCreat(string screenKey)
        {
            return true;
        }

        public bool CanEdit(string screenKey)
        {
            return true;
        }

        public bool CanDelete(string screenKey)
        {
            return true;
        }
    }
}
