namespace budget_management_api.Services;

public interface ITokenService
{
    Task Revoke(string token,Guid userId);
    Task InsertToken(string token,Guid userId);
    Task<bool> IsTokenRevoked(string token);
}