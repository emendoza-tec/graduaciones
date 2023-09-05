namespace HabilitadorGraduaciones.Core.DTO
{
    public class ExpedienteOutDto : Base.BaseOutDto
    {
        public string Estatus { get; set; }
        public string Detalle { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    public class DetalleExpedienteDto : Base.BaseOutDto
    {
        public string Titulo { get; set; }
        public List<Documento> Documentos { get; set; }
        public string Detalle { get; set; }
        public string EstadoDetalle { get; set; }
        public Duda Dudas { get; set; }
    }

    public class Documento
    {
        public string Descripcion { get; set; }
    }

    public class Duda
    {
        public List<Contacto> Contactos { get; set; }
    }

    public class Contacto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}