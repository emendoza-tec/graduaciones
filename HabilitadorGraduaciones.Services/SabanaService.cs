using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class SabanaService : IReporteSabanaService
    {
        private readonly IReporteSabanaRepository _reporteSabanaRepository;

        public SabanaService(IReporteSabanaRepository reporteSabanaRepository)
        {
            _reporteSabanaRepository = reporteSabanaRepository;
        }

        public async Task<List<SabanaEntity>> GetReporteSabana(UsuarioAdministradorDto data)
        {
            return await _reporteSabanaRepository.GetReporteSabana(data);
        }
    }
}
