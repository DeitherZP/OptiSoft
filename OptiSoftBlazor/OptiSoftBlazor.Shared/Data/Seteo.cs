using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class Seteo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSeteo { get; set; }

        [StringLength(100)]
        public string Empresa { get; set; } = string.Empty;

        [StringLength(20)]
        public string Ruc { get; set; } = string.Empty;

        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(50)]
        public string Telefono { get; set; } = string.Empty;
    }
}
