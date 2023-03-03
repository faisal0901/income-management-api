using budget_management_api.Dtos;
using budget_management_api.Entities;

namespace budget_management_api.Services;

public interface IAuthService
{
    Task<User> LoadByEmail(string email);
    Task<string> LoadRegisterEmail(string email);
    Task<RegisterResponse> RegisterFree(RegisterRequest request);
    Task<RegisterResponse> RegisterPremium(RegisterRequest request);
    Task<LoginResponse> Login(LoginRequest request);
}