using HomeBudget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeBudget.Core.EntitiesConfig
{
    public class BudgetEntityConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.Property(x => x.FullAmount).HasPrecision(18, 2);
        }
    }
}