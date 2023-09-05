namespace HabilitadorGraduaciones.Core.Entities
{
    public class SemanasTecExtEntity : TarjetaDetalle
    {
        public SemanasTecExtEntity()
        {
            LinkDudas = new List<string>();
        }
        public List<String> LinkDudas { get; set; }
        public string MensajeModal { get; set; }
    }
}