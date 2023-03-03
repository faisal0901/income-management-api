using System.Net;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.Enum;
using budget_management_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _mockService = new Mock<IAuthService>();
        _authController = new AuthController(_mockService.Object);
    }
    
    [Fact]
    public async Task Should_ReturnCreated_When_RegisterFree()
    {
        var request = new RegisterRequest
        {
            Email = "unit@testing.com",
            Password = "password",
            PhoneNumber = "0812345678"
        };

        var registerResponse = new RegisterResponse
        {
            Email = request.Email,
            Role = ERole.Free.ToString()
        };

        _mockService.Setup(service =>
                service.RegisterFree(request))
            .ReturnsAsync(registerResponse);

        CommonResponse<RegisterResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Create New User",
            Data = registerResponse
        };

        var result = await _authController.RegisterFree(request) as CreatedResult;
        
        _mockService.Verify(service => service.RegisterFree(request), Times.Once);

        var resultResponse = result?.Value as CommonResponse<RegisterResponse>;
        Assert.Equal(response.StatusCode, result?.StatusCode);
        Assert.Equal(response.Data, resultResponse?.Data);
    }
    
    [Fact]
    public async Task Should_ReturnCreated_When_RegisterPremium()
    {
        var request = new RegisterRequest
        {
            Email = "unit@testing.com",
            Password = "password",
            PhoneNumber = "0812345678"
        };

        var registerResponse = new RegisterResponse
        {
            Email = request.Email,
            Role = ERole.Premium.ToString()
        };

        _mockService.Setup(service =>
                service.RegisterPremium(request))
            .ReturnsAsync(registerResponse);

        CommonResponse<RegisterResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Create New User",
            Data = registerResponse
        };

        var result = await _authController.RegisterPremium(request) as CreatedResult;
        
        _mockService.Verify(service => service.RegisterPremium(request), Times.Once);

        var resultResponse = result?.Value as CommonResponse<RegisterResponse>;
        Assert.Equal(response.StatusCode, result?.StatusCode);
        Assert.Equal(response.Data, resultResponse?.Data);
    }

    [Fact]
    public async Task Should_ReturnOk_When_Login()
    {
        var request = new LoginRequest
        {
            Email = "unit@testing.com",
            Password = "password123"
        };

        var loginResponse = new LoginResponse
        {
            Email = request.Email,
            Role = ERole.Free.ToString(),
            Token = "token"
        };

        _mockService.Setup(service =>
                service.Login(request))
            .ReturnsAsync(loginResponse);

        CommonResponse<LoginResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Sucessfully Login",
            Data = loginResponse
        };

        var result = await _authController.Login(request) as OkObjectResult;
        
        _mockService.Verify(service => service.Login(request), Times.Once);

        var resultResponse = result?.Value as CommonResponse<LoginResponse>;
        Assert.Equal(response.StatusCode, result?.StatusCode);
        Assert.Equal(response.Data, resultResponse?.Data);
    }
}