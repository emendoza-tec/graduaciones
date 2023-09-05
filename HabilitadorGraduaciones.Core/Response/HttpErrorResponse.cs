using System.Diagnostics.CodeAnalysis;

namespace HabilitadorGraduaciones.Core.Response
{
    public class HttpErrorResponse
    {
        public int HttpStatus { get; set; }
        public string Mensaje { get; set; }
    }
}
