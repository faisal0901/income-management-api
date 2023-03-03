using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.Entities;

namespace budget_management_api.Services;

public interface ISubCategoryService
{
  Task<SubCategoryResponse> Create(SubCategory payload);
  Task<SubCategoryResponse> GetById(Guid id);
  Task<IEnumerable<SubCategory>> GetAll();
  Task<SubCategory> Update(SubCategory payload);
  Task DeleteById(Guid id);
  Task<GetCategoryResponse> GetCategoryId(Guid id);
}
