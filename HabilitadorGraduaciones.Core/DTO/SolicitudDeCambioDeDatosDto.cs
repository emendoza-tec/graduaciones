using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class SolicitudDeCambioDeDatosDto : BaseOutDto
    {
        public int IdSolicitud { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Matricula { get; set; }
        public string PeriodoGraduacion { get; set; }
        public int IdDatosPersonales { get; set; }
        public string DatoIncorrecto { get; set; }
        public string DatoCorrecto { get; set; }
        public Boolean OK { get; set; }
        public string Error { get; set; }
        public List<DetalleSolicitudDeCambioDeDatosDto> Detalle { get; set; }
        public int IdCorreo { get; set; }

    }
    public class DetalleSolicitudDeCambioDeDatosDto
    {
        public int IdDetalleSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public string Archivo { get; set; }
        public string Documento { get; set; }
        public string Extension { get; set; }
        public string AzureStorage { get; set; }
        public Boolean OK { get; set; }
        public string Error { get; set; }
    }

    public class ModificarEstatusSolicitudDto
    {
        public int IdSolicitud { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Matricula { get; set; }
        public int IdEstatusSolicitud { get; set; }
        public string UsarioRegistro { get; set; }
        public string Comentarios { get; set; }
        public Boolean OK { get; set; }
        public string Error { get; set; }
        public int IdCorreo { get; set; }
    }

    public class TotalSolicitudesDto
    {
        public int Total { get; set; }
    }

    public class CorreoSolicitudDatosPersonalesDto
    {
        public int IdCorreo { get; set; }
        public int IdSolicitud { get; set; }
        public string Destinatario { get; set; }
        public string Comentarios { get; set; }
        public Boolean Enviado { get; set; }
        public string Nombre { get; set; }

    }
}
