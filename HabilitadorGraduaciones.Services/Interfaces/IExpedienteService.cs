using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IExpedienteService
    {
        public Task<ExpedienteEntity> GetByAlumno(string matricula);
        public Task<List<ExpedienteEntity>> GetExpedientes(int idUsuario);
        public Task<List<ExpedienteEntity>> ConsultarComentarios(string matricula);
        public Task<ProcesosExpedienteEntity> ProcesaExcel(IFormFile archivo, int idUsuario);
        public Task<HttpResponseMessage> GuardaExpedientes(ArchivoEntity archivo, int idUsuario);
        public Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula);
    }
}
