using AutoMapper;
using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class PlanDeEstudiosService : IPlanDeEstudiosService
    {
        public readonly IPlanDeEstudiosRepository _planDeEstudiosRepository;
        private readonly IApiService _apiService;

        public PlanDeEstudiosService(IPlanDeEstudiosRepository planDeEstudiosRepository, IApiService apiService)
        {
            _planDeEstudiosRepository = planDeEstudiosRepository;
            _apiService = apiService;
        }

        #region Método para mostrar el Plan de Estudio en la vista de la tarjeta
        public async Task<PlanDeEstudiosDto> GetPlanDeEstudios(EndpointsDto dto)
        {
            PlanDeEstudiosDto planDeEstudios = new PlanDeEstudiosDto();
            try
            {
                Sesion sesion = await _apiService.VerificaTokenUsuario(dto.NumeroMatricula);

                planDeEstudios = await _planDeEstudiosRepository.ConsultarApiPlanDeEstudios(dto, sesion);
                planDeEstudios.CreditosAcreditados = planDeEstudios.CreditosPorCampus.Sum(x => x.CreditosCampus);
                if (planDeEstudios.CreditosRequisito > 0 && (planDeEstudios.CreditosRequisito - planDeEstudios.CreditosAcreditados) <= 0)
                {
                    planDeEstudios.isCumplePlanDeEstudios = true;
                }
                else
                {
                    planDeEstudios.isCumplePlanDeEstudios = false;
                }
                planDeEstudios.Result = true;
            }
            catch (Exception ex)
            {
                planDeEstudios.Result = false;
                planDeEstudios.ErrorMessage = ex.Message;
                throw new CustomException("Error en obtener los créditos del Plan de estudios", ex);
            }
            return planDeEstudios;
        }
        #endregion
    }
}