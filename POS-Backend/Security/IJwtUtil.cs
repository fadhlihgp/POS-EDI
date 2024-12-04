using POS_Backend.Entities;

namespace POS_Backend.Security;

public interface IJwtUtil
{
    string GenerateToken(Account account);
}