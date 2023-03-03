using budget_management_api.DTOs;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using budget_management_api.Dtos;
using budget_management_api.DTOS;

namespace budget_management_api.Controllers;

[Route("api/sub-categories")]
public class SubCategoryController : ControllerBase
{
  private readonly ISubCategoryService _subCategoryService;

  public SubCategoryController(ISubCategoryService subCategoryService)
  {
    _subCategoryService = subCategoryService;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] SubCategory subCategory)
  {
    var newSubCategory = await _subCategoryService.Create(subCategory);
    var response = new CommonResponse<SubCategoryResponse>
    {
      StatusCode = (int)HttpStatusCode.Created,
      Message = "Successfully Created Sub Category.",
      Data = newSubCategory
    };
    return Created("/api/sub-categories", response);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var subCategories = await _subCategoryService.GetAll();
    var response = new CommonResponse<IEnumerable<SubCategory>>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get customer",
      Data = subCategories
    };
    return Ok(response);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var subCategory = await _subCategoryService.GetById(id);
    var response = new CommonResponse<SubCategoryResponse>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get customer",
      Data = subCategory
    };
    return Ok(response);
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] SubCategory subCategory)
  {
    var updated = await _subCategoryService.Update(subCategory);
    var response = new CommonResponse<SubCategory>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "Successfully Updated Sub Category.",
      Data = updated
    };
    return Ok(response);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteById(Guid id)
  {
    await _subCategoryService.DeleteById(id);
    var response = new CommonResponse<string?>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "Successfully Deleted Sub Category.",
    };
    return Ok(response);
  }
}
