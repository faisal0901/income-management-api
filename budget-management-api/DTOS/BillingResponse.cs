using budget_management_api.Entities;

namespace budget_management_api.DTOS;

public class BillingResponse
{
  public Guid Id { get; set; }
  public string BillName { get; set; }
  public long BillAmount { get; set; }
  public DateTime EachBilledDate { get; set; }

  public Guid UserId { get; set; }
}
