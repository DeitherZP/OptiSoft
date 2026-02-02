using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [StringLength(100)]
        public String? Nombre { get; set; }

        [StringLength(20)]
        public String? Ruc { get; set; }

        public String? Direccion { get; set; }

        public String? Telefono { get; set; }

        public String? Email { get; set; }

        public Boolean Proveedor { get; set; } = false;

        public DateTime? FechaNacimiento { get; set; }

        public DateTime? Fecha { get; set; } = DateTime.Today;
    }
}
