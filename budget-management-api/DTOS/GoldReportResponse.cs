namespace budget_management_api.Dtos;

public class GoldReportResponse
{
     public long   currentTotalPrice { get; set; }  
     public double percentage { get; set; } 
     public int GoldTotalGram { get; set; } 
     public long TotalPriceWhenBuying { get; set; }
}