using ClosedXML.Excel;
using HabilitadorGraduaciones.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Services.ProcesaExcel
{
    public static class ProcesaExamenIntegrador
    {
        public static async Task<List<ExamenIntegradorEntity>> ObtenExamenesIntegrador(IFormFile archivo)
        {
            var listaData = new List<ExamenIntegradorEntity>();
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
                            var cell = new ExamenIntegradorEntity();
                            cell.Matricula = dataRow.Cell(1).GetString();
                            cell.PeriodoGraduacion = dataRow.Cell(2).GetString();
                            cell.Nivel = dataRow.Cell(3).GetString();
                            cell.NombreRequisito = dataRow.Cell(4).GetString();
                            cell.Estatus = dataRow.Cell(5).GetString();
                            cell.FechaExamen = dataRow.Cell(6).GetString();

                            listaData.Add(cell);
                        }
                    }

                }
            }
            return listaData;
        }
    }
}