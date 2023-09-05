using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IAlmacenadorAzureStorageService
    {
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}
