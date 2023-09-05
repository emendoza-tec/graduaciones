using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class SemanasTecDto : BaseOutDto
    {
        public int SemanasObtenidas { get; set; }
        public int SemanasMaximas { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public string ClaveNivelAcademico { get; set; }
        public string ClaveProgramaAcademico { get; set; }
        public string NumeroMatricula { get; set; }
    }
}