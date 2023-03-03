using System.Net;
using System.Security.Claims;
using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;

namespace budget_management_api.Controllers;

[ApiController]
[Authorize]
[Route("api/summary")]
public class SummaryController : ControllerBase
{
   private readonly ISummaryService _summaryService;

   public SummaryController(ISummaryService summaryService)
   {
      _summaryService = summaryService;
   }

   [HttpGet("{month}")]
   public async Task<IActionResult> GetSummary(int month)
   {
      var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      
      var summary = await _summaryService.GetMonthlySummary(Guid.Parse(userId), month);
      var response = new CommonResponse<SummaryResponse>
      {
         StatusCode = (int)HttpStatusCode.OK,
         Message = "Successfully Get Summary",
         Data = summary
      };
      return Ok(response);
   }
   
   [HttpGet("download/{month}")]
   public async Task<IActionResult> ExportToExcel(int month)
   {
      var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      
      var summary = await _summaryService.GetMonthlySummary(Guid.Parse(userId), month);

      var workbook = new HSSFWorkbook();
      var worksheet = workbook.CreateSheet("Summary");
      
      worksheet.CreateRow(1).CreateCell(1).SetCellValue($"Month: {summary.Month}");
      worksheet.CreateRow(2).CreateCell(1).SetCellValue($"Total Income: Rp{summary.TotalIncome}");
      worksheet.CreateRow(3).CreateCell(1).SetCellValue($"Total Expend: Rp{summary.TotalExpend}");
      worksheet.CreateRow(4).CreateCell(1).SetCellValue($"Current Balance: Rp{summary.CurrentBalance}");
      worksheet.CreateRow(5).CreateCell(1).SetCellValue($"Printed At: {DateTime.Now:f} ");
      
      var stream = new MemoryStream();
      workbook.Write(stream);
      stream.Seek(0, SeekOrigin.Begin);

      return File(stream, "application/vnd.ms-excel", "summary-report.xls");
   }
}