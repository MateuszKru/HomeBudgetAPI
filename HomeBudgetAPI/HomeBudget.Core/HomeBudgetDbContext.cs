using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContext : DbContext
    {
        public HomeBudgetDbContext(DbContextOptions<HomeBudgetDbContext> options) : base(options) { }
    }
}
