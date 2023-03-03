using System.Net;
using System.Security.Claims;
using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_api.Controllers;
[ApiController]
[Authorize]
[Route("api/transactions")]
public class TransactionController:ControllerBase
{
    private readonly ITransactionService _transactionService;
   

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
 
    }
    [HttpPost("income")]
    public async Task<IActionResult> CreateIncome([FromBody] Transactional request)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = Guid.Parse(userId);
        var result = await _transactionService.CreateNewIncome(request);
        var response = new CommonResponse<IncomeResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Create new Income",
            Data = result
        };
        return Created("api/transactions", response);
    }
    [HttpPost("expend")]
    public async Task<IActionResult> CreateExpend([FromBody] Transactional request)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not null)
        {
         request.UserId = Guid.Parse(userId);
        }
        var result = await _transactionService.CreateNewExpand(request);
        var response = new CommonResponse<ExpendResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Create new Expend",
            Data = result
        };
        return Created("api/transactions", response);
    }

    // Monthly Income Report
    [HttpGet("income/monthly/{month}")]
    public async Task<IActionResult> GetMonthlyReportIncome(int month)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _transactionService.MonthlyReportIncome(Guid.Parse(userId),month);
        var response = new CommonResponse<MonthlyIncomeReportResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Monthly Income Report",
            Data = result
        };
        return Ok(response);
    }

    // Daily Income Report
    [HttpGet("income/daily/{month}")]
    public async Task<IActionResult> GetDailyReportIncome(int month)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var results = await _transactionService.DailyReportIncome(Guid.Parse(userId), month);
        var response = new CommonResponse<IEnumerable<DailyIncomeReportResponse>>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Daily Income Report",
            Data = results
        };
        return Ok(response);
    }
    
    // Monthly Expend Report
    [HttpGet("expend/monthly/{month}")]
    public async Task<IActionResult> GetMonthlyReportExpend(int month)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var result = await _transactionService.MonthlyReportExpend(Guid.Parse(userId), month);
        var response = new CommonResponse<MonthlyExpendReportResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Monthly Expend Report",
            Data = result
        };
        return Ok(response);
    }

    // Daily Expend Report
    [HttpGet("expend/daily/{month}")]
    public async Task<IActionResult> GetDailyReportExpend(int month)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var results = await _transactionService.DailyReportExpend(Guid.Parse(userId), month);
        var response = new CommonResponse<IEnumerable<DailyExpendReportResponse>>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Daily Expend Report",
            Data = results
        };
        return Ok(response);
    }
}