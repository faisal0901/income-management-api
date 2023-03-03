namespace budget_management_api.DTOS;

public class MonthlyExpendReportResponse
{
    public string Month { get; set; }
    public long TotalCurrentExpend { get; set; }
    public long DailyAverageExpend { get; set; }
    public long TotalPreviousExpend { get; set; }
    public string PercentageIncrease { get; set; }
}