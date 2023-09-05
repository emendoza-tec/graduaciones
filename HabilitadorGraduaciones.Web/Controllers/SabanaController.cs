using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Common;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SabanaController : Controller
    {
        private readonly IReporteSabanaService _reporteSabanaService;

        public SabanaController(IReporteSabanaService reporteSabanaService)
        {
            _reporteSabanaService = reporteSabanaService;
        }

        [HttpPost("DescargarReporteSabana")]
        public async Task<IActionResult> DescargarExcelReporteSabana(UsuarioAdministradorDto data)
        {
            string excelRSabanaContentType = "aplication/vdn.openxmlformats-officedocument.spreadsheetml.sheet";
            var reg = await _reporteSabanaService.GetReporteSabana(data);
            GenerarExcelHelper generarExcelH = new GenerarExcelHelper();
            ExcelPackage libroExcel = generarExcelH.GenerarExcelSabana(reg);

            return File(libroExcel.GetAsByteArray(), excelRSabanaContentType, "Sabana.xlsx");
        }
    }
}
