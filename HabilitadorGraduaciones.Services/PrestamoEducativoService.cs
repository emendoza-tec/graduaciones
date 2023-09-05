using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class PrestamoEducativoService : IPrestamoEducativoService
    {
        private readonly IPrestamoEducativoRepository _prestamoEducativoData;

        public PrestamoEducativoService(IPrestamoEducativoRepository prestamoEducativoData)
        {
            _prestamoEducativoData = prestamoEducativoData;
        }

        public async Task<PrestamoEducativoDto> GetPrestamoEducativo(PrestamoEducativoEntity entity)
        {
            return await _prestamoEducativoData.GetPrestamoEducativo(entity);
        }
    }
}
