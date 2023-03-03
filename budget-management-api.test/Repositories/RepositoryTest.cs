using System.Linq.Expressions;
using budget_management_api.Entities;
using budget_management_api.Repositories;
using budget_management_api.Services;
using budget_management_api.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace budget_management_api.test.Repositories;

public class RepositoryTest
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly AppDbContext _context;
    private readonly IRepository<Bill> _repoBilling;
    private Bill _bill;
    private List<Bill> _listBills;

    public RepositoryTest()
    {
        var optionDbContext = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "DB_budgetManagement")
            .Options;

        _context = new AppDbContext(optionDbContext);
        _mockContext = new Mock<AppDbContext>(optionDbContext);
        _repoBilling = new Repository<Bill>(_mockContext.Object);
        _bill = new Bill
        {
            Id = Guid.NewGuid(),
            BillName = "tagihan listrik",
            BillDate = new DateTime(2023, 02, 5),
            BillAmount = 100000,
            UserId = Guid.NewGuid()
        };

        _listBills = new List<Bill>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                BillName = "tagihan listrik",
                BillDate = new DateTime(2023, 02, 5),
                BillAmount = 100000,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                BillName = "tagihan rumah",
                BillDate = new DateTime(2023, 02, 5),
                BillAmount = 100000,
                UserId = Guid.NewGuid()
            }
        };


    }

    [Fact]
    public async Task Should_ReturnBill_When_Save()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Save_SavesEntityToDatabase")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var repository = new Repository<Bill>(context);

            // Act
            var result = await repository.Save(_bill);

            // Assert
            Assert.Equal(_bill, result);
        }
    }

    [Fact]
    public void Should_ReturnBill_When_Attach()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Save_SavesEntityToDatabase")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var repository = new Repository<Bill>(context);

            // Act
            var result = repository.Attach(_bill);

            // Assert
            Assert.Equal(_bill, result);
        }
    }

    [Fact]
    public async Task Should_ReturnBill_When_FindById()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Save_SavesEntityToDatabase")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var repository = new Repository<Bill>(context);

            // Act
            var save = await repository.Save(_bill);
            var result = await repository.FindById(save.Id);

            // Assert
            Assert.NotNull(result);

        }
    }

    [Fact]
    public async Task Should_ReturnBill_When_Find()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Save_SavesEntityToDatabase")
            .Options;
        
        using (var context = new AppDbContext(options))
        {
            var repository = new Repository<Bill>(context);
        
            // Act
            var saveBill = await repository.Save(_bill);
            await context.SaveChangesAsync();
            
            var result = await repository.Find(user => user.Id.Equals(saveBill.Id));
        
            // Assert
            Assert.NotNull(result);
            Assert.Equal(saveBill.Id, result.Id);
        }
    }
}