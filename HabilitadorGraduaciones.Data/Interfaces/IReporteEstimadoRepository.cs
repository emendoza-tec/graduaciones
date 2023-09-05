using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IReporteEstimadoRepository
    {
        public Task<List<ReporteEstimadoDeGraduacionEntity>> GetReporteEstimadoDeGraduacion(UsuarioAdministradorDto data);
    }
}
