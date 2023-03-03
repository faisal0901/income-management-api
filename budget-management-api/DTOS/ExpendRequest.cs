namespace budget_management_api.DTOS;

public class ExpendRequest
{
  public Guid  UserId { get; set; }
    public long Balance { get; set; }
    public Guid SubCategoryId { get; set; }
}