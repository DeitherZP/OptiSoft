using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdArticulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Codigo { get; set; }

        [Column("Articulo")]
        public string Nombre { get; set; }

        public double Precio2 { get; set; }

        public double Iva { get; set; }
    }
}
