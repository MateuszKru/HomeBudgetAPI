using HomeBudget.Core.ContextSeeder;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.RegularExpressions;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContextSeeder
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<HomeBudgetDbContextSeeder> _logger;

        public HomeBudgetDbContextSeeder(
            HomeBudgetDbContext dbContext,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            ILogger<HomeBudgetDbContextSeeder> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (_dbContext.Database.IsRelational())
            {
                await CheckMigrations();
                await CheckUserRoles();
                await CheckAdminUser();
                await CheckBudgets();
            }
        }

        private async Task CheckMigrations()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations != null && pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();

                string logInformation = $"Migrations was applied on database in count {pendingMigrations.Count()}.";
                _logger.LogInformation(logInformation);
            }
        }

        private async Task CheckUserRoles()
        {
            if (!_dbContext.Roles.Any())
            {
                var userRoles = HomeBudgetDbContextSeederHelper.GetRoles();
                await _dbContext.Roles.AddRangeAsync(userRoles);
                await _dbContext.SaveChangesAsync();

                string logInformation = $"Roles was added to database: {string.Join(", ", userRoles.Select(x => x.Name).ToList())}.";
                _logger.LogInformation(logInformation);
            }
        }

        private async Task CheckAdminUser()
        {
            var adminUserData = _configuration.GetSection("AdminUserData");
            Role adminRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == Enum.GetName(UserRoleEnum.Admin));
            User adminUser = await _dbContext.Users.SingleOrDefaultAsync(y => y.Role == adminRole);

            CheckAdminUserData(adminUserData);

            if (adminUser == null)
            {
                User newAdminUser = new()
                {
                    FirstName = adminUserData["FirstName"],
                    LastName = adminUserData["LastName"],
                    Email = adminUserData["Email"],
                    Role = adminRole,
                };
                newAdminUser.PasswordHash = _passwordHasher.HashPassword(newAdminUser, adminUserData["Password"]);

                await _dbContext.AddAsync(newAdminUser);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckBudgets()
        {
            if (!_dbContext.Budgets.Any())
            {
                var bugdets = HomeBudgetDbContextSeederHelper.GetSampleBudgets();
                await _dbContext.Budgets.AddRangeAsync(bugdets);
                await _dbContext.SaveChangesAsync();
            }
        }

        private void CheckAdminUserData(IConfigurationSection adminUserData)
        {
            if (adminUserData != null)
            {
                if (string.IsNullOrEmpty(adminUserData["Email"]))
                {
                    _logger.LogCritical("Admin email was empty. Check appsettings file.");
                    throw new Exception("Admin email was empty. Check appsettings file.");
                }
                if (!IsValidEmail(adminUserData["Email"]))
                {
                    _logger.LogCritical("Admin email is not valid. Check appsettings file.");
                    throw new Exception("Admin email is not valid. Check appsettings file.");
                }
                if (string.IsNullOrEmpty(adminUserData["Password"]))
                {
                    _logger.LogCritical("Admin password was empty. Check appsettings file.");
                    throw new Exception("Admin password was empty. Check appsettings file.");
                }
                if (!IsValidPassword(adminUserData["Password"]))
                {
                    _logger.LogCritical("Admin password is not valid. Check appsettings file.");
                    throw new Exception("Admin password is not valid. Check appsettings file.");
                }
                if (string.IsNullOrEmpty(adminUserData["FirstName"]))
                {
                    _logger.LogCritical("Admin first name was empty. Check appsettings file.");
                    throw new Exception("Admin first name was empty. Check appsettings file.");
                }
                if (string.IsNullOrEmpty(adminUserData["LastName"]))
                {
                    _logger.LogCritical("Admin last name was empty. Check appsettings file.");
                    throw new Exception("Admin last name was empty. Check appsettings file.");
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
            return isValidated;
        }
    }
}