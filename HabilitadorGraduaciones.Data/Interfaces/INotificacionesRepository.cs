using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface INotificacionesRepository
    {
        public Task<NotificacionesDto> GetNotificaciones(Notificacion notificacion);
        public Task<NotificacionesDto> InsertarNotificacion(Notificacion notificacion);
        public Task<NotificacionesDto> InsertarNotificacionCorreo(NotificacionCorreoDto data);
        public Task<NotificacionesDto> BienvenidoGraduacion(string matricula, int tipoCorreo);
        public Task<NotificacionesDto> IsCorreoEnviado(int tipo, string matricula);
        public Task<NotificacionesDto> EnteradoCreditosInsuficientes(string matricula);
        public Task<BaseOutDto> EnviaCorreoConfirmacion(DatosPersonalesCorreoDto datos);
    }
}
