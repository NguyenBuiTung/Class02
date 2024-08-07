using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace api.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly ISheepRepository _sheepRepository;

        public ExcelExportService(ISheepRepository sheepRepository)
        {
            _sheepRepository = sheepRepository;
        }

        public async Task<byte[]> ExportSheepToExcel(List<Sheep> sheeps)
        {
            using (var package = new ExcelPackage())
            {
                var groupedSheeps = sheeps.GroupBy(s => s.Color).ToList();

                if (groupedSheeps.Count == 0)
                {
                    // Add a default worksheet if there are no sheep
                    package.Workbook.Worksheets.Add("Sheet1");
                }

                foreach (var group in groupedSheeps)
                {
                    var worksheet = package.Workbook.Worksheets.Add(group.Key);
                    worksheet.Cells[1, 1].Value = "Color";
                    worksheet.Cells[1, 2].Value = "Meat Weight";
                    worksheet.Cells[1, 3].Value = "Wool Weight";
                    worksheet.Cells[1, 4].Value = "Time";

                    for (int i = 0; i < group.Count(); i++)
                    {
                        var sheep = group.ElementAt(i);
                        worksheet.Cells[i + 2, 1].Value = sheep.Color;
                        worksheet.Cells[i + 2, 2].Value = sheep.MeatWeight;
                        worksheet.Cells[i + 2, 3].Value = sheep.WoolWeight;
                        worksheet.Cells[i + 2, 4].Value = sheep.Time;
                    }
                }

                return await Task.FromResult(package.GetAsByteArray());
            }
        }

        public async Task ExportSheepDataAsync()
        {
            var sheeps = await _sheepRepository.GetAllSheepAsync();
            var fileContent = await ExportSheepToExcel(sheeps);

            var fileName = $"SheepData_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var filePath = Path.Combine("D:\\SheepData\\", fileName);

            if (!Directory.Exists("D:\\SheepData\\"))
            {
                Directory.CreateDirectory("D:\\SheepData\\");
            }

            await File.WriteAllBytesAsync(filePath, fileContent);
        }
    }

}