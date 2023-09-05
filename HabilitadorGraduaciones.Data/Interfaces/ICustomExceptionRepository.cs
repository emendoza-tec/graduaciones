using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ICustomExceptionRepository
    {
        public void GuardarExcepcion(BitacoraLog data);
    }
}
