using Microsoft.EntityFrameworkCore;
using OptiSoftBlazor.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    public class ConsultaService
    {
        private readonly OptiSoftDbContext _db;

        public ConsultaService(OptiSoftDbContext db)
        {
            _db = db;
        }

        public async Task<List<Consulta>> ObtenerConsultasAsync()
        {
            return await _db.Consulta
                .Include(c => c.Cliente)
                .Include(c => c.Profesional)
                .Include(c => c.Pedido)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<Consulta?> ObtenerConsultaPorIdAsync(int idConsulta)
        {
            return await _db.Consulta
                .Include(c => c.Cliente)
                .Include(c => c.Profesional)
                .Include(c => c.Pedido)
                .FirstOrDefaultAsync(c => c.IdConsulta == idConsulta);
        }

        public async Task GuardarConsultaAsync(Consulta consulta)
        {
            try
            {
                if (consulta.IdConsulta == 0)
                {
                    // Nueva consulta
                    _db.Consulta.Add(consulta);
                }
                else
                {
                    // Actualizar consulta existente
                    var consultaExistente = await _db.Consulta
                        .FirstOrDefaultAsync(c => c.IdConsulta == consulta.IdConsulta);

                    if (consultaExistente != null)
                    {
                        // Actualizar propiedades
                        consultaExistente.IdCliente = consulta.IdCliente;
                        consultaExistente.IdProfesional = consulta.IdProfesional;
                        consultaExistente.IdPedido = consulta.IdPedido;
                        consultaExistente.Fecha = consulta.Fecha;
                        consultaExistente.Motivo = consulta.Motivo;
                        consultaExistente.AntPatoOcular = consulta.AntPatoOcular;

                        // Rx en uso
                        consultaExistente.RxUsoEsferaOD = consulta.RxUsoEsferaOD;
                        consultaExistente.RxUsoEsferaOI = consulta.RxUsoEsferaOI;
                        consultaExistente.RxUsoCilindroOD = consulta.RxUsoCilindroOD;
                        consultaExistente.RxUsoCilindroOI = consulta.RxUsoCilindroOI;
                        consultaExistente.RxUsoEjeOD = consulta.RxUsoEjeOD;
                        consultaExistente.RxUsoEjeOI = consulta.RxUsoEjeOI;
                        consultaExistente.RxUsoAddOD = consulta.RxUsoAddOD;
                        consultaExistente.RxUsoAddOI = consulta.RxUsoAddOI;
                        consultaExistente.RxUsoAvlOD = consulta.RxUsoAvlOD;
                        consultaExistente.RxUsoAvlOI = consulta.RxUsoAvlOI;
                        consultaExistente.RxUsoAvcOD = consulta.RxUsoAvcOD;
                        consultaExistente.RxUsoAvcOI = consulta.RxUsoAvcOI;

                        // Rx Final
                        consultaExistente.RxEsferaOD = consulta.RxEsferaOD;
                        consultaExistente.RxEsferaOI = consulta.RxEsferaOI;
                        consultaExistente.RxCilindroOD = consulta.RxCilindroOD;
                        consultaExistente.RxCilindroOI = consulta.RxCilindroOI;
                        consultaExistente.RxEjeOD = consulta.RxEjeOD;
                        consultaExistente.RxEjeOI = consulta.RxEjeOI;
                        consultaExistente.RxAddOD = consulta.RxAddOD;
                        consultaExistente.RxAddOI = consulta.RxAddOI;
                        consultaExistente.RxDnpOD = consulta.RxDnpOD;
                        consultaExistente.RxDnpOI = consulta.RxDnpOI;
                        consultaExistente.RxAltOD = consulta.RxAltOD;
                        consultaExistente.RxAltOI = consulta.RxAltOI;
                        consultaExistente.RxAvlOD = consulta.RxAvlOD;
                        consultaExistente.RxAvlOI = consulta.RxAvlOI;
                        consultaExistente.RxAvcOD = consulta.RxAvcOD;
                        consultaExistente.RxAvcOI = consulta.RxAvcOI;

                        // Recomendaciones
                        consultaExistente.TipoLente = consulta.TipoLente;
                        consultaExistente.Lubricante = consulta.Lubricante;
                        consultaExistente.AlturaOD = consulta.AlturaOD;
                        consultaExistente.AlturaOI = consulta.AlturaOI;
                        consultaExistente.DiamArmVertical = consulta.DiamArmVertical;
                        consultaExistente.DiamArmHorizontal = consulta.DiamArmHorizontal;
                        consultaExistente.DiamArmPuente = consulta.DiamArmPuente;
                        consultaExistente.DiamArmMayor = consulta.DiamArmMayor;
                        consultaExistente.DiamArmTipo = consulta.DiamArmTipo;
                        consultaExistente.ParPersAP = consulta.ParPersAP;
                        consultaExistente.ParPersCP = consulta.ParPersCP;
                        consultaExistente.ParPersDV = consulta.ParPersDV;
                        consultaExistente.ParPersCorredor = consulta.ParPersCorredor;
                        consultaExistente.Tratamiento = consulta.Tratamiento;

                        // Lentes de contacto - Queratometría
                        consultaExistente.LC_KeraK1OD = consulta.LC_KeraK1OD;
                        consultaExistente.LC_KeraK1OI = consulta.LC_KeraK1OI;
                        consultaExistente.LC_KeraK2OD = consulta.LC_KeraK2OD;
                        consultaExistente.LC_KeraK2OI = consulta.LC_KeraK2OI;

                        // Lentes de contacto - Prueba
                        consultaExistente.LC_LFinRxOD = consulta.LC_LFinRxOD;
                        consultaExistente.LC_LFinRxOI = consulta.LC_LFinRxOI;
                        consultaExistente.LC_LFinTipoOD = consulta.LC_LFinTipoOD;
                        consultaExistente.LC_LFinTipoOI = consulta.LC_LFinTipoOI;
                        consultaExistente.LC_LFinCBOD = consulta.LC_LFinCBOD;
                        consultaExistente.LC_LFinCBOI = consulta.LC_LFinCBOI;
                        consultaExistente.LC_LFinDiamOD = consulta.LC_LFinDiamOD;
                        consultaExistente.LC_LFinDiamOI = consulta.LC_LFinDiamOI;
                        consultaExistente.LC_LFinEspesorOD = consulta.LC_LFinEspesorOD;
                        consultaExistente.LC_LFinEspesorOI = consulta.LC_LFinEspesorOI;
                        consultaExistente.LC_LFinSagitaOD = consulta.LC_LFinSagitaOD;
                        consultaExistente.LC_LFinSagitaOI = consulta.LC_LFinSagitaOI;
                        consultaExistente.LC_LFinDisenoOD = consulta.LC_LFinDisenoOD;
                        consultaExistente.LC_LFinDisenoOI = consulta.LC_LFinDisenoOI;
                        consultaExistente.LC_Diagnostico = consulta.LC_Diagnostico;
                        consultaExistente.LC_Humectante = consulta.LC_Humectante;
                        consultaExistente.LC_SolucLimpieza = consulta.LC_SolucLimpieza;

                        _db.Consulta.Update(consultaExistente);
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar la consulta: {ex.Message}", ex);
            }
        }

        public async Task EliminarConsultaAsync(int idConsulta)
        {
            try
            {
                var consulta = await _db.Consulta
                    .FirstOrDefaultAsync(c => c.IdConsulta == idConsulta);

                if (consulta != null)
                {
                    _db.Consulta.Remove(consulta);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la consulta: {ex.Message}", ex);
            }
        }
    }
}
