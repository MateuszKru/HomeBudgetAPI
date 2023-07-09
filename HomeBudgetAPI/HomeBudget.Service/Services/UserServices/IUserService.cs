using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace HomeBudget.Service.Services.UserServices
{
    public interface IUserService
    {
        Role GetRole(UserRoleEnum userRole);

        Task<Role> GetRoleAsync(UserRoleEnum userRole);

        string GenerateTokenJWT(User user);

        string HashPassword(User user, string password);

        PasswordVerificationResult VerifyHashedPassword(User user, string password);
    }
}