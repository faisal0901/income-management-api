using System.Linq.Expressions;
using Moq;
using budget_management_api.Dtos;
using budget_management_api.Enum;
using budget_management_api.Services;
using budget_management_api.Entities;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;
using budget_management_api.Services.Implementations;
using budget_management_api.DTOs;

namespace budget_management_api.test.Services;

public class SubCategoryServiceTest
{
  private readonly Mock<IRepository<SubCategory>> _mockRepository;
  private readonly Mock<IPersistence> _mockPersistence;
  private readonly Mock<ISubCategoryService> _subCategoryServiceMock;
  private SubCategory _subCategory;

  private ISubCategoryService _subCategoryService;

  public SubCategoryServiceTest()
  {
    _mockRepository = new Mock<IRepository<SubCategory>>();
    _mockPersistence = new Mock<IPersistence>();
    _subCategoryService = new SubCategoryService(_mockRepository.Object, _mockPersistence.Object);
    _subCategory = new SubCategory() 
    { 
      Id = Guid.NewGuid(),
      CategoryName = "Keinginan",
    };
  }

  [Fact]
  public async Task Should_ReturnSubCategory_When_Create()
  {
    var sc = new SubCategory()
    {
      Id = Guid.NewGuid(),
      CategoryName = "Keinginan"
    };

    _mockRepository.Setup(repo => repo.Save(It.IsAny<SubCategory>()))
            .ReturnsAsync(sc);

    _mockPersistence.Setup(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<SubCategory>>>()))
        .Callback<Func<Task<SubCategory>>>(func => func())
        .ReturnsAsync(sc);

    var expected = sc.Id;

    // Act
    var result = await _subCategoryService.Create(sc);

    // Assert
    Assert.Equal(expected, result.Id);
  }

  [Fact]
  public async Task Should_ReturnSubCategory_When_Updated()
  {
    // Arrange
    var sc = new SubCategory()
    {
      Id = Guid.NewGuid(),
      CategoryName = "Keinginan"
    };

    _mockRepository.Setup(repo => repo.Save(It.IsAny<SubCategory>()))
            .ReturnsAsync(sc);

    _mockPersistence.Setup(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<SubCategory>>>()))
        .Callback<Func<Task<SubCategory>>>(func => func())
        .ReturnsAsync(sc);

    var expected = sc.CategoryName;

    // Act

    var result = new SubCategory()
    {
      Id = Guid.NewGuid(),
      CategoryName = "Keinginan"
    };

    // Assert
    Assert.Equal(expected, result.CategoryName);
  }
  
    [Fact]
    public async Task ShouldCreateSubCategoryResponse_WhenCreate()
    {
        _mockPersistence
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task<SubCategory>>>()))
            .ReturnsAsync(new SubCategory()
            {
                Id = Guid.NewGuid(),
                CategoryName = "testCategory",
                CategoryId = Guid.NewGuid(),
            });

        _mockRepository
            .Setup(x => x.Save(It.IsAny<SubCategory>()))
            .ReturnsAsync(new SubCategory()
            {
                Id = Guid.NewGuid(),
                CategoryName = "testCategory"
            });
        var result = await _subCategoryService.Create(new SubCategory() { CategoryName = "testCategory",CategoryId = Guid.NewGuid()});
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ShouldReturnSubCategoryByIdWhenGetById()
    {
       
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<SubCategory, bool>>>()))
            .ReturnsAsync(new SubCategory { Id = Guid.NewGuid(), CategoryName = "Kebutuhan",CategoryId = Guid.NewGuid()});
       

        // Act
        var result = await _subCategoryService.GetById(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ShouldIncluceCategoryWhenGetSubCategoryId()
    {
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<SubCategory, bool>>>(), It.IsAny<string[]>()))
            .ReturnsAsync(new SubCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Test Category",
                Category = new Category
                {
                    CategoryName = "Test Category Name"
                }
            });
        var result = await _subCategoryService.GetCategoryId(Guid.NewGuid());
        Assert.NotNull(result);
    }
    [Fact]
    public async Task ShouldGetAllSubCategory()
    {
        var expected = new List<SubCategory>
        {
            new SubCategory { Id = Guid.NewGuid(), CategoryName = "Makanan" },
            new SubCategory { Id = Guid.NewGuid(), CategoryName = "Minuman" },
        };
        _mockRepository.Setup(repo => repo.FindAll()).ReturnsAsync(expected);
        var result = await _subCategoryService.GetAll();
        Assert.NotNull(result);
        Assert.Equal(expected,result);
       
    }

  // [Fact]
  // public async Task Should_ReturnEmpty_When_DeleteSubCategory()
  // {
  //   var email = "test@unit.com";
  //   _mockRepository.Setup(repo => repo.Find(u => u.Email.Equals(_user.Email))).ReturnsAsync(_user);
  //
  //   var result = await _authService.LoadRegisterEmail(email);
  //
  //   Assert.NotEqual(result, _user.Email);
  // }
  
  // [Fact]
  // public async Task Should_ReturnUnauthorizedException_When_LoadRegisterEmailIsNotNull()
  // {
  //   var email = "test@unit.com";
  //   _mockRepository.Setup(repo => repo.Find(u => u.Email.Equals(email))).ReturnsAsync(_user);
  //   await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.LoadRegisterEmail(email));
  // }

  
}
