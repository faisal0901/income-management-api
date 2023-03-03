namespace budget_management_api.DTOS;

public class SummaryResponse
{
    public string Month { get; set; }
    public long TotalIncome { get; set; }
    public long TotalExpend { get; set; }
    public long CurrentBalance { get; set; }
}