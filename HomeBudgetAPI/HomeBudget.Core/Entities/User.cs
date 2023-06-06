namespace HomeBudget.Core.Entities
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}