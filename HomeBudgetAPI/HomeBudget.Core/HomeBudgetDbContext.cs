using HomeBudget.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HomeBudget.Core
{
    public class HomeBudgetDbContext : DbContext
    {
        public HomeBudgetDbContext(DbContextOptions<HomeBudgetDbContext> options) : base(options)
        {
        }

        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}