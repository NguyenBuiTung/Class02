using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class ReportController : ControllerBase
    {
        private readonly ISheepRepository _sheepRepository;
        private readonly IExcelExportService _excelExportService;
        private readonly ApplicationDbContext _context;
        public ReportController(ISheepRepository sheepRepository, IExcelExportService excelExportService, ApplicationDbContext context)
        {
            _sheepRepository = sheepRepository;
            _excelExportService = excelExportService;
            _context = context;
        }

        [HttpGet("report-by-time")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ReportByTime(
      [FromQuery] int? startTime = null, // Thời gian là tham số tùy chọn
      [FromQuery] int? endTime = null, // Thời gian là tham số tùy chọn
      [FromQuery] string color = "all")
        {
            var query = _context.Sheeps.AsQueryable();

            // Lọc theo thời gian nếu các tham số được cung cấp
            if (startTime.HasValue && endTime.HasValue)
            {
                query = query.Where(s => s.Time >= startTime.Value && s.Time <= endTime.Value);
            }

            // Nếu màu sắc không phải là "all", lọc theo màu sắc
            if (!string.IsNullOrEmpty(color) && color.ToLower() != "all")
            {
                var colorLower = color.ToLower();
                query = query.Where(s => s.Color.ToLower() == colorLower);
            }

            var sheeps = await query.ToListAsync();

            // Tính tổng số lượng cừu theo màu sắc
            // Tính tổng số lượng cừu, MeatWeight và WoolWeight theo màu sắc
            var sheepCountByColor = sheeps
                .GroupBy(s => s.Color.ToLower())
                .Select(g => new
                {
                    Color = g.Key,
                    Count = g.Count(),
                    TotalMeatWeight = g.Sum(s => s.MeatWeight),
                    TotalWoolWeight = g.Sum(s => s.WoolWeight)
                }).ToList();

            // Tạo đối tượng chứa kết quả trả về
            var result = new
            {
                Sheeps = sheeps.Select(s => new SheepDto
                {
                    Color = s.Color,
                    MeatWeight = s.MeatWeight,
                    WoolWeight = s.WoolWeight,
                    Time = s.Time
                }).ToList(),
                SheepCountByColor = sheepCountByColor
            };

            return Ok(result);
        }



        // [HttpGet("report-by-color")]
        // public async Task<ActionResult<List<SheepDto>>> GetReportByColor([FromQuery] string color)
        // {
        //     var sheeps = await _sheepRepository.GetSheepByColorAsync(color);
        //     var sheepDtos = sheeps.Select(s => new SheepDto
        //     {
        //         Color = s.Color,
        //         MeatWeight = s.MeatWeight,
        //         WoolWeight = s.WoolWeight,
        //         Time = s.Time
        //     }).ToList();

        //     return Ok(sheepDtos);
        // }

        [HttpGet("export-sheep")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ExportSheepToExcel()
        {
            var sheeps = await _sheepRepository.GetAllSheepAsync();
            var fileContent = await _excelExportService.ExportSheepToExcel(sheeps);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sheeps.xlsx");
        }
    }
}