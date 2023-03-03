using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.Entities;
using budget_management_api.Repositories;

namespace budget_management_api.Services.Implementations;

public class SubCategoryService : ISubCategoryService
{
  private readonly IRepository<SubCategory> _subCategoryRepository;
  private readonly IPersistence _persistence;

  public SubCategoryService(IRepository<SubCategory> subCategoryRepository, IPersistence persistence)
  {
    _subCategoryRepository = subCategoryRepository;
    _persistence = persistence;
  }

  public async Task<SubCategoryResponse> Create(SubCategory payload)
  {
    var result = await _persistence.ExecuteTransactionAsync(async () =>
    {
      var save = await _subCategoryRepository.Save(payload);
      await _persistence.SaveChangesAsync();
      return save;
    });

    SubCategoryResponse response = new()
    {
      Id = result.Id,
      CategoryName = result.CategoryName,
     
    };

    return response;
  }

  public async Task<SubCategoryResponse> GetById(Guid id)
  {
    try
    {
      var subCategory = await _subCategoryRepository.Find(subCategory => subCategory.Id.Equals(id));

      if (subCategory is null) throw new Exception("subCategory not found");

      SubCategoryResponse response = new()
      {
        Id = subCategory.Id,
        CategoryName = subCategory.CategoryName,
      };

      return response;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
  public async Task<GetCategoryResponse> GetCategoryId(Guid id)
  {
    try
    {
      var subCategory = await _subCategoryRepository.Find(subCategory => subCategory.Id.Equals(id), new string[] { "Category" });

      if (subCategory == null) throw new Exception("subCategory not found");

      var response = new GetCategoryResponse
      {
        Id = subCategory.Id,
        CategoryName = subCategory.Category.CategoryName,
        SubCategoryName = subCategory.CategoryName,
      };

      return response;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  public async Task<IEnumerable<SubCategory>> GetAll()
  {
    var subCategories = await _subCategoryRepository.FindAll();

    return subCategories;
  }
  

  public async Task<SubCategory> Update(SubCategory payload)
  {
    var subCategory = GetById(payload.Id);
    if (subCategory is null) throw new Exception("Sub Category not found.");

    var attach = _subCategoryRepository.Attach(payload);
    await _persistence.SaveChangesAsync();
    return attach;
  }

  public async Task DeleteById(Guid id)
  {
    var subCategory = await _subCategoryRepository.FindById(id);
    if (subCategory is null) throw new Exception("subCategory not found");
    _subCategoryRepository.Delete(subCategory);
    await _persistence.SaveChangesAsync();
  }
}
