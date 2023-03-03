using System.Linq.Expressions;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Repositories;
using budget_management_api.Services;
using Moq;

namespace budget_management_api.test.Services;

public class BillingServiceTest
{
    private readonly Mock<IRepository<Bill>> _mockRepo;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly IBillingService _service;

    public BillingServiceTest()
    {
        _mockRepo = new Mock<IRepository<Bill>>();
        _mockPersistence = new Mock<IPersistence>();
        _service = new BillingService(_mockRepo.Object, _mockPersistence.Object);
    }

    [Fact]
    public async Task Should_ReturnBillingResponse_When_Create()
    {
        var billIn = new Bill
        {
            Id = Guid.NewGuid(),
            BillName = "Listrik",
            BillDate = DateTime.Now,
            BillAmount = 200000,
            UserId = Guid.NewGuid(),
        };

        var response = new BillingResponse
        {
            Id = billIn.Id,
            BillName = billIn.BillName,
            BillAmount = billIn.BillAmount,
            EachBilledDate = billIn.BillDate,
            UserId = billIn.UserId
        };

        _mockRepo.Setup(x => x.Save(It.IsAny<Bill>()))
            .ReturnsAsync(billIn);
        _mockPersistence.Setup(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<Bill>>>()))
            .Callback<Func<Task<Bill>>>(func => func())
            .ReturnsAsync(billIn);

        var expected = response;

        var result = await _service.Create(billIn);
        
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.BillName, result.BillName);
    }

    [Fact]
    public async Task Should_ReturnIenum_When_GetAll()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bill1 = new Bill { Id = Guid.NewGuid(), UserId = userId };
        var bill2 = new Bill { Id = Guid.NewGuid(), UserId = userId };
        var bill3 = new Bill { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        
        _mockRepo.Setup(x => x.FindAll(It.IsAny<Expression<Func<Bill, bool>>>()))
            .ReturnsAsync(new List<Bill> { bill1, bill2, bill3 });
        
        // Act
        var bills = await _service.GetAll(userId);

        // Assert
        Assert.Equal(3, bills.Count());
        Assert.Contains(bills, b => b.Id == bill1.Id);
        Assert.Contains(bills, b => b.Id == bill2.Id);
    }
}