namespace HabilitadorGraduaciones.Core.DTO
{
    public class DatosPersonalesCorreoDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Curp { get; set; }
        public string ProgramaAcademico { get; set; }
        public string Correo { get; set; }
        public List<string> Concentracion { get; set; }
        public List<string> DiplomasAdicionales { get; set; }
    }
}
