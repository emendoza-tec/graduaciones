using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Common;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteEstimadoDeGraduacionController : Controller
    {
        private readonly IReporteEstimadoService _reporteEstimadoService;

        public ReporteEstimadoDeGraduacionController(IReporteEstimadoService reporteEstimadoService)
        {
            _reporteEstimadoService = reporteEstimadoService;
        }

        [HttpPost("Descargar")]
        public async Task<IActionResult>DescargarExcelReporteEG(UsuarioAdministradorDto data)
        {
            string excelREGContentType = "aplication/vdn.openxmlformats-officedocument.spreadsheetml.sheet";
            var registros = await _reporteEstimadoService.GetReporteEstimadoDeGraduacion(data);
            GenerarExcelHelper generarExcel = new GenerarExcelHelper();
            ExcelPackage libroExcel = generarExcel.GenerarExcelEstimadoGraduacion(registros);
            
            return File(libroExcel.GetAsByteArray(), excelREGContentType, "Reporte de Estimado de Graduacion.xlsx");
        }
    }
}
