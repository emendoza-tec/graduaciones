using System.Net;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class BitacoraLog
    {
        public bool ErrorControlado { get; set; }
        public string MensajeUsuario { get; set; }
        public string MensajeExcepcion { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public int HResult { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string UsuarioAlta { get; set; }
    }
}
