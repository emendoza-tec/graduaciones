using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IPermisosNominaService
    {
        public Task<PermisosNominaDto> ObtenerPermisosPorNomina(string nomina);
    }
}
