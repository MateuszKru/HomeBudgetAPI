using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;

namespace HomeBudget.Core.ContextSeeder
{
    public static class HomeBudgetDbContextSeederHelper
    {
        public static IEnumerable<Role> GetRoles()
        {
            List<Role> userRoles = new();

            foreach (string userRoleName in Enum.GetNames(typeof(UserRoleEnum)))
            {
                userRoles.Add(new Role() { Name = userRoleName });
            }
            return userRoles;
        }

        public static IEnumerable<Budget> GetSampleBudgets()
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