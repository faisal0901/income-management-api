using budget_management_api.Entities;
using budget_management_api.Enum;

namespace budget_management_api.Services;

public interface IWalletService
{
    public Task<Wallet> AddWalletBalance(EWalletType type, long balance,Guid userId);
    public Task<Wallet> SubtractWalletBalance(EWalletType type, long balance,Guid userId);
    public Task<Wallet?> CreateNewWallet(EWalletType type, Guid userId);
   public Task<Wallet?> GetWalletByType(EWalletType type, Guid userId);
}