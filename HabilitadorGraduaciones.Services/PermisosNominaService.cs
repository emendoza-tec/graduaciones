using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class PermisosNominaService : IPermisosNominaService
    {
        private readonly IPermisosNominaRepository _permisosNominaData;

        public PermisosNominaService(IPermisosNominaRepository permisosNominaData)
        {
            _permisosNominaData = permisosNominaData;
        }

        public async Task<PermisosNominaDto> ObtenerPermisosPorNomina(string nomina)
        {
            return await _permisosNominaData.ObtenerPermisosPorNomina(nomina);
        }
    }
}
