using HomeBudget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContextSeeder
    {
        private readonly HomeBudgetDbContext _dbContext;

        public HomeBudgetDbContextSeeder(HomeBudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();

            if (pendingMigrations != null && pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }

            if (!_dbContext.Budgets.Any())
            {
                var bugdets = GetSampleBudgets();
                _dbContext.AddRange(bugdets);
                _dbContext.SaveChanges();
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
