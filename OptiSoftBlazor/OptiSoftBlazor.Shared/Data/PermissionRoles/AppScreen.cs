using OptiSoftBlazor.Shared.Data.PermissionRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.Permission
{
    public class AppScreen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Route { get; set; } = string.Empty;

        public ICollection<RoleScreenPermission>? RoleScreenPermission { get; set; }
    }
}
