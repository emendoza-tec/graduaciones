using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class NivelInglesEntity : BaseEntity
    {
        public string Estatus { get; set; }
        public string NivelIdiomaAlumno { get; set; }
        public string RequisitoNvl { get; set; }
        public string NivelIdiomaRequisito { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public Boolean NivelCumple { get; set; }
        public string Programa { get; set; }
    }

    public class ConfiguracionNivelInglesEntity
    {
        public string IdNivelIngles { get; set; }
        public string ClaveProgramaAcademico { get; set; }
        public string IdUsuario { get; set; }
    }
}