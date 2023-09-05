using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.Utils
{
    public class ArchivoLocalStorageService : IArchivoLocalStorageService
    {
        private readonly IHostingEnvironment _env;

        public ArchivoLocalStorageService(IHostingEnvironment env)
        {
            this._env = env;
        }

        public Task DeleteFile(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }

            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(_env.WebRootPath, contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditFile(string contenedor, IFormFile archivo, string ruta)
        {
            await DeleteFile(ruta, contenedor);
            return await SaveFile(contenedor, archivo);
        }

        public async Task<string> SaveFile(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
            {
                _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            string folder = Path.Combine(_env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            var rutaParaDB = Path.Combine(contenedor, nombreArchivo).Replace("\\", "/");

            return rutaParaDB;
        }
    }
}
