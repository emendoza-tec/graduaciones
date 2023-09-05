using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class DistincionesEntity : BaseEntity
    {
        public List<string> LstConcentracion { get; set; } = new();
        public string Diploma { get; set; }
        public string Ulead { get; set; }
        public bool DiplomaOk { get; set; }
        public bool UleadOk { get; set; }
        public bool HasDiploma { get; set; }
        public bool HasUlead { get; set; }
        public string ClaveNivelAcademico { get; set; }
        public string ClaveEjercicioAcademico { get; set; }
    }
}