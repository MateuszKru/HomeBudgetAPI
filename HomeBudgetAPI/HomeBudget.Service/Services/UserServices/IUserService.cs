using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;

namespace HomeBudget.Service.Services.UserServices
{
    public interface IUserService
    {
        Role GetRole(UserRoleEnum userRole);

        Task<Role> GetRoleAsync(UserRoleEnum userRole);
    }
}