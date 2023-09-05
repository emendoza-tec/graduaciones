using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class PeriodosDto : BaseOutDto
    {
        public string PeriodoId { get; set; }
        public string Matricula { get; set; }
        public string Descripcion { get; set; }
        public bool IsRegular { get; set; }
        public string PeriodoElegido { get; set; }
        public string PeriodoEstimado { get; set; }
        public string PeriodoCeremonia { get; set; }
        public string MotivoCambioPeriodo { get; set; }
        public string EleccionAsistenciaCeremonia { get; set; }
        public string MotivoNoAsistirCeremonia { get; set; }
        public int OrigenActualizacionPeriodoId { get; set; }

    }
}
