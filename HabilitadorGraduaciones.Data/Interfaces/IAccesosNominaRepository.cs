using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IAccesosNominaRepository
    {
        public Task<AccesosNominaEntity> GetAcceso(string matricula);
        public Task<AccesosNominaEntity> GetAccesoUsuarioAdmin(string nomina);
    }
}
