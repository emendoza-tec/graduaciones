using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IExamenConocimientosService
    {
        public Task<ExamenConocimientosDto> GetExamenConocimiento(EndpointsDto dto);
        public Task<TipoExamenConocimientosEntity> GetExamenConocimientoPorLenguaje(int tipoExamen, string lenguaje);
    }
}
