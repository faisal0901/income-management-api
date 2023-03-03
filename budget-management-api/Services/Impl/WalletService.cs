using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Repositories;

namespace budget_management_api.Services;

public class WalletService:IWalletService
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IPersistence _persistence;
   

    public WalletService(IRepository<Wallet> walletRepository, IPersistence persistence)
    {
        _walletRepository = walletRepository;
        _persistence = persistence;
    }

    public async Task<Wallet> AddWalletBalance(EWalletType type,long balance,Guid userId)
    {
       //cari tipe walletnya
       //select * from wallet where wallet.wallettype="1" and wallet.userId="1"
       var wallet = await _walletRepository.Find(w => w.WalletType == type && w.UserId == userId);
       //select * from wallet where wallet.wallettype="1" and wallet.userId="1"
       if (wallet == null)
       {
           return null;
       }
       wallet.WalletBalance += balance;
       _walletRepository.Update(wallet);
       await _persistence.SaveChangesAsync();
       return wallet;
    }

    public  async Task<Wallet> SubtractWalletBalance(EWalletType type, long balance,Guid userId)
    {
        var walletFind = await _walletRepository.Find(wallet => wallet.WalletType.Equals(type) && wallet.UserId.Equals(userId));
        if (walletFind == null)
        {
            throw new Exception("Wallet not found");
        }

        if (walletFind.WalletBalance < balance)
        {
            throw new Exception("Insufficient balance");
        }

        walletFind.WalletBalance -= balance;
        var entry = _walletRepository.Update(walletFind);
        await _persistence.SaveChangesAsync();
        return entry;
    }

    public async Task<Wallet?> GetWalletByType(EWalletType type,Guid userId)
    {
        var  walletFind = await _walletRepository.Find(wallet => wallet.WalletType.Equals(type)&&wallet.UserId.Equals(userId));
        return walletFind;
    }
    public async Task<Wallet?> CreateNewWallet(EWalletType type,Guid userId)
    {
        var walletInsert = new Wallet()
        {
            WalletBalance = 0,
            WalletType = type,
            UserId = userId,
        };
        var  walletFind = await _walletRepository.Save(walletInsert);
        return walletFind;
    }
}