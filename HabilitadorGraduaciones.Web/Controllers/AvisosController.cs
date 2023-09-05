using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Bases;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        private readonly IAvisosService _avisosService;
        private readonly IArchivoStorage _fileStore;
        public AvisosController(IAvisosService avisosService, IArchivoStorage fileStore)
        {
            _avisosService = avisosService;
            this._fileStore = fileStore;
        }
        //Test

        /// <summary>
        /// Endpoint que guarda un Archivo
        /// </summary>
        /// <param name="Image">Objeto de tipo IFormFile.</param>
        /// <returns>Un string con el Url del objeto</returns>
        // GET: api/<AvisosController>
        [HttpPost("GuardarArchivo")]
        public async Task<ActionResult<object>> GuardarArchivo(IFormFile Image)
        {
            string folder = "AvisosImagenes";
            try
            {
                if (Image == null || Image.Length == 0)
                {
                    return Content("File not selected");
                }
                var rutaDB = await _fileStore.SaveFile(folder, Image);
                return new { res = rutaDB, result = true };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }


        /// <summary>
        /// Endpoint que guarda un nuevo Aviso
        /// </summary>
        /// <param name="entity">Objeto de tipo Avisos.</param>
        /// <returns>Un Objeto que hereda de BaseOutDto</returns>
        // GET: api/<AvisosController>
        [HttpPost]
        public async Task<ActionResult<BaseOutDto>> PostAvisos(Aviso aviso)
        {
            AvisoGuardar entity = new()
            {
                Aviso = aviso
            };
            entity.Aviso.Id = 0;
            entity.Aviso.FechaCreacion = DateTime.Now;
            BaseOutDto result = await _avisosService.SetAvisosService(entity);
            return Ok(result);
        }
        /// <summary>
        /// Endpoint que obtiene la información de Avisos
        /// </summary>
        /// <param name="id">Matrícula del Alumno seleccionado.</param>
        /// <returns>Un Objeto que hereda de BaseOutDto</returns>
        // GET: api/<AvisosController>
        [HttpGet("{id}")]
        public async Task<ActionResult<AvisosDto>> GetAvisos(string id)
        {
            AvisosEntity entity = new()
            {
                Matricula = id
            };
            AvisosDto result = await _avisosService.Get3AvisosService(entity);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint que obtiene la información del historial de avisos
        /// </summary>
        /// <param name="id">Matrícula del Alumno seleccionado.</param>
        /// <returns>Un Objeto que hereda de BaseOutDto</returns>
        // GET: api/historial/<AvisosController>
        [HttpGet("historial/{id}")]
        public async Task<ActionResult<AvisosDto>> GetAvisosHistorial(string id)
        {
            AvisosEntity entity = new()
            {
                Matricula = id
            };
            AvisosDto result = await _avisosService.GetAvisosService(entity);
            return Ok(result);
        }


        /// <summary>
        /// Endpoint que obtiene las filtros para la pantalla de avisos
        /// </summary>
        /// <returns>Un Objeto que hereda de BaseOutDto</returns>
        // GET: api/filtros/<AvisosController>
        [HttpGet("filtros")]
        public async Task<ActionResult<FiltrosDto>> GetFiltros()
        {
            List<CatalogoDto> nivel = await _avisosService.ObtenerCatalogo((int)Filtros.Nivel);
            List<CatalogoDto> campus = await _avisosService.ObtenerCatalogo((int)Filtros.Campus);
            List<CatalogoDto> sedes = await _avisosService.ObtenerCatalogo((int)Filtros.Sedes);
            List<CatalogoDto> escuelas = await _avisosService.ObtenerCatalogo((int)Filtros.Escuelas);
            List<CatalogoDto> programas = await _avisosService.ObtenerCatalogo((int)Filtros.Programas);
            List<CatalogoDto> requisitos = await _avisosService.ObtenerCatalogo((int)Filtros.Requisitos);
            List<CatalogoDto> roll = new()
                {
                    new CatalogoDto() { Clave = "1", Descripcion = "Rol 1" },
                    new CatalogoDto() { Clave = "2", Descripcion = "Rol 2" }
                };
            FiltrosDto filtros = new(nivel, campus, sedes, escuelas, programas, requisitos, roll)
            {
                Result = true
            };
            return Ok(filtros);

        }

        /// <summary>
        /// Endpoint que obtiene la información del historial de avisos
        /// </summary>
        /// <returns>Un Objeto que hereda de BaseOutDto</returns>
        // GET: api/filtros/<AvisosController>
        [HttpPost("filtroMatriculas")]
        public async Task<ActionResult<List<CatalogoDto>>> GetFiltroMatriculas(FiltrosMatriculaDto filtros)
        {
            List<CatalogoDto> matriculas = await _avisosService.ObtenerCatalogoMatricula(filtros);
            return Ok(matriculas);
        }
    }
}