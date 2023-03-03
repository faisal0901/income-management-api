namespace budget_management_api.DTOS;

public class DailyMonthlyTotalResponse
{
    public string Month { get; set; }
    public long Average { get; set; }
    public long Balance { get; set; }
}