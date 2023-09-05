namespace HabilitadorGraduaciones.Core.DTO
{
    public class CenevalDto : Base.BaseOutDto
    {
        public bool CumpleRequisitoCeneval { get; set; }
        public DateTime FechaExamen { get; set; }
        public string FechaRegistro { get; set; }
        public bool EsRequisito { get; set; }
    }
}