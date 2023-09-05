using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IExamenConocimientosRepository
    {
        public Task<ExamenConocimientosDto> GetCeneval(EndpointsDto dto, Sesion sesion);

        public Task<TipoExamenPorCarreraDto> GetTipoExamenPorCarrera(string claveCarrera);

        public Task<ExamenConocimientosDto> GetExamenConocimientoPorTipo(string matricula, int tipoExamen);

        public Task<TipoExamenConocimientosEntity> GetExamenConocimientoPorLenguaje(int tipoExamen, string lenguaje);
    }
}