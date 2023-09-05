namespace HabilitadorGraduaciones.Core.Entities.Bases
{
    public class Aviso
    {
        public Aviso() { }
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string UrlImage { get; set; }
        public string Matricula { get; set; }
        public string CampusId { get; set; }
        public string SedeId { get; set; }
        public string RequisitoId { get; set; }
        public string RequisitoEstatus { get; set; }
        public string ProgramaId { get; set; }
        public string EscuelasId { get; set; }
        public string Cc_rolesId { get; set; }
        public string Cc_camposId { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Correo { get; set; }
        public bool Habilitador { get; set; }
        public int Cumple { get; set; }
        public int IdUsuario { get; set; }
    }
}