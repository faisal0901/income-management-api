using System.Net;
using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register-free")]
    public async Task<IActionResult> RegisterFree([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterFree(request);
        var response = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Create New User",
            Data = user
        };
        return Created("api/auth/register", response);
    }

    [HttpPost("register-premium")]
    public async Task<IActionResult> RegisterPremium([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterPremium(request);
        var response = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Succesfully Create New User",
            Data = user
        };
        return Created("api/auth/register", response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var login = await _authService.Login(request);
        var response = new CommonResponse<LoginResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Sucessfully Login",
            Data = login
        };
        return Ok(response);
    }
}