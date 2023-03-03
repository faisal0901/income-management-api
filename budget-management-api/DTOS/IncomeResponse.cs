namespace budget_management_api.DTOS;

public class IncomeResponse
{
    public string Id { get; set; }
    public long Balance { get; set; }
    public DateTime TransDate { get; set; }
    public string UserId { get; set; }
   
    public long emergencyBalance { get; set; }
    public long needsBalance { get; set; }
    public long wantsBalance { get; set; }
    public long savingBalance { get; set; }
}