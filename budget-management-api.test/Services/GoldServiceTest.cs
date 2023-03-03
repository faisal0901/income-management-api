using System.Linq.Expressions;
using budget_management_api.Dtos;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Repositories;
using budget_management_api.Services;
using budget_management_api.Services.Impl;
using Moq;

namespace budget_management_api.test.Services;

public class GoldServiceTest
{
    private readonly Mock<IRepository<Gold>> _goldRepository;
    private readonly Mock<ITransactionService> _transactionService;
    private readonly Mock<IWalletService> _walletService;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<ISubCategoryService> _subCategoryService;
    private readonly IGoldService _goldService;
    public GoldServiceTest()
    {
        _goldRepository = new Mock<IRepository<Gold>>();
        _walletService = new Mock<IWalletService>();
        _mockPersistence = new Mock<IPersistence>();
        _transactionService = new Mock<ITransactionService>();
        _subCategoryService = new Mock<ISubCategoryService>();
        _goldService = new GoldService(_goldRepository.Object, 
            _walletService.Object,
            _mockPersistence.Object,
            _transactionService.Object,
            _subCategoryService.Object
        );

    }
    [Fact]
    public async Task ShouldReturnCurrentPriceGold()
    {
        var data = await _goldService.GetCurrentPriceGold();
        Assert.NotNull(data);
    }
    [Fact]
    public async Task ShouldReturnIDRPriceGold()
    {
        var data = await _goldService.ConvertGoldToIdr();
        Assert.NotNull(data);
    }
    [Fact]
    public async Task ShouldReturnGoldReportResponse()
    {
        var userId = Guid.NewGuid();
        var wallet = new Wallet() { Id = Guid.NewGuid(), WalletType = EWalletType.SavingWallet, UserId = userId };
        _walletService.Setup(w => w.GetWalletByType(EWalletType.SavingWallet, userId)).ReturnsAsync(wallet);

        var goldList = new List<Gold>
        {
            new Gold() { Id = Guid.NewGuid(), WalletId = wallet.Id, Price = 100, Gram = 10 },
            new Gold() { Id = Guid.NewGuid(), WalletId = wallet.Id, Price = 200, Gram = 20 }
        };
        _goldRepository.Setup(g => g.FindAll(It.IsAny<Expression<Func<Gold, bool>>>())).ReturnsAsync(goldList);

  

    
        var result = await _goldService.GetGold(userId);

     
        Assert.NotNull(result);
    }
    [Fact]
    public async Task ShouldReturnGoldRespone_WhenBuyNewGold()
    {
        var request = new GoldRequest()
        {
            UserId = Guid.NewGuid(),
            gram = 10
        };
        var expectedGold = new Gold()
        {
            Gram = request.gram,
            WalletId = Guid.NewGuid(),
            Price = 1000,
            TransDate = DateTime.Now
        };
        _goldRepository.Setup(x => x.Save(It.IsAny<Gold>())).ReturnsAsync(expectedGold);
        _mockPersistence.Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task<GoldResponse>>>()))
            .Returns(Task.FromResult(new GoldResponse()
            {
                Gram = expectedGold.Gram,
                WalletType = EWalletType.SavingWallet.ToString(),
                price = 100,
                total = 1000,
                UserId = request.UserId.ToString()
            }));
        var result = await _goldService.BuyNewGold(request);
        Assert.NotNull(result);
        Assert.Equal(expectedGold.Gram, result.Gram);
        Assert.Equal(100, result.price);
        Assert.Equal(1000, result.total);
    }
}