using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities;
[Table(name:"sub_category")]
public class SubCategory
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name: "category_name"), Required]
    public string CategoryName { get; set; }
    [Column(name: "category_id")] public Guid? CategoryId { get; set; }
    public virtual Category Category { get; set; }
}