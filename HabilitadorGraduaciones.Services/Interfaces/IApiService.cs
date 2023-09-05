using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IApiService
    {
        public Task<Sesion> VerificaTokenUsuario(string matricula);
    }
}
