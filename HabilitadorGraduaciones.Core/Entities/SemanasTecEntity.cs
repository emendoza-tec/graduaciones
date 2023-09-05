using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class SemanasTecEntity : BaseEntity
    {
        public string ClaveNivelAcademico { get; set; }
        public int SemanasObtenidas { get; set; }
        public int SemanasMaximas { get; set; }
        public DateTime ultimaActualizacion { get; set; }
        public string NumeroMatricula { get; set; } = string.Empty;

    }
}