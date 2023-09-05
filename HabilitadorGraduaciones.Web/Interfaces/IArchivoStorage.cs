namespace HabilitadorGraduaciones.Web.Interfaces
{
    public interface IArchivoStorage
    {
        Task DeleteFile(string ruta, string contenedor);
        Task<string> EditFile(string contenedor, IFormFile archivo, string ruta);
        Task<string> SaveFile(string contenedor, IFormFile archivo);
    }
}