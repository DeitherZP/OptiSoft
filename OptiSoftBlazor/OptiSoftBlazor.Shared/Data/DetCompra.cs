using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class DetCompra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetCompra { get; set; }

        public double Cantidad { get; set; } = 1;

        public int IdArticulo { get; set; }

        [ForeignKey(nameof(IdArticulo))]
        public Articulo Articulo { get; set; }

        public int IdCompra { get; set; }

        [ForeignKey(nameof(IdCompra))]
        public Compra Compra { get; set; }
    }
}
