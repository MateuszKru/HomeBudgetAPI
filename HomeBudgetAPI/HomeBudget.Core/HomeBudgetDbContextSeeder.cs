using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Globalization;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContextSeeder
    {
        private readonly HomeBudgetDbContext _dbContext;

        public HomeBudgetDbContextSeeder(HomeBudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            await CheckMigrations();

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
    }
}