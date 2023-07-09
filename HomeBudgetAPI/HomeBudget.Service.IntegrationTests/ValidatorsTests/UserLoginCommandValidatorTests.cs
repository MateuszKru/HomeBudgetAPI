using FluentValidation.TestHelper;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using HomeBudget.Service.Actions.UserActions.Login;
using HomeBudget.Service.Services.UserServices;
using HomeBudget.Service.Validators.UserValidators.cs;
using HomeBudgetAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.IntegrationTests.ValidatorsTests
{
    public class UserLoginCommandValidatorTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IServiceScopeFactory? _scopefactory;
        private readonly HomeBudgetDbContext _dbContext;

        public UserLoginCommandValidatorTests(WebApplicationFactory<Startup> factory)
        {
            _scopefactory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped<IUserService, UserService>();
                    });
                }).Services.GetService<IServiceScopeFactory>();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HomeBudgetDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("HomeBudgetDbTestLogin");

            _dbContext = new HomeBudgetDbContext(dbContextOptionsBuilder.Options);

            Seed();
        }

        private void Seed()
        {
            if (!_dbContext.Users.Any())
            {
                using var scope = _scopefactory?.CreateScope();
                var userService = scope?.ServiceProvider.GetService(typeof(IUserService)) as IUserService;

                var user1 = new User()
                {
                    Email = "test1@mail.com",
                    FirstName = "Piotr",
                    LastName = "Nowicki",
                    Role = userService?.GetRole(UserRoleEnum.User)
                };
                user1.PasswordHash = userService?.HashPassword(user1, "hashedPassword123");

                var user2 = new User()
                {
                    Email = "test2@mail.com",
                    FirstName = "Tomasz",
                    LastName = "Kowalski",
                    Role = userService?.GetRole(UserRoleEnum.User)
                };
                user2.PasswordHash = userService?.HashPassword(user2, "hashedPassword112233");

                List<User> users = new() { user1, user2 };

                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();
            }
        }

        private IUserService? GetUserService()
        {
            using var scope = _scopefactory?.CreateScope();

            return scope?.ServiceProvider.GetService(typeof(IUserService)) as IUserService;
        }

        public static IEnumerable<object[]> GetUserLoginValidModelList()
        {
            var users = new List<UserLoginCommand>()
            {
                new ()
                {
                    Email = "test1@mail.com",
                    Password = "hashedPassword123",
                },
                new UserLoginCommand()
                {
                    Email = "test2@mail.com",
                    Password = "hashedPassword112233",
                },
            };

            return users.Select(u => new object[] { u });
        }

        public static IEnumerable<object[]> GetUserLoginInvalidModelList()
        {
            var users = new List<UserLoginCommand>()
            {
                // wrong Email format
                new UserLoginCommand()
                {
                    Email = "test1mail.com",
                    Password = "Password",
                },
                // empty Email
                new UserLoginCommand()
                {
                    Email = "",
                    Password = "Password",
                },
                // empty Password
                new UserLoginCommand()
                {
                    Email = "test1@mail.com",
                    Password = "",
                },
                // invalid password
                new UserLoginCommand()
                {
                    Email = "test1@mail.com",
                    Password = "hashedPassword12",
                },
            };

            return users.Select(u => new object[] { u });
        }

        [Theory]
        [MemberData(nameof(GetUserLoginValidModelList))]
        public void Validate_ForValidModel_ReturnsSuccess(UserLoginCommand model)
        {
            // arrange

            var userService = GetUserService();

            var validator = new UserLoginCommandValidator(userService, _dbContext);

            // act

            var result = validator.TestValidate(model);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetUserLoginInvalidModelList))]
        public void Validate_ForInvalidModel_ReturnsError(UserLoginCommand model)
        {
            // arrange

            var userService = GetUserService();

            var validator = new UserLoginCommandValidator(userService, _dbContext);

            // act

            var result = validator.TestValidate(model);

            // assert

            result.ShouldHaveAnyValidationError();
        }
    }
}