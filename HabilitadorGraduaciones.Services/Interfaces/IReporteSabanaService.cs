using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IReporteSabanaService
    {
        public Task<List<SabanaEntity>> GetReporteSabana(UsuarioAdministradorDto data);
    }
}
