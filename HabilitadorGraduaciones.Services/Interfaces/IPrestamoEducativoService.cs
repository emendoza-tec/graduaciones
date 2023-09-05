using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IPrestamoEducativoService
    {
        public Task<PrestamoEducativoDto> GetPrestamoEducativo(PrestamoEducativoEntity entity);
    }
}
