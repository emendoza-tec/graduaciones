using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class ReporteEstimadoDeGraduacionService : IReporteEstimadoService
    {
        private readonly IReporteEstimadoRepository _reporteEstimadoRepository;

        public ReporteEstimadoDeGraduacionService(IReporteEstimadoRepository reporteEstimadoRepository)
        {
            _reporteEstimadoRepository = reporteEstimadoRepository;
        }

        public async Task<List<ReporteEstimadoDeGraduacionEntity>> GetReporteEstimadoDeGraduacion(UsuarioAdministradorDto data)
        {
            return await _reporteEstimadoRepository.GetReporteEstimadoDeGraduacion(data);
        }

    }
}
