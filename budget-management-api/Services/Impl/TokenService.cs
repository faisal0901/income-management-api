using budget_management_api.Entities;
using budget_management_api.Repositories;

namespace budget_management_api.Services.Impl;

public class TokenService:ITokenService
{
    private readonly IRepository<Token> _tokenRepository;
    private readonly IPersistence _persistence;
    public async Task<bool> IsTokenRevoked(string token)
    {
        var existingToken = await _tokenRepository.Find(t=>t.TokenValue==token&&t.Revoked==1);

        if (existingToken != null)
        {
            // Memeriksa apakah token telah dicabut berdasarkan status di repository
            return true;
        }

        // Jika token tidak ditemukan di repository, anggap token tidak dicabut
        return false;
    }
    public TokenService(IRepository<Token> tokenRepository,IPersistence persistence)
    {
        _tokenRepository = tokenRepository;
        _persistence = persistence;
    }

    public  Task Revoke(string token,Guid userId)
    {
        throw new NotImplementedException();
    }
    
    public async Task InsertToken(string token,Guid userId)
    {
        var tokenInsert = new Token()
        {
            Revoked = 0,
            UserId = userId,
            TokenValue = token,
            CreatedAt = new DateTime()
        };
        await _tokenRepository.Save(tokenInsert);
        await _persistence.SaveChangesAsync();
    }

   
}