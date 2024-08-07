using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class SheepController : ControllerBase

    {
        private readonly ISheepRepository _sheepRepository;
        private readonly IMapper _mapper;
        private readonly IExcelExportService _excelExportService;
        public SheepController(ISheepRepository sheepRepository, IMapper mapper, IExcelExportService excelExportService)
        {
            _sheepRepository = sheepRepository;
            _mapper = mapper;
            _excelExportService = excelExportService;
        }


        // [HttpGet("report/color")]
        // public async Task<ActionResult<IEnumerable<SheepDto>>> GetSheepsByColor(string color)
        // {
        //     var sheeps = await _sheepRepository.GetSheepsByColorAsync(color);
        //     return Ok(_mapper.Map<IEnumerable<SheepDto>>(sheeps));
        // }
        // [HttpGet("export-sheep")]
        // public async Task<IActionResult> ExportSheepToExcel()
        // {
        //     var sheeps = await _sheepRepository.GetAllSheepsAsync();
        //     var fileContent = await _excelExportService.ExportSheepToExcel(sheeps);
        //     return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sheeps.xlsx");
        // }

    }
}