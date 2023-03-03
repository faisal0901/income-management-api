namespace budget_management_api.DTOS;

public class MonthlyIncomeReportResponse
{
    public string Month { get; set; }
    public long TotalCurrentIncome { get; set; }
    public long DailyAverageIncome { get; set; }
    public long TotalPreviousIncome { get; set; }
    public string PercentageIncrease { get; set; }
}