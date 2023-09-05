using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities.Expediente;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IExpedienteRepository
    {
        public Task<List<ExpedienteEntity>> GetExpedientes(int idUsuario);
        public Task<ExpedienteEntity> GetByAlumno(string matricula);
        public Task<List<ExpedienteEntity>> ConsultarComentarios(string Matricula);
        public Task GuardaExpedientes(List<ExpedienteEntity> expedientes, string usuarioAplicacion);
        public Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula);
    }
}
