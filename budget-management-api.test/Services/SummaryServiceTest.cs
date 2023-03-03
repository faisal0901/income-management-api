using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Services;
using Moq;

namespace budget_management_api.test.Services;

public class SummaryServiceTest
{
    private readonly Mock<ITransactionService> _mockTransaction;
    private readonly ISummaryService _summaryService;

    private readonly Transactional _transactional;
    public SummaryServiceTest()
    {
        _mockTransaction = new Mock<ITransactionService>();
        _summaryService = new SummaryService(_mockTransaction.Object);
    }

    [Fact]
    public async Task Should_ReturnLong_When_CurrentBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var income = _mockTransaction.Setup(income => income.TotalIncome(userId))
            .ReturnsAsync(100);
        var outcome = _mockTransaction.Setup(outcome => outcome.TotalExpend(userId))
            .ReturnsAsync(50);

        // Act
        var result = await _summaryService.CurrentBalance(userId);
        
        // Assert
        Assert.Equal(50, result);
    }

    [Fact]
    public async Task Should_ReturnSummaryResponse_When_GetMonthlySummary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 3;

        var income = _mockTransaction.Setup(i => i.TotalMonthlyIncome(userId, month)).ReturnsAsync(
            new DailyMonthlyTotalResponse
            {
                Month = month.ToString(),
                Balance = 500000
            });
        var outcome = _mockTransaction.Setup(o => o.TotalMonthlyExpend(userId, month)).ReturnsAsync(
            new DailyMonthlyTotalResponse
            {
                Month = month.ToString(),
                Balance = 1000000
            });

        // Act
        var current = await _summaryService.GetMonthlySummary(userId, month);
        
        // Action
        Assert.Equal(1000000, current.TotalExpend);
        Assert.Equal(500000, current.TotalIncome);
    }
}