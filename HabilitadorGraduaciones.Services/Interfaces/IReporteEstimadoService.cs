using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IReporteEstimadoService
    {
        public Task<List<ReporteEstimadoDeGraduacionEntity>> GetReporteEstimadoDeGraduacion(UsuarioAdministradorDto data);
    }
}
