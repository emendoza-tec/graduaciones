using ClosedXML.Excel;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.ProcesaExcel
{
    public static class ProcesaExpediente
    {
        public static async Task<List<ExpedienteEntity>> ObtenExpedientes(IFormFile archivo)
        {
            var listaData = new List<ExpedienteEntity>();
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
                            var cell = new ExpedienteEntity();
                            cell.Matricula = dataRow.Cell(1).GetString();
                            cell.Estatus = dataRow.Cell(2).GetString();
                            cell.Detalle = dataRow.Cell(3).GetString();
                            listaData.Add(cell);
                        }
                    }

                }
            }
            return listaData;
        }
    }
}