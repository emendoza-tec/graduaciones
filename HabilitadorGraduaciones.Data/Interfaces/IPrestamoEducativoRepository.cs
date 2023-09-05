using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IPrestamoEducativoRepository
    {
        public Task<PrestamoEducativoDto> GetPrestamoEducativo(PrestamoEducativoEntity entity);
    }
}
