using budget_management_api.Enum;

namespace budget_management_api.Dtos;

public class GoldResponse
{
    public  string UserId { get; set; }
    public int Gram { get; set; }
    public long price { get; set; }
    public long total { get; set; }
    public string WalletType { get; set; }
    
}