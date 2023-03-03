using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities;
[Table(name:"category")]
public class Category
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name: "category_name"), Required]
    public string CategoryName { get; set; } = null;
    public ICollection<SubCategory> SubCategories { get; set; }
}