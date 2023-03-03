using System.Net;
using System.Security.Claims;
using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.Entities;
using budget_management_api.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_api.Controllers;
[Route("api/gold")]
[Authorize]
public class GoldController:ControllerBase
{
    private readonly IGoldService _goldService;

    public GoldController(IGoldService goldService)
    {
        _goldService = goldService;
    }

    
    [HttpPost("buy")]
    [Authorize(Roles = "Premium")]
    public async Task<CreatedResult> buyGold([FromBody] GoldRequest payload)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        payload.UserId = Guid.Parse(userId);
        var result = await _goldService.BuyNewGold(payload);
        var response = new CommonResponse<GoldResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully buy gold.",
            Data = result
        };
        return Created("api/gold", response);
    }
    [HttpGet]
    [Authorize(Roles = "Premium")]
    public async Task<IActionResult> GetAllGold()
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _goldService.GetGold(Guid.Parse(userId));
        return Ok(result);
    }

   
}