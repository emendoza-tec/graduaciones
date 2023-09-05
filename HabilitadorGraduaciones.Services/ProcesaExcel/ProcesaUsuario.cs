using ClosedXML.Excel;
using HabilitadorGraduaciones.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.ProcesaExcel
{
    public class ProcesaUsuario
    {
        public static async Task<List<UsuarioAdministradorDto>> ObtenerUsuariosFromExcel(IFormFile archivo)
        {
            var listaData = new List<UsuarioAdministradorDto>();
            if (archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using (var memoryStream = new MemoryStream())
                {
                    await archivo.CopyToAsync(memoryStream);

                    var workbook = new XLWorkbook(memoryStream);
                    var ws = workbook.Worksheet(1);
                    var nonEmptyDataRows = ws.RowsUsed();
                    foreach (var dataRow in nonEmptyDataRows)
                    {
                        //for row number check
                        if (dataRow.RowNumber() > 1 && dataRow.RowNumber() <= nonEmptyDataRows.Count())
                        {
                            var cell = new UsuarioAdministradorDto();
                            cell.Nomina = dataRow.Cell(1).GetString();
                            cell.Correo = dataRow.Cell(2).GetString();
                            cell.Campus = dataRow.Cell(3).GetString();
                            cell.Sede = dataRow.Cell(4).GetString();
                            cell.Nivel = dataRow.Cell(5).GetString();
                            cell.Rol = dataRow.Cell(6).GetString();
                            listaData.Add(cell);
                        }
                    }

                }
            }
            return listaData;
        }
    }
}
