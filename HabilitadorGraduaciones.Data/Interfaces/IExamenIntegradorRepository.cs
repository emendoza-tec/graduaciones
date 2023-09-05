using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IExamenIntegradorRepository
    {
        public Task<List<ExamenIntegradorEntity>> GetExamenesIntegrador(int idUsuario);
        public Task<ExamenIntegradorEntity> GetMatricula(string matricula);
        public Task GuardaExamenesIntegrador(List<ExamenIntegradorEntity> expedientes, string usuarioAplicacion);
        public Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula);
    }
}
