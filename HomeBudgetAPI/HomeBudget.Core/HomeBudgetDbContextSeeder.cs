using Azure.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContextSeeder
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeBudgetDbContextSeeder> _logger;

        public HomeBudgetDbContextSeeder(
            HomeBudgetDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IConfiguration configuration,
            ILogger<HomeBudgetDbContextSeeder> logger)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await CheckMigrations();

            await CheckUserRoles();

            await CheckAdminUser();

            if (!_dbContext.Budgets.Any())
            {
                var bugdets = GetSampleBudgets();
                await _dbContext.Budgets.AddRangeAsync(bugdets);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckMigrations()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations != null && pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        private async Task CheckUserRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                var userRoles = GetUserRoles();
                foreach (var role in userRoles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task CheckAdminUser()
        {
            if (_userManager.GetUsersInRoleAsync(Enum.GetName(UserRoleEnum.Admin)).Result.Count == 0)
            {
                var adminUserData = _configuration.GetSection("AdminUserData");

                User adminUser = new()
                {
                    UserName = "HomeBudgetAdmin",
                    FirstName = adminUserData["FirstName"],
                    LastName = adminUserData["LastName"],
                    Email = adminUserData["Email"],
                    PhoneNumber = adminUserData["PhoneNumber"]
                };

                var createAdminResult = await _userManager.CreateAsync(adminUser, adminUserData["Password"]);

                if (!createAdminResult.Succeeded)
                {
                    string errorMessage = $"Error with create administrator user. Check appsettings file. {string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(" ", createAdminResult.Errors.Select(x => x.Description).ToList()))}";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(adminUser, Enum.GetName(UserRoleEnum.Admin));
            }
        }

        private IEnumerable<Budget> GetSampleBudgets()
        {
            var bugdets = new List<Budget>()
            {
                new Budget()
                {
                    BugdetMonth= 1,
                    FullAmount= 5000,
                },
                new Budget()
                {
                    BugdetMonth= 2,
                    FullAmount= 8000,
                }
            };
            return bugdets;
        }

        private IEnumerable<IdentityRole> GetUserRoles()
        {
            var userRoles = new List<IdentityRole>();

            foreach (var userRole in Enum.GetNames(typeof(UserRoleEnum)))
            {
                userRoles.Add(new IdentityRole(userRole));
            }
            return userRoles;
        }
    }
}