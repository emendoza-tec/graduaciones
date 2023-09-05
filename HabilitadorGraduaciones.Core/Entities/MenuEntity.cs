namespace HabilitadorGraduaciones.Core.Entities
{
    public class MenuEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Path { get; set; }
        public string Icono { get; set; }
        public int IdMenu { get; set; }
        public bool Result { get; set; }
        public List<MenuHijoEntity> MenuHijo { get; set; }
    }

    public class MenuHijoEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Path { get; set; }
        public string Icono { get; set; }
        public int IdMenu { get; set; }
        public bool Result { get; set; }
    }
}