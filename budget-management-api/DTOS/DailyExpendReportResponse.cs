namespace budget_management_api.DTOS;

public class DailyExpendReportResponse
{
    public string TransDate { get; set; }
    public long Out { get; set; }
    public string SubCategory { get; set; }
    public string Category { get; set; }
}