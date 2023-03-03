using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Entities;

namespace budget_management_api.Services;

public interface ITransactionService
{
    Task<IncomeResponse> CreateNewIncome(Transactional payload);
    Task<ExpendResponse> CreateNewExpand(Transactional payload);
    Task<Transactional> CreateNewTransactional(Transactional payload);
    Task<IEnumerable<DailyIncomeReportResponse>> DailyReportIncome(Guid userId, int date);
    Task<MonthlyIncomeReportResponse> MonthlyReportIncome(Guid userId, int date);
    Task<IEnumerable<DailyExpendReportResponse>> DailyReportExpend(Guid userId, int date);
    Task<MonthlyExpendReportResponse> MonthlyReportExpend(Guid userId, int date);
    Task<DailyMonthlyTotalResponse> TotalMonthlyIncome(Guid userId, int date);
    Task<DailyMonthlyTotalResponse> TotalMonthlyExpend(Guid userId, int date);
    Task<long> TotalIncome(Guid userId);
    Task<long> TotalExpend(Guid userId);
}