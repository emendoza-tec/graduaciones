namespace HabilitadorGraduaciones.Core.DTO
{
    public class NivelInglesDto : Base.BaseOutDto
    {
        public string Matricula { get; set; } = string.Empty;

        public string Estatus { get; set; }
        public string NivelIdiomaAlumno { get; set; }
        public string RequisitoNvl { get; set; }
        public string NivelIdiomaRequisito { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public Boolean NivelCumple { get; set; }
    }

    public class DetalleNivelInglesDto : Base.BaseOutDto
    {
        public string Titulo { get; set; }
        public List<Documento> Documentos { get; set; }
        public string Detalle { get; set; }
        public string EstadoDetalle { get; set; }
        public Duda Dudas { get; set; }
    }
}