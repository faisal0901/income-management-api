using budget_management_api.Entities;

namespace budget_management_api.Security;

public interface IJwtUtil
{
    string GenerateToken(User user);
}