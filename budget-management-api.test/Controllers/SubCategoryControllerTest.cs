using System.Net;
using budget_management_api.Controllers;
using budget_management_api.Dtos;
using budget_management_api.DTOs;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace budget_management_api.test.Controllers;

public class SubCategoryControllerTest
{
  private readonly Mock<ISubCategoryService> _mockService;
  private readonly SubCategoryController _controller;

  public SubCategoryControllerTest()
  {
    _mockService = new Mock<ISubCategoryService>();
    _controller = new SubCategoryController(_mockService.Object);
  }

  [Fact]
  public async Task Should_ReturnCreated_When_Create()
  {
    var subCat = new SubCategory
    {
      Id = Guid.NewGuid(),
      CategoryName = "kebutuhan",
      CategoryId = Guid.NewGuid(),
    };

    var subResponse = new SubCategoryResponse
    {
      Id = Guid.NewGuid(),
      CategoryName = "kebutuhan"
    };

    _mockService.Setup(x => x.Create(subCat))
        .ReturnsAsync(subResponse);

    CommonResponse<SubCategoryResponse> response = new()
    {
      StatusCode = (int)HttpStatusCode.Created,
      Message = "Successfully Created Sub Category.",
      Data = subResponse
    };

    // act
    var result = await _controller.Create(subCat) as CreatedResult;

    _mockService.Verify(service => service.Create(subCat), Times.Once);

    var resultResponse = result?.Value as CommonResponse<SubCategoryResponse>;
    Assert.Equal(response.StatusCode, result?.StatusCode);
    Assert.Equal(response.Data, resultResponse?.Data);

  }

  [Fact]
  public async Task Should_ReturnOk_When_GetAll()
  {
    var subs = new List<SubCategory>()
        {
            new SubCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "listrik",
                CategoryId = Guid.NewGuid(),
            }
        };

    _mockService.Setup(x => x.GetAll())
        .ReturnsAsync(subs);

    var response = new CommonResponse<IEnumerable<SubCategory>>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get customer",
      Data = subs
    };

    // act
    var result = await _controller.GetAll() as OkObjectResult;

    _mockService.Verify(service => service.GetAll(), Times.Once);

    var resultResponse = result?.Value as CommonResponse<IEnumerable<SubCategory>>;
    Assert.Equal(response.StatusCode, result?.StatusCode);
    Assert.Equal(response.Data, resultResponse?.Data);
  }

  [Fact]
  public async Task Should_ReturnOk_When_GetById()
  {
    var subResponse = new SubCategoryResponse
    {
      Id = Guid.NewGuid(),
      CategoryName = "kebutuhan"
    };

    _mockService.Setup(x => x.GetById(Guid.NewGuid()))
        .ReturnsAsync(subResponse);

    var response = new CommonResponse<SubCategoryResponse>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get customer",
      Data = subResponse
    };

    // act
    var result = await _controller.GetById(subResponse.Id) as OkObjectResult;

    _mockService.Verify(s => s.GetById(subResponse.Id), Times.Once);
    Assert.Equal(response.StatusCode, result?.StatusCode);
  }

  [Fact]
  public async Task Should_ReturnOk_When_Update()
  {
    // Arrange
    var subCategoryServiceMock = new Mock<ISubCategoryService>();
    subCategoryServiceMock.Setup(x => x.Update(It.IsAny<SubCategory>()))
        .ReturnsAsync(new SubCategory());

    var controller = new SubCategoryController(subCategoryServiceMock.Object);

    // Act
    var result = await controller.Update(new SubCategory());

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<CommonResponse<SubCategory>>(okResult.Value);
    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    Assert.Equal("Successfully Updated Sub Category.", response.Message);
    Assert.NotNull(response.Data);
  }

  [Fact]
  public async Task Should_ReturnOk_When_DeleteById()
  {
    // Arrange
    //var subCategoryServiceMock = new Mock<ISubCategoryService>();
    _mockService.Setup(x => x.DeleteById(It.IsAny<Guid>()))
        .Returns(Task.CompletedTask);

    //var controller = new SubCategoryController(subCategoryServiceMock.Object);
    var id = Guid.NewGuid();

    // Act
    var result = await _controller.DeleteById(id);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<CommonResponse<string?>>(okResult.Value);
    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    Assert.Equal("Successfully Deleted Sub Category.", response.Message);
  }
}