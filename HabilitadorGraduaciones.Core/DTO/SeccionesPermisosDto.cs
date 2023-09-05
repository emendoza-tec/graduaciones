namespace HabilitadorGraduaciones.Core.DTO
{
    public class SeccionesPermisosDto
    {
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public int IdSubMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public bool Ver { get; set; }
        public bool Editar { get; set; }
        public bool Activa { get; set; }
        public bool Result { get; set; }
    }
}
