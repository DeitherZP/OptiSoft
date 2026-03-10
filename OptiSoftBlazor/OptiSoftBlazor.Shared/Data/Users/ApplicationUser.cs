using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OptiSoftBlazor.Shared.Pages.Optica;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.Users
{
    public class ApplicationUser : IdentityUser
    {
        public bool ForcePasswordChange { get; set; } = false;

        public int? idPersonal { get; set; } = 0;

        [ForeignKey(nameof(idPersonal))]
        public Personal? Personal { get; set; }

        [NotMapped]
        public string NombrePersonal
        {
            get
            {
                // Aquí SÍ puedes usar el FK
                if (string.IsNullOrEmpty(Id))
                    return string.Empty;

                return Personal?.Nombre ?? string.Empty;
            }
        }
    }
}
