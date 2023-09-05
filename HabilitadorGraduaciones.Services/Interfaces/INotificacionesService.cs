using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface INotificacionesService
    {
        public Task<BaseOutDto> EnviarCorreo(CorreoDto correo);
        public Task<NotificacionesDto> GetNotificaciones(Notificacion param);
        public Task<NotificacionesDto> ActualizarEstatus(Notificacion _object);
        public Task<BaseOutDto> MarcarTodasLeidas(List<Notificacion> lista);
        public Task<NotificacionesDto> InsertarNotificacion(Notificacion _object);
        public Task<NotificacionesDto> InsertarNotificacionCorreo(NotificacionCorreoDto data);
        public Task<NotificacionesDto> BienvenidoGraduacion(string matricula, int tipoCorreo);
        public Task<NotificacionesDto> IsCorreoEnviado(int tipo, string matricula);
        public Task<NotificacionesDto> EnteradoCreditosInsuficientes(string matricula);
        public Task<BaseOutDto> EnviaCorreoConfirmacion(DatosPersonalesCorreoDto datos);
    }
}
