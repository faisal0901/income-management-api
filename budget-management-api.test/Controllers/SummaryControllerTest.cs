using System.Net;
using System.Security.Claims;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class SummaryControllerTest
{
    private readonly Mock<ISummaryService> _mockSummary;
    private readonly SummaryController _summaryController;

    public SummaryControllerTest()
    {
        _mockSummary = new Mock<ISummaryService>();
        _summaryController = new SummaryController(_mockSummary.Object);
    }
    
    [Fact]
    public async Task Should_ReturnOk_When_GetSummary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;

        var summaryResponse = new SummaryResponse
        {
            Month = month.ToString(),
            TotalIncome = 100,
            TotalExpend = 50,
            CurrentBalance = 50
        };
    
        _mockSummary.Setup(s => s.GetMonthlySummary(userId, month))
            .ReturnsAsync(summaryResponse);

        var expectedResponse = new CommonResponse<SummaryResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Summary",
            Data = summaryResponse
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _summaryController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            }
        };

        // Act
        var result = await _summaryController.GetSummary(month) as OkObjectResult;

        var resultResponse = result?.Value as CommonResponse<SummaryResponse>;
        
        _mockSummary.Verify(service => service.GetMonthlySummary(userId, month), Times.Once);
        // Assert
        Assert.Equal(expectedResponse.StatusCode, result.StatusCode);
        Assert.Equal(expectedResponse.Message, resultResponse.Message);
    }

    [Fact]
    public async Task Should_ExportToExcel_When_GetSummary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;

        var summaryResponse = new SummaryResponse
        {
            Month = month.ToString(),
            TotalIncome = 100,
            TotalExpend = 50,
            CurrentBalance = 50
        };
    
        _mockSummary.Setup(s => s.GetMonthlySummary(userId, month))
            .ReturnsAsync(summaryResponse);

        var context = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }, "mock"))
            }
        };
        _summaryController.ControllerContext = context;

        // Act
        var result = await _summaryController.ExportToExcel(month) as FileStreamResult;

        // Assert
        Assert.Equal("application/vnd.ms-excel", result.ContentType);
        Assert.Equal("summary-report.xls", result.FileDownloadName);
        Assert.NotNull(result.FileStream);
    }

}