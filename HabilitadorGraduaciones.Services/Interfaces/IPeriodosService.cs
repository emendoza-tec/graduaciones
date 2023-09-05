using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IPeriodosService
    {
        public Task<List<PeriodosEntity>> GetPeriodos(EndpointsDto dto);

        public Task<PeriodosEntity> GetPeriodoPronostico(EndpointsDto dto);

        public Task<PeriodosEntity> GetPeriodoAlumno(string matricula);

        public Task<BaseOutDto> GuardarPeriodo(PeriodosDto data);
      
        public Task<BaseOutDto> EnviarCorreo(UsuarioDto correo);
    }

}
