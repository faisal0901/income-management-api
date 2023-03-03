using System.Linq.Expressions;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Repositories;
using budget_management_api.Services;
using Moq;

namespace budget_management_api.test.Services;

public class WalletServiceTest
{
    private readonly Mock<IRepository<Wallet>> _walletRepository;
    private readonly Mock<IPersistence> _mockPersistence;

    private IWalletService _walletService;
    public WalletServiceTest()
    {
        _walletRepository = new Mock<IRepository<Wallet>>();
        _mockPersistence = new Mock<IPersistence>();
        _walletService = new WalletService(_walletRepository.Object, _mockPersistence.Object);
    }
    [Fact]
    public async Task should_return_wallet_whenCreateNewWallet()
    {
        var userId = Guid.NewGuid();
        var walletType = EWalletType.SavingWallet;
        var expectedResult = new Wallet
        {
            Id = Guid.NewGuid(),
            WalletBalance = 0,
            WalletType = walletType,
            UserId = userId
        };

        _walletRepository
            .Setup(x => x.Save(It.IsAny<Wallet>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _walletService.CreateNewWallet(walletType, userId);

        // Assert
        Assert.NotNull(result);
        
    }

    [Fact]
    public async Task Should_ReturnWalletWhenGetWalletByType()
    {
        var walletType = EWalletType.SavingWallet;
        var userId = Guid.NewGuid();

        var expectedWallet = new Wallet
        {
            Id = Guid.NewGuid(),
            WalletType = walletType,
            UserId = userId,
            WalletBalance = 1000
        };
       _walletRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Wallet, bool>>>()))
            .ReturnsAsync(expectedWallet);
        
        var result = await _walletService.GetWalletByType(walletType, userId);
        Assert.NotNull(result);
        Assert.Equal(expectedWallet,result);
    }

    [Fact]
    public async Task Should_return_wallet_When_AddWalletBalance()
    {
        var type = EWalletType.SavingWallet;
        var balance = 1000;
        var userId = Guid.NewGuid();
        var wallet = new Wallet
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            WalletType = type,
            WalletBalance = 0
        };
        _walletRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Wallet, bool>>>()))
            .ReturnsAsync(wallet);
        var result = await _walletService.AddWalletBalance(type, balance, userId);
        Assert.NotNull(result);
    
    }
    [Fact]
    public async Task Should_return_wallet_When_SubtractWalletBalance()
    {
        var type = EWalletType.SavingWallet;
        var balance = 1000;
        var userId = Guid.NewGuid();
        var wallet = new Wallet
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            WalletType = type,
            WalletBalance = 100000
        };
        _walletRepository
            .Setup(repo => repo.Find(It.IsAny<Expression<Func<Wallet, bool>>>()))
            .ReturnsAsync(wallet);
        _walletRepository .Setup(repo => repo.Update(It.IsAny<Wallet>()))
            .Returns((Wallet w) =>
            {
                w.WalletBalance -= balance;
                return w;
            });
        _mockPersistence.Setup(p => p.SaveChangesAsync())
            .Returns(Task.CompletedTask);
        var result = await _walletService.SubtractWalletBalance(type, balance, userId);
        Assert.NotNull(result);
    
    }
}