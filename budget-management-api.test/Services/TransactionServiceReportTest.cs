using System.Linq.Expressions;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;
using budget_management_api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace budget_management_api.test.Services;

public class TransactionServiceReportTest
{
    private readonly Mock<IRepository<Transactional>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<IWalletService> _mockWalletService;
    private readonly Mock<ISubCategoryService> _mockSubCatService;

    private readonly ITransactionService _transactionService;

    public TransactionServiceReportTest()
    {
        _mockRepository = new Mock<IRepository<Transactional>>();
        _mockPersistence = new Mock<IPersistence>();
        _mockWalletService = new Mock<IWalletService>();
        _mockSubCatService = new Mock<ISubCategoryService>();
        _transactionService = new TransactionService(_mockRepository.Object, _mockPersistence.Object, 
            _mockWalletService.Object, _mockSubCatService.Object);
    }
    
    // Total monthly income
    [Fact]
    public async Task Should_ReturnDailyMonthlyTotal_When_TotalMonthlyIncome()
    {
        // Arrange
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Income, Balance = 10000 });
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional> {
                new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Income, Balance = 10000 },
                new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Income, Balance = 10000 }
            });
        
        // Act
        var result = await _transactionService.TotalMonthlyIncome(Guid.NewGuid(), 2);

        // Assert
        Assert.Equal(20000, result.Balance);
        Assert.Equal(10000, result.Average);
        Assert.NotNull(result.Month);
    }

    [Fact]
    public async Task Should_ReturnNotFoundException_When_TotalMonthlyIsNull()
    {
        // Arrange
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync((Transactional)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _transactionService.TotalMonthlyIncome(Guid.NewGuid(), 2));
    }

    [Fact]
    public async Task Should_ReturnZero_When_TotalMonthlyAvgIs0()
    {
        // Arrange
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Income, Balance = 10000 });
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional>());
        
        // Act
        var result = await _transactionService.TotalMonthlyIncome(Guid.NewGuid(), 2);

        // Assert
        Assert.Equal(0, result.Average);
    }

    // Daily Report Income
    [Fact]
    public async Task Should_ReturnIEnumDailyIncomeReportResponse_When_DailyReportIncome()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var date = 2;
        
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional>
            {
                new Transactional(){TransDate = new DateTime(2023, 2, 1), Type = ETransactionType.Income, UserId = userId, Balance = 1000},
                new Transactional(){TransDate = new DateTime(2023, 2, 2), Type = ETransactionType.Income, UserId = userId, Balance = 2000}
            });
        
        // Act
        var result = await _transactionService.DailyReportIncome(userId, date);
    
        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("01 February 2023", result.ElementAt(0).TransDate);
        Assert.Equal(1000, result.ElementAt(0).In);
        Assert.Equal("02 February 2023", result.ElementAt(1).TransDate);
        Assert.Equal(2000, result.ElementAt(1).In);
    }

    [Fact]
    public async Task Should_ReturnNotFoundException_When_DailyReportIncomeIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;

        await  Assert.ThrowsAsync<NotFoundException>(() => _transactionService.DailyReportIncome(userId, month));
    }
    
    // Total Monthly Expend
    [Fact]
    public async Task Should_ReturnDailyMonthlyTotal_When_TotalMonthlyExpend()
    {
        // Arrange
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Income, Balance = 10000 });
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional> {
                new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Expand, Balance = 10000 },
                new Transactional { TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Expand, Balance = 10000 }
            });
        
        // Act
        var result = await _transactionService.TotalMonthlyExpend(Guid.NewGuid(), 2);

        // Assert
        Assert.Equal(20000, result.Balance);
        Assert.Equal(10000, result.Average);
        Assert.NotNull(result.Month);
    }

    [Fact]
    public async Task Should_ReturnNotFoundException_When_TotalMonthlyExpendIsNull()
    {
        // Arrange
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync((Transactional) null);

        await Assert.ThrowsAsync<NotFoundException>(() => _transactionService.TotalMonthlyExpend(Guid.NewGuid(), 2));
    }

    [Fact]
    public async Task Should_ReturnZero_When_TotalMonthlyExpendAvgIs0()
    {
        _mockRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new Transactional {TransDate = DateTime.Now, UserId = Guid.NewGuid(), Type = ETransactionType.Expand, Balance = 10000});
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional>());

        // Act
        var result = await _transactionService.TotalMonthlyExpend(Guid.NewGuid(), 2);
        
        // Result
        Assert.Equal(0, result.Average);
    }
    
    // Daily Report Expend
    [Fact]
    public async Task Should_ReturnIEnumDailyExpendReportResponse_When_DailyReportExpendIsSuccess()
    {
        // Arrange
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>(), 
                It.IsAny<string[]>()))
            .ReturnsAsync(new List<Transactional>
            {
                new Transactional
                {
                    TransDate = new DateTime(2022, 2, 1),
                    Balance = 1000,
                    Type = ETransactionType.Expand,
                    SubCategory = new SubCategory
                    {
                        CategoryName = "Food",
                        Category = new Category
                        {
                            CategoryName = "Expend"
                        }
                    }
                },
                new Transactional
                {
                    TransDate = new DateTime(2022, 2, 2),
                    Balance = 500,
                    Type = ETransactionType.Expand,
                    SubCategory = new SubCategory
                    {
                        CategoryName = "Transportation",
                        Category = new Category
                        {
                            CategoryName = "Expend"
                        }
                    }
                }
            });
        
        var userId = Guid.NewGuid();
        var date = 2;

        // Act
        var result = await _transactionService.DailyReportExpend(userId, date);

        // Assert
        Assert.NotEmpty(result);
        foreach (var res in result)
        {
            Assert.NotNull(res.TransDate);
            Assert.NotNull(res.Out);
            Assert.NotNull(res.SubCategory);
            Assert.NotNull(res.Category);
        }
    }

    [Fact]
    public async Task Should_ReturnNotFoundException_When_DailyReportExpendIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var month = 2;

        await  Assert.ThrowsAsync<NotFoundException>(() => _transactionService.DailyReportExpend(userId, month));
    }

    // Total income
    [Fact]
    public async Task Should_ReturnLong_When_TotalIncome()
    {
        // Arrange
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional>
            {
                new Transactional
                {
                    Balance = 2000,
                    Type = ETransactionType.Income,
                    UserId = Guid.NewGuid()
                },
                new Transactional
                {
                    Balance = 2000,
                    Type = ETransactionType.Income,
                    UserId = Guid.NewGuid()
                }
            });
        
        var userId = Guid.NewGuid();

        // Act
        var result = await _transactionService.TotalIncome(userId);

        // Assert
        Assert.Equal(4000, result);
    }
    
    // Total Expend
    [Fact]
    public async Task Should_ReturnLong_When_TotalExpend()
    {
        // Arrange
        _mockRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Transactional, bool>>>()))
            .ReturnsAsync(new List<Transactional>
            {
                new Transactional
                {
                    Balance = 1000,
                    Type = ETransactionType.Expand,
                    UserId = Guid.NewGuid()
                },
                new Transactional
                {
                    Balance = 1500,
                    Type = ETransactionType.Expand,
                    UserId = Guid.NewGuid()
                }
            });
        
        // act
        var result = await _transactionService.TotalExpend(Guid.NewGuid());
        
        // Assert
        Assert.Equal(2500, result);
    }
}