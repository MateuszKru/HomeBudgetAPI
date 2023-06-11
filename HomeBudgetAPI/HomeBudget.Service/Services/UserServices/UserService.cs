using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HomeBudgetDbContext _dbContext;

        public UserService(HomeBudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetRole(UserRoleEnum userRole)
            => _dbContext.Roles.SingleOrDefault(r => r.Name == Enum.GetName(userRole));

        public async Task<Role> GetRoleAsync(UserRoleEnum userRole)
            => await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == Enum.GetName(userRole));
    }
}