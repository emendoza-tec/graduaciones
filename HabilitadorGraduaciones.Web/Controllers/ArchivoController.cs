using HabilitadorGraduaciones.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivoController : Controller
    {
        private readonly string contenedor = "Templates";
        private readonly IWebHostEnvironment env;
        public ArchivoController(IArchivoStorage fileStore, IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Content("El nombre del archivo está vacío...");
                }

                var filePath = Path.Combine(env.WebRootPath,
                    contenedor, fileName);

                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }

                memoryStream.Position = 0;

                return File(memoryStream, "APPLICATION/octet-stream", Path.GetFileName(filePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}
