using System.Net;
using System.Security.Claims;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class GoldControllerTest
{
    private readonly Mock<IGoldService> _mockGoldService;

    private readonly GoldController _controller;

    public GoldControllerTest()
    {
        _mockGoldService = new Mock<IGoldService>();
        _controller = new GoldController(_mockGoldService.Object);
    }

    [Fact]
    public async Task ShouldReturnGoldResponse_WhenBuyGold()
    {
        var userId = Guid.NewGuid();
        var goldRequest = new GoldRequest
        {
            gram = 10,
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }))
            }
        };
       
        var result = await _controller.buyGold(goldRequest);
        var createdResult = result as CreatedResult;
        Assert.NotNull(createdResult);
        Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        var response = createdResult.Value as CommonResponse<GoldResponse>;
        Assert.NotNull(response);
    }
    [Fact]
    public async Task ShouldReturnGoldResponse_WhenGetGold()
    {
        var userId = Guid.NewGuid();
      
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }))
            }
        };
       
        var result = await _controller.GetAllGold();
        Assert.NotNull(result);
        // var createdResult = result as CreatedResult;
        // Assert.NotNull(createdResult);
        // Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        // var response = createdResult.Value as CommonResponse<GoldResponse>;
        // Assert.NotNull(response);
    }
}