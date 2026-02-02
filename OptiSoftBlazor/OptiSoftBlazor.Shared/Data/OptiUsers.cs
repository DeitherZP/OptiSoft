using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class OptiUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? PasswordHash { get; set; } = string.Empty;

        public bool Activo { get; set;  } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
