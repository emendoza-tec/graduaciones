using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class LogEnteradoDto : BaseOutDto
    {
        public string Matricula { get; set; }
        public string Periodo { get; set; }
        public string PeriodoId { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
