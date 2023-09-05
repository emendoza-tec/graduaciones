using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IReporteSabanaRepository
    {
        public Task<List<SabanaEntity>> GetReporteSabana(UsuarioAdministradorDto data);
    }
}