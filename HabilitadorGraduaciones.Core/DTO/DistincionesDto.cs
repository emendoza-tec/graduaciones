using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class DistincionesDto : BaseOutDto
    {
        public List<string> LstConcentracion { get; set; }
        public string Diploma { get; set; }
        public string Ulead { get; set; }
        public bool DiplomaOk { get; set; }
        public bool UleadOk { get; set; }
        public bool HasDiploma { get; set; }
        public bool HasUlead { get; set; }
        public string NumeroMatricula { get; set; }
        public string ClaveEjercicioAcademico { get; set; }
        public string ClaveNivelAcademico { get; set; }


    }
}