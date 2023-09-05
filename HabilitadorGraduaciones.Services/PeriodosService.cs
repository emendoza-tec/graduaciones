using AutoMapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class PeriodosService : IPeriodosService
    {
        private readonly IPeriodosRepository _periodosData;
        private readonly IPlanDeEstudiosService _planDeEstudiosService;

        public PeriodosService(IMapper mapper, ISesionRepository sessionData,
            IPeriodosRepository periodosData, IPlanDeEstudiosService planDeEstudiosService)
        {
            _periodosData = periodosData;
            _planDeEstudiosService = planDeEstudiosService;
        }
        public async Task<List<PeriodosEntity>> GetPeriodos(EndpointsDto dto)
        {
            PeriodosEntity entity = new PeriodosEntity();
            entity.Matricula = dto.NumeroMatricula;
            var result = await GetPeriodos(entity);
            var calculo = await CalcularPeriodos(result.Where(x => x.IsRegular).ToList(), dto);
            var clinicas = await GetClinicas();

            List<PeriodosEntity> periodos;
            if (!clinicas.Exists(x => x.Carrera == dto.ClaveCarrera))
            {
                if (dto.ClaveProgramaAcademico != null)
                {
                    periodos = result.Where(x => Convert.ToInt32(x.PeriodoId) >= Convert.ToInt32(calculo.First().PeriodoId) && x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Semestral)).ToList();
                }
                else
                {
                    periodos = result.Where(x => Convert.ToInt32(x.PeriodoId) > Convert.ToInt32(dto.ClaveEjercicioAcademico) && x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Semestral)).ToList();
                }
            }
            else
            {
                if (dto.ClaveProgramaAcademico != null)
                {
                    periodos = result.Where(x => Convert.ToInt32(x.PeriodoId) >= Convert.ToInt32(calculo.First().PeriodoId) && x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Clinicas)).ToList();
                }
                else
                {
                    periodos = result.Where(x => Convert.ToInt32(x.PeriodoId) > Convert.ToInt32(dto.ClaveEjercicioAcademico) && x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Clinicas)).ToList();

                }
            }
            var lista = periodos.Count > 0 ? periodos.Take(5).ToList() : new List<PeriodosEntity>();
            return lista;
        }

        public async Task<PeriodosEntity> GetPeriodoPronostico(EndpointsDto dto)
        {
            PeriodosEntity entity = new PeriodosEntity();
            entity.Matricula = dto.NumeroMatricula;
            var result = await GetPeriodos(entity);
            result = result.Where(x => x.IsRegular).ToList();
            var calculo = await CalcularPeriodos(result, dto);
            return calculo.Count > 0 ? calculo.First() : new PeriodosEntity();
        }
        public async Task<PeriodosEntity> GetPeriodoAlumno(string matricula)
        {
            return await _periodosData.GetPeriodoAlumno(matricula);
        }
        public async Task<BaseOutDto> GuardarPeriodo(PeriodosDto data)
        {
            return await _periodosData.GuardarPeriodo(data);
        }

        public async Task<List<PeriodosEntity>> GetPeriodos(PeriodosEntity data)
        {
            return await _periodosData.GetPeriodos(data);
        }
        public async Task<List<ConfiguracionClinicasDto>> GetClinicas()
        {
            return await _periodosData.GetClinicas();
        }

        public async Task<ConfiguracionClinicasDto> GetCongfiguracionClinicas(string carrera)
        {
            return await _periodosData.GetCongfiguracionClinicas(carrera);
        }

        public async Task<List<PeriodoGraduacionEntity>> GetPeriodosGraduacion()
        {
            return await _periodosData.GetPeriodosGraduacion();
        }

        private async Task<List<PeriodosEntity>> CalcularPeriodos(List<PeriodosEntity> lista, EndpointsDto dto)
        {
            List<PeriodosEntity> result = new List<PeriodosEntity>();
            List<PeriodosEntity> listaSemestral = lista.Where(x => x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Semestral)).ToList();
            List<PeriodosEntity> listaClinicas = lista.Where(x => x.TipoPeriodo == Convert.ToInt32(TipoPeriodos.Clinicas)).ToList();
            decimal periodosFaltantes = 0;
            var configuracionClinicas = await GetCongfiguracionClinicas(dto.ClaveCarrera);
            var clinicas = await GetClinicas();
            var creditosTotales = configuracionClinicas.CantidadTrimestres * configuracionClinicas.CantidadPeriodos;

            var creditosConsulta = await _planDeEstudiosService.GetPlanDeEstudios(dto);

            var creditos = creditosConsulta;
            decimal creditosPerido = 0;

            var creditosFaltantes = (creditos.CreditosRequisito - creditos.CreditosAcreditados - creditos.CreditosInscritos);
            creditosFaltantes = creditosFaltantes >= 0 ? creditosFaltantes : 0;

            if (!clinicas.Exists(x => x.Carrera == dto.ClaveCarrera))
            {
                creditosPerido = Convert.ToDecimal(listaSemestral.Select(x => x.CreditosPeriodo).First());
                periodosFaltantes = Math.Round(creditosFaltantes / creditosPerido);
                listaSemestral.RemoveRange(0, (int)periodosFaltantes);
                result = listaSemestral;
            }
            else if (listaClinicas.Count > 0 && listaSemestral.Count > 0)
            {
                if (creditosFaltantes >= creditosTotales)
                {
                    creditosPerido = Convert.ToDecimal(listaSemestral.Select(x => x.CreditosPeriodo).First());
                    var diff = creditosFaltantes - creditosTotales;
                    periodosFaltantes = Math.Round(diff / creditosPerido);
                    var aux = listaSemestral.Where(x => int.Parse(x.PeriodoId) <= listaClinicas.Select(x => int.Parse(x.PeriodoId)).First()).OrderByDescending(x => x.PeriodoId).ToList();
                    result = aux.Take((int)periodosFaltantes).ToList();
                    result.AddRange(listaClinicas.Where(x => x.FechaInicio > aux.Select(x => x.FechaInicio).First()));
                    result = result.OrderBy(x => x.TipoPeriodo).ThenBy(x => x.FechaInicio).ToList();
                }
                else
                {
                    creditosPerido = Convert.ToDecimal(listaClinicas.Select(x => x.CreditosPeriodo).First());
                    periodosFaltantes = Math.Round(creditosFaltantes / creditosPerido);
                    listaClinicas.RemoveRange(0, (int)periodosFaltantes);
                    result = listaClinicas;
                }
            }
            return result;
        }
        public async Task<BaseOutDto> EnviarCorreo(UsuarioDto correo)
        {
            return await _periodosData.EnviarCorreo(correo);

        }
    }
}
