using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    [Table("Consulta", Schema = "optica")]
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConsulta { get; set; }

        public int IdCliente { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente? Cliente { get; set; }

        public int? IdPedido { get; set; }

        [ForeignKey(nameof(IdPedido))]
        public Compra? Pedido { get; set; }

        public DateTime? Fecha { get; set; } = DateTime.Today;


        #region Informacion Global

        public int IdProfesional { get; set; }

        [ForeignKey(nameof(IdProfesional))]
        public Personal? Profesional { get; set; }

        public string? Motivo { get; set; }

        public string? AntPatoOcular { get; set; }

        #endregion  Informacion Global


        #region Lente Normales

        #region Datos para Fabricación (Abierta)
        #region Rx Final
        [StringLength(20)]
        public string? RxEsferaOD { get; set; }

        [StringLength(20)]
        public string? RxEsferaOI { get; set; }

        [StringLength(20)]
        public string? RxCilindroOD { get; set; }

        [StringLength(20)]
        public string? RxCilindroOI { get; set; }

        [StringLength(20)]
        public string? RxEjeOD { get; set; }

        [StringLength(20)]
        public string? RxEjeOI { get; set; }

        [StringLength(20)]
        public string? RxAddOD { get; set; }

        [StringLength(20)]
        public string? RxAddOI { get; set; }

        [StringLength(20)]
        public string? RxDnpOD { get; set; }

        [StringLength(20)]
        public string? RxDnpOI { get; set; }

        [StringLength(20)]
        public string? RxAltOD { get; set; }

        [StringLength(20)]
        public string? RxAltOI { get; set; }

        [StringLength(20)]
        public string? RxAvlOD { get; set; }

        [StringLength(20)]
        public string? RxAvlOI { get; set; }

        [StringLength(20)]
        public string? RxAvcOD { get; set; }

        [StringLength(20)]
        public string? RxAvcOI { get; set; }
        #endregion

        #region Tipo Lente
        [StringLength(20)]
        public string? DiamArmVertical { get; set; }

        [StringLength(20)]
        public string? DiamArmHorizontal { get; set; }

        [StringLength(20)]
        public string? DiamArmPuente { get; set; }

        [StringLength(20)]
        public string? DiamArmMayor { get; set; }

        [StringLength(20)]
        public string? DiamArmTipo { get; set; }

        [StringLength(20)]
        public string? ParPersAP { get; set; }

        [StringLength(20)]
        public string? ParPersCP { get; set; }

        [StringLength(20)]
        public string? ParPersDV { get; set; }

        [StringLength(20)]
        public string? ParPersCorredor { get; set; }

        [StringLength(4000)]
        public string? TipoLente { get; set; }

        [StringLength(4000)]
        public string? Lubricante { get; set; }

        [StringLength(20)]
        public string? AlturaOD { get; set; }

        [StringLength(20)]
        public string? AlturaOI { get; set; }

        #endregion

        #region Tratamiento

        [StringLength(4000)]
        public string? Tratamiento { get; set; }
        #endregion
        #endregion Datos para Fabricación (Abierta)

        #region Evaluacion Clinica

        #region Rx Uso
        [StringLength(20)]
        public string? RxUsoEsferaOD { get; set; }

        [StringLength(20)]
        public string? RxUsoEsferaOI { get; set; }

        [StringLength(20)]
        public string? RxUsoCilindroOD { get; set; }

        [StringLength(20)]
        public string? RxUsoCilindroOI { get; set; }

        [StringLength(20)]
        public string? RxUsoEjeOD { get; set; }

        [StringLength(20)]
        public string? RxUsoEjeOI { get; set; }

        [StringLength(20)]
        public string? RxUsoAddOD { get; set; }

        [StringLength(20)]
        public string? RxUsoAddOI { get; set; }

        [StringLength(20)]
        public string? RxUsoAvlOD { get; set; }

        [StringLength(20)]
        public string? RxUsoAvlOI { get; set; }

        [StringLength(20)]
        public string? RxUsoAvcOD { get; set; }

        [StringLength(20)]
        public string? RxUsoAvcOI { get; set; }
        #endregion

        #endregion Evaluacion Clinica

        #endregion Lente Normales


        #region Lentes de Contacto
        #region Queratometria
        [StringLength(20)]
        public string? LC_KeraK1OD { get; set; }

        [StringLength(20)]
        public string? LC_KeraK1OI { get; set; }

        [StringLength(20)]
        public string? LC_KeraK2OD { get; set; }

        [StringLength(20)]
        public string? LC_KeraK2OI { get; set; }
        #endregion

        #region Prueba Final
        #region Bloque 1
        [StringLength(20)]
        public string? LC_LFinRxOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinRxOI { get; set; }

        [StringLength(20)]
        public string? LC_LFinTipoOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinTipoOI { get; set; }

        [StringLength(20)]
        public string? LC_LFinCBOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinCBOI { get; set; }

        [StringLength(20)]
        public string? LC_LFinDiamOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinDiamOI { get; set; }

        [StringLength(20)]
        public string? LC_LFinEspesorOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinEspesorOI { get; set; }

        [StringLength(20)]
        public string? LC_LFinSagitaOD { get; set; }

        [StringLength(20)]
        public string? LC_LFinSagitaOI { get; set; }

        [StringLength(4000)]
        public string? LC_LFinDisenoOD { get; set; }

        [StringLength(4000)]
        public string? LC_LFinDisenoOI { get; set; }
        #endregion

        #region Bloque 2
        [StringLength(4000)]
        public string? LC_Diagnostico { get; set; }

        [StringLength(4000)]
        public string? LC_Humectante { get; set; }

        [StringLength(4000)]
        public string? LC_SolucLimpieza { get; set; }
        #endregion
        #endregion

        #endregion  Lentes de Contacto

    }
}
