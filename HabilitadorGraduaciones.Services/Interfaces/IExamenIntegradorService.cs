using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IExamenIntegradorService
    {
        public Task<List<ExamenIntegradorEntity>> GetExamenesIntegrador(int idUsuario);
        public Task<ProcesosExamenIntegradorEntity> ProcesaExcel(IFormFile archivo, int idUsuario);
        public Task<HttpResponseMessage> GuardaExamenesIntegrador(ArchivoEntity archivo, int idUsuario);
        public Task<ExamenIntegradorEntity> GetMatricula(string matricula);
        public Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula);
    }
}
