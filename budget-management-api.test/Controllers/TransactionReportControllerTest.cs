using System.Security.Claims;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class TransactionReportControllerTest
{
    private readonly Mock<ITransactionService> _mockTransactionService;

    private readonly TransactionController _controller;

    public TransactionReportControllerTest()
    {
        _mockTransactionService = new Mock<ITransactionService>();
        _controller = new TransactionController(_mockTransactionService.Object);
    }

    // Get Monthly Income Report
    [Fact]
    public async Task Should_ReturnOk_When_GetMonthlyReportIncome()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;
        var expectedResult = new MonthlyIncomeReportResponse();
        
        _mockTransactionService
            .Setup(x => x.MonthlyReportIncome(userId, month))
            .ReturnsAsync(expectedResult);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        // Act
        var result = await _controller.GetMonthlyReportIncome(month);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var response = okResult.Value as CommonResponse<MonthlyIncomeReportResponse>;
        Assert.NotNull(response);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Successfully Get Monthly Income Report", response.Message);
        Assert.Equal(expectedResult, response.Data);
    }

    // Get Daily Income Report
    [Fact]
    public async Task Should_ReturnOk_When_GetDailyReportIncome()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;
        var expectedResult = new List<DailyIncomeReportResponse>();
        
        _mockTransactionService
            .Setup(x => x.DailyReportIncome(userId, month))
            .ReturnsAsync(expectedResult);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        // Act
        var result = await _controller.GetDailyReportIncome(month);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var response = okResult.Value as CommonResponse<IEnumerable<DailyIncomeReportResponse>>;
        Assert.NotNull(response);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Successfully Get Daily Income Report", response.Message);
        Assert.Equal(expectedResult, response.Data);
    }
    
    // Get Monthly Expend Report
    [Fact]
    public async Task Should_ReturnOk_When_GetMonthlyReportExpend()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;
        var expectedResult = new MonthlyExpendReportResponse();
        
        _mockTransactionService
            .Setup(x => x.MonthlyReportExpend(userId, month))
            .ReturnsAsync(expectedResult);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        // Act
        var result = await _controller.GetMonthlyReportExpend(month);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var response = okResult.Value as CommonResponse<MonthlyExpendReportResponse>;
        Assert.NotNull(response);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Successfully Get Monthly Expend Report", response.Message);
        Assert.Equal(expectedResult, response.Data);
    }
    
    // Get Daily Expend Report
    [Fact]
    public async Task Should_ReturnOk_When_GetDailyReportExpend()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;
        var expectedResult = new List<DailyExpendReportResponse>();
        
        _mockTransactionService
            .Setup(x => x.DailyReportExpend(userId, month))
            .ReturnsAsync(expectedResult);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        // Act
        var result = await _controller.GetDailyReportExpend(month);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var response = okResult.Value as CommonResponse<IEnumerable<DailyExpendReportResponse>>;
        Assert.NotNull(response);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Successfully Get Daily Expend Report", response.Message);
        Assert.Equal(expectedResult, response.Data);
    }
}