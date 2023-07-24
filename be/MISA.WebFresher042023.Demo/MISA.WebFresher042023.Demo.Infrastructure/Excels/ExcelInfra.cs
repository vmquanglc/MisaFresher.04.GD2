using MISA.WebFresher042023.Demo.Core.Interface.Excels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Infrastructure.Excels
{
    public class ExcelInfra : IExcelInfra
    {
        public byte[] ExportToExcel(DataTable data, string title)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Hoặc LicenseContext.Commercial

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(title);

                // Lấy số lượng cột hiện tại trong DataTable
                int columnCount = data.Columns.Count;

                // Xác định ô cuối cùng trong title header
                string lastCell = GetExcelColumnName(columnCount) + "1";

                // Gộp các ô từ A1 đến ô cuối cùng của title header
                worksheet.Cells["A1:" + lastCell].Merge = true;

                // Thiết lập các thuộc tính cho title header
                var titleRange = worksheet.Cells["A1:" + lastCell];
                titleRange.Value = title;
                titleRange.Style.Font.Size = 16;
                titleRange.Style.Font.Name = "Arial";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Ghi dữ liệu vào worksheet từ DataTable
                worksheet.Cells["A3"].LoadFromDataTable(data, true);

                // Tạo style cho header (dòng đầu tiên)
                using (var headerRange = worksheet.Cells[3, 1, 3, data.Columns.Count])
                {
                    headerRange.Style.Font.Size = 10;
                    headerRange.Style.Font.Name = "Arial";
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    headerRange.Style.Border.Top.Color.SetColor(Color.Black);
                    headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    headerRange.Style.Border.Bottom.Color.SetColor(Color.Black);
                    headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    headerRange.Style.Border.Left.Color.SetColor(Color.Black);
                    headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    headerRange.Style.Border.Right.Color.SetColor(Color.Black);
                }
                if (worksheet.Dimension.End.Row >= 4)
                {
                    // Tạo style cho các hàng cột
                    using (var bodyRange = worksheet.Cells[4, 1, worksheet.Dimension.End.Row, data.Columns.Count])
                    {
                        bodyRange.Style.Font.Size = 10;
                        bodyRange.Style.Font.Name = "Time New Roman";
                        bodyRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        bodyRange.Style.Border.Bottom.Color.SetColor(Color.Black);
                        bodyRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        bodyRange.Style.Border.Left.Color.SetColor(Color.Black);
                        bodyRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        bodyRange.Style.Border.Right.Color.SetColor(Color.Black);
                        bodyRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        bodyRange.Style.Border.Top.Color.SetColor(Color.Black);
                        bodyRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        bodyRange.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                    }

                    // AutoFit các cột
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Căn giữa các cột DateTime
                    List<string> dateTimeColumns = new List<string>
                        {
                            "STT",
                            "Ngày sinh",
                            "IdentityDateRelease",
                            "CreatedDate",
                            "ModifiedDate"
                        };

                    foreach (string columnName in dateTimeColumns)
                    {
                        if (data.Columns.Contains(columnName))
                        {
                            int columnIndex = data.Columns.IndexOf(columnName) + 1;
                            worksheet.Column(columnIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                    }


                }

                // Tạo một MemoryStream để lưu trữ file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
