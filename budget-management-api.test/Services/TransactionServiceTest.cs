using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Repositories;
using budget_management_api.Services;
using Moq;

namespace budget_management_api.test.Services;

public class TransactionServiceTest
{

    private readonly Mock<IRepository<Transactional>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<IWalletService> _mockWalletService;
    private readonly Mock<ISubCategoryService> _mockSubCatService;

    private readonly ITransactionService _transactionService;

    public TransactionServiceTest()
    {
        _mockRepository = new Mock<IRepository<Transactional>>();
        _mockPersistence = new Mock<IPersistence>();
        _mockWalletService = new Mock<IWalletService>();
        _mockSubCatService = new Mock<ISubCategoryService>();
        _transactionService = new TransactionService(_mockRepository.Object, _mockPersistence.Object, 
            _mockWalletService.Object, _mockSubCatService.Object);
    }
    [Fact]
    public async  Task Should_returnTransactionWhenCreateNewTransaction(){
        var trans = new Transactional
        {
            Balance = 1000,
            UserId = Guid.NewGuid()
        };
        _mockRepository
        .Setup(x => x.Save(It.IsAny<Transactional>()))
        .ReturnsAsync(trans);
        var result = await _transactionService.CreateNewTransactional(trans);
        Assert.NotNull(result);
    }
   
    // [Fact]
    // public async Task ShouldCreateIncome()
    // {
    //     var trans = new Transactional
    //     {
    //         Balance = 1000,
    //         UserId = Guid.NewGuid()
    //     };
    //     // Arrange
    //     _mockRepository
    //         .Setup(x => x.Save(It.IsAny<Transactional>()))
    //         .ReturnsAsync(trans);
    //     _mockWalletService
    //         .Setup(x => x.AddWalletBalance(It.IsAny<EWalletType>(), It.IsAny<long>(), It.IsAny<Guid>()))
    //         .ReturnsAsync(new Wallet());
    //     _mockPersistence
    //         .Setup(x => x.SaveChangesAsync());
    //       
    //
    //     // Act
    //     var result = await _transactionService.CreateNewIncome(trans);
    //
    //     // Assert
    //    
    //     Assert.Equal(trans.Balance, result.Balance);
    //     Assert.Equal(trans.UserId.ToString(), result.UserId);
    //     // Assert.Equal(50, result.needsBalance);
    //     // Assert.Equal(30, result.wantsBalance);
    //     // Assert.Equal(15, result.savingBalance);
    //     // Assert.Equal(5, result.emergencyBalance);
    //
    //     _mockRepository.Verify(x => x.Save(It.IsAny<Transactional>()), Times.Once());
    //     _mockWalletService.Verify(x => x.AddWalletBalance(It.IsAny<EWalletType>(), It.IsAny<long>(), It.IsAny<Guid>()), Times.Exactly(4));
    //     _mockPersistence.Verify(x => x.SaveChangesAsync(), Times.Once());
    // }
    // [Fact]
    // public async Task ExpendTransaction_CreateNewExpend()
    // {
    //     var payload = new Transactional()
    //     {
    //         UserId = Guid.NewGuid(),
    //         Balance = 1000,
    //         SubCategoryId = Guid.NewGuid()
    //     };
    //     var category = new SubCategory
    //     {
    //        Id = Guid.NewGuid(),
    //        CategoryName = "coffe",
    //        CategoryId = Guid.NewGuid();
    //     };
    //   
    //         _mockSubCatService.Setup(x => x.GetCategoryId((Guid)payload.SubCategoryId))
    //             .ReturnsAsync(category);
    //     var wallet = new Wallet
    //     {
    //         Id = Guid.NewGuid(),
    //         WalletType = EWalletType.NeedsWallet
    //     };
    //     _mockWalletService.Setup(x => x.SubtractWalletBalance(EWalletType.NeedsWallet, payload.Balance, payload.UserId))
    //         .ReturnsAsync(wallet);
    //     var transactional = new Transactional
    //     {
    //         Id = Guid.NewGuid(),
    //         Balance = payload.Balance,
    //         TransDate = DateTime.Now,
    //         WalletId = wallet.Id
    //     };
    //     _mockRepository.Setup(x => x.Save(It.IsAny<Transactional>()))
    //         .ReturnsAsync(transactional);
    //     _mockPersistence.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
    //     var result = await _transactionService.CreateNewExpand(payload);
    //    Assert.NotNull(result);
    // }
}

