using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class Compra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCompra { get; set; }

        public string? Numero { get; set; } = string.Empty;

        public DateTime? Fecha { get; set; } = DateTime.Today;

        public int? IdCliente { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente? Cliente { get; set; }

        public int IdTipoFactura { get; set; } = 13;

        public bool Finalizado { get; set; } = false;

        public string? Laboratorio { get; set; } = string.Empty;

        public ObservableCollection<DetCompra> DetCompra { get; set; } = [];
    }
}
