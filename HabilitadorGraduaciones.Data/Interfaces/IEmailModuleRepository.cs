using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IEmailModuleRepository
    {
        public Task<BaseOutDto> EnviarCorreo(string destinatario, string asunto, string cuerpo, string adjuntos, string cc, int tipo = 0);
    }
}
