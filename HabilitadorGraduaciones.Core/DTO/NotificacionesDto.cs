using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class NotificacionesDto : BaseOutDto
    {
        public List<Notificacion> ListaNotificaciones { get; set; }
    }
    public class Notificacion
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool IsModificarNotificacion { get; set; }
        public bool IsModificarTodas { get; set; }
        public bool IsNotificacion { get; set; }
        public bool IsConsultaNotificacionesNoLeidas { get; set; }
        public string ListNotificacionesLeidas { get; set; }

    }
}
