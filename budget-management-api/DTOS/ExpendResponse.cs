namespace budget_management_api.Dtos;

public class ExpendResponse
{
    public string Id { get; set; }
    public long Balance { get; set; }
    public DateTime TransDate { get; set; }
    public string UserId { get; set; }
   
    public string walletType { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
}