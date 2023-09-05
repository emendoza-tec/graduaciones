using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ISesionRepository
    {
        public Task<Sesion> GetSesion(string matricula);
        public Task GuardaSesion(Sesion sesion);
        public Task ModificaSesion(Sesion sesion);
    }
}
