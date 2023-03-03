using budget_management_api.DTOS;

namespace budget_management_api.Services;

public class SummaryService : ISummaryService
{
    private readonly ITransactionService _transactionService;

    public SummaryService(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<long> CurrentBalance(Guid userId)
    {
        var incomes = await _transactionService.TotalIncome(userId);
        var expends = await _transactionService.TotalExpend(userId);
        return incomes - expends;
    }

    public async Task<SummaryResponse> GetMonthlySummary(Guid userId, int month)
    {
        var income = await _transactionService.TotalMonthlyIncome(userId, month);
        var expend = await _transactionService.TotalMonthlyExpend(userId, month);
        var currentBalance = await CurrentBalance(userId);
        return new SummaryResponse
        {
            Month = income.Month,
            TotalIncome = income.Balance,
            TotalExpend = expend.Balance,
            CurrentBalance = currentBalance
        };
    }
}