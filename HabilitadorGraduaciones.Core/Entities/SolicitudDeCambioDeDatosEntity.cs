
namespace HabilitadorGraduaciones.Core.Entities
{
    public class SolicitudDeCambioDeDatosEntity
    {
        public int IdSolicitud { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Matricula { get; set; }
        public string PeriodoGraduacion { get; set; }
        public int IdDatosPersonales { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public int IdEstatusSolicitud { get; set; }
        public string Estatus { get; set; }
    }

    public class DetalleSolicitudDeCambioDeDatosEntity
    {
        public int IdSolicitud { get; set; }
        public int NumeroSolicitud { get; set; }
        public int IdDetalleSolicitud { get; set; }
        public int IdDatosPersonales { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string DatoIncorrecto { get; set; }
        public string DatoCorrecto { get; set; }
        public int IdEstatusSolicitud { get; set; }
        public string Estatus { get; set; }
        public string Documento { get; set; }
        public string Extension { get; set; }
        public string AzureStorage { get; set; }

    }

    public class EstatusSolicitudDatosPersonalesEntity
    {
        public int IdEstatusSolicitud { get; set; }
        public string Descripcion { get; set; }

    }
}
