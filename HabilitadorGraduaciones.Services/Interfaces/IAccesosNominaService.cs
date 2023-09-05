using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IAccesosNominaService
    {
        public Task<AccesosNominaEntity> GetAcceso(string matricula);
    }
}
