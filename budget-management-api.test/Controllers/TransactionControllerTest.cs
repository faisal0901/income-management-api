using System.Security.Claims;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class TransactionControllerTest
{  
    
    private readonly Mock<ITransactionService> _mockTransactionService;

    private readonly TransactionController _controller;

    public TransactionControllerTest()
    {
        _mockTransactionService = new Mock<ITransactionService>();
        _controller = new TransactionController(_mockTransactionService.Object);
    }

    [Fact]
    public async Task shouldReturnCreateNewIncomeResponse_whenCreateNewIncomeResponse()
    {
        var userId = Guid.NewGuid();
        var request = new Transactional()
        {
            UserId = userId,
            Balance = 10000,
        };
        _mockTransactionService.Setup(x => x.CreateNewIncome(It.IsAny<Transactional>()))
            .ReturnsAsync(new IncomeResponse()
            {
                Balance = request.Balance,
                UserId = request.UserId.ToString(),
                TransDate = request.TransDate
            });
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
        var result = await _controller.CreateIncome(request);
        Assert.NotNull(result);
    }
    [Fact]
    public async Task shouldReturnCreateNewExpendResponse_whenCreateNewExpendResponse()
    {
        var userId = Guid.NewGuid();
        var request = new Transactional()
        {
            UserId = userId,
            Balance = 10000,
            SubCategoryId = new Guid()
        };
        _mockTransactionService.Setup(x => x.CreateNewExpand(It.IsAny<Transactional>()))
            .ReturnsAsync(new ExpendResponse()
            {
                Balance = request.Balance,
                UserId = request.UserId.ToString(),
                TransDate = request.TransDate
            });
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
        var result = await _controller.CreateExpend(request);
        Assert.NotNull(result);
    }
}