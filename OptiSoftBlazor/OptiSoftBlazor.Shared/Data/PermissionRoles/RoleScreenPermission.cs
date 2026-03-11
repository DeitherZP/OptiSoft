using Microsoft.AspNetCore.Identity;
using OptiSoftBlazor.Shared.Data.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.PermissionRoles
{
    public class RoleScreenPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RoleId { get; set; } = string.Empty;

        [ForeignKey(nameof(RoleId))]
        public IdentityRole? Role { get; set; }

        public int ScreenId { get; set; }

        [ForeignKey(nameof(ScreenId))]
        public AppScreen? AppScreen { get; set; }

        public bool CanView { get; set; } = true;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }
}
