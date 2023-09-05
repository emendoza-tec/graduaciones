using DocumentFormat.OpenXml.Drawing;
using HabilitadorGraduaciones.Core.Entities;
using OfficeOpenXml;
using System.Globalization;

namespace HabilitadorGraduaciones.Web.Common
{
    public class GenerarExcelHelper
    {
        public ExcelPackage GenerarExcelEstimadoGraduacion(List<ReporteEstimadoDeGraduacionEntity> registros)
        {
            int totalRegistros = registros.Count + 1;
            string rango = "I2:I" + totalRegistros.ToString().Trim();//Para definir el campo donde irá la fecha
            ExcelPackage libroExcel = new ExcelPackage();

            var worksheetExcel = libroExcel.Workbook.Worksheets.Add("EstimadoDeGraduacion");
            if (registros.Count > 0)
            {
                worksheetExcel.Cells["A1"].LoadFromCollection(registros, PrintHeaders: true);
                // Formato para la fecha
                worksheetExcel.Cells[rango].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                for (var col = 1; col < 14; col++)
                {
                    worksheetExcel.Column(col).AutoFit();
                }

                // Formato de tabla
                var tablaExcel = worksheetExcel.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: totalRegistros, toColumn: 13), "EstimadoDeGraduacion");
                tablaExcel.ShowHeader = true;
                tablaExcel.ShowTotal = true;

            }
            return libroExcel;
        }

        public ExcelPackage GenerarExcelSabana(List<SabanaEntity> registros)
        {
            int totalReg = registros.Count + 1;
            string rango = "AI2:AI" + totalReg.ToString().Trim();
            ExcelPackage libroExcel = new ExcelPackage();

            var worksheet = libroExcel.Workbook.Worksheets.Add("Sabana");
            if (registros.Count > 0)
            {
                worksheet.Cells["A1"].LoadFromCollection(registros, PrintHeaders: true);
                worksheet.Cells[rango].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                for (var col = 1; col < 37; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                // Agregar formato de tabla
                var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: totalReg, toColumn: 37), "Sabana");
                tabla.ShowHeader = true;
                tabla.ShowTotal = true;
            }
            return libroExcel;
        }
    }
}
