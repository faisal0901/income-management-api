using budget_management_api.Dtos;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;
using budget_management_api.Security;

namespace budget_management_api.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _repository;
    private readonly IPersistence _persistence;
    private readonly IWalletService _walletService;
    private readonly ITokenService _tokenService;
    private readonly IJwtUtil _jwtUtil;

    public AuthService(IRepository<User> repository, IPersistence persistence, IJwtUtil jwtUtil, IWalletService walletService, ITokenService tokenService)
    {
        _repository = repository;
        _persistence = persistence;
        _jwtUtil = jwtUtil;
        _walletService = walletService;
        _tokenService = tokenService;
    }
    
    public async Task<User> LoadByEmail(string email)
    {
        var user = await _repository.Find(user => user.Email.Equals(email));
        if (user is null) throw new NotFoundException("user not found");
        return user;
    }

    public async Task<string> LoadRegisterEmail(string email)
    {
        var user = await _repository.Find(user => user.Email.Equals(email));
        if (user is not null) throw new UnauthorizedException("Email has already registered, Login Please!");
        return email;
    }

    public async Task<RegisterResponse> RegisterFree(RegisterRequest request)
    {
        var userEmail = await LoadRegisterEmail(request.Email);
        var registerResponse = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var user = new User
            {
                Email = userEmail,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Role = ERole.Free,
            }; 
            var saveUser = await _repository.Save(user);
            await _walletService.CreateNewWallet(EWalletType.EmergencyWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.NeedsWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.WantsWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.SavingWallet,user.Id);
            await _persistence.SaveChangesAsync();

            return new RegisterResponse
            {
                Email = saveUser.Email,
                Role = saveUser.Role.ToString()
            };
        });

        return registerResponse;
    }

    public async Task<RegisterResponse> RegisterPremium(RegisterRequest request)
    {
        var userEmail = await LoadRegisterEmail(request.Email);
        var registerResponse = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var user = new User
            {
                Email = userEmail,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Role = ERole.Premium,
            };
            var saveUser = await _repository.Save(user);
            await _persistence.SaveChangesAsync();
            await _walletService.CreateNewWallet(EWalletType.EmergencyWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.NeedsWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.WantsWallet,user.Id);
            await _walletService.CreateNewWallet(EWalletType.SavingWallet,user.Id);
            await _persistence.SaveChangesAsync();
            
            return new RegisterResponse
            {
                Email = saveUser.Email,
                Role = saveUser.Role.ToString()
            };
        });

        return registerResponse;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await LoadByEmail(request.Email);
        var verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!verify) throw new UnauthorizedException("Unauthorized");
      
        var token = _jwtUtil.GenerateToken(user);
        await _tokenService.InsertToken(token, user.Id);
        return new LoginResponse
        {
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }
}