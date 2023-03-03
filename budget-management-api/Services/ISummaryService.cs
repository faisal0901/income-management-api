using budget_management_api.DTOS;

namespace budget_management_api.Services;

public interface ISummaryService
{
    Task<SummaryResponse> GetMonthlySummary(Guid userId, int month);
    Task<long> CurrentBalance(Guid userId);
}