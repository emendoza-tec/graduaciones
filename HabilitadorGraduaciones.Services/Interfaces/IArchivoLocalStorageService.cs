using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IArchivoLocalStorageService
    {
        Task DeleteFile(string ruta, string contenedor);
        Task<string> EditFile(string contenedor, IFormFile archivo, string ruta);
        Task<string> SaveFile(string contenedor, IFormFile archivo);
    }
}
