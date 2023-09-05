using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class AccesosNominaService : IAccesosNominaService
    {
        private readonly IAccesosNominaRepository _accesosNominaData;

        public AccesosNominaService(IAccesosNominaRepository accesosNominaData)
        {
            _accesosNominaData = accesosNominaData;
        }

        public async Task<AccesosNominaEntity> GetAcceso(string matricula)
        {
            AccesosNominaEntity result = await _accesosNominaData.GetAcceso(matricula);
            if (result.Acceso && matricula.StartsWith('L'))
            {
                result = await _accesosNominaData.GetAccesoUsuarioAdmin(matricula);
            }
            return result;
        }
    }
}