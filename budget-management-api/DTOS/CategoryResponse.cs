namespace budget_management_api.Dtos;

public class GetCategoryResponse
{
    public Guid Id { get; set; }
    public string SubCategoryName { get; set; }
    public string CategoryName { get; set; }
}