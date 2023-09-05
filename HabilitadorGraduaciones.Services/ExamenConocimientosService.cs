using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class ExamenConocimientosService : IExamenConocimientosService
    {
        private readonly IExamenConocimientosRepository _examenConocimientosData;
        private readonly IApiService _apiService;

        public ExamenConocimientosService(IExamenConocimientosRepository examenConocimientosData,
                    IApiService apiService)
        {
            _examenConocimientosData = examenConocimientosData;
            _apiService = apiService;
        }

        public async Task<ExamenConocimientosDto> GetExamenConocimiento(EndpointsDto dto)
        {
            Sesion sesion = await _apiService.VerificaTokenUsuario(dto.NumeroMatricula);

            ExamenConocimientosDto dtoExamen = new();
            TipoExamenPorCarreraDto dtoTipoExamen = await _examenConocimientosData.GetTipoExamenPorCarrera(dto.ClaveCarrera);

            if (dtoTipoExamen.IdTipoExamen == 0)
            {
                dtoExamen.FechaRegistro = DateTime.Now;
                dtoExamen.DescripcionExamen = "Carrera Exenta";
                dtoExamen.TituloExamen = "Examen de Conocimientos";
                dtoExamen.Result = true;
                return dtoExamen;
            }
            else if (dtoTipoExamen.IdTipoExamen == (int)TipoExamen.CENEVAL)
            {
                dtoExamen = await _examenConocimientosData.GetCeneval(dto, sesion);
            }
            else
            {
                dtoExamen = await _examenConocimientosData.GetExamenConocimientoPorTipo(dto.NumeroMatricula, dtoTipoExamen.IdTipoExamen);
            }

            dtoExamen.IdTipoExamen = dtoTipoExamen.IdTipoExamen;
            dtoExamen.DescripcionExamen = dtoTipoExamen.Descripcion;
            dtoExamen.TituloExamen = dtoTipoExamen.Titulo;

            return dtoExamen;
        }

        public async Task<TipoExamenConocimientosEntity> GetExamenConocimientoPorLenguaje(int tipoExamen, string lenguaje)
        {
            return await _examenConocimientosData.GetExamenConocimientoPorLenguaje(tipoExamen, lenguaje);
        }
    }
}
