using HabilitadorGraduaciones.Core.DTO;
namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IPermisosNominaRepository
    {
        public Task<PermisosNominaDto> ObtenerPermisosPorNomina(string nomina);
    }
}
