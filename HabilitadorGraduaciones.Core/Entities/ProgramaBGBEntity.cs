﻿using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ProgramaBgbEntity : BaseEntity
    {
        public bool CumpleRequisitoInglesCuarto { get; set; }
        public bool CumpleRequisitoInglesQuinto { get; set; }
        public bool CumpleRequisitoInglesOctavo { get; set; }
        public bool CumpleProgramaInternacional { get; set; }
        public bool CumpleRequisitosProgramaEspecial { get; set; }
        public int CreditosDeInglesAprobadosCuartoSemestre { get; set; }
        public int CreditosDeInglesAprobadosQuintoSemestre { get; set; }
        public int CreditosDeInglesAprobadosOctavoSemestre { get; set; }
        public int CreditosAprobadosProgramaInternacional { get; set; }
        public string ClavePrograma { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
