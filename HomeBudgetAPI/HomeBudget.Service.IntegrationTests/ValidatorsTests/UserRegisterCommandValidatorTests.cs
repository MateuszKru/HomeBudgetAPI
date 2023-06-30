using FluentValidation.TestHelper;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.Actions.UserActions.Register;
using HomeBudget.Service.ModelsDTO.UserModels;
using HomeBudget.Service.Validators.UserValidators.cs;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.IntegrationTests.ValidatorsTests
{
    public class UserRegisterCommandValidatorTests
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly UserRegisterCommandValidator _validator;

        public UserRegisterCommandValidatorTests()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HomeBudgetDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("HomeBudgetDbTest");

            _dbContext = new HomeBudgetDbContext(dbContextOptionsBuilder.Options);

            Seed();

            _validator = new UserRegisterCommandValidator(_dbContext);
        }

        private void Seed()
        {
            var users = new List<User>()
            {
                new User()
                {
                    Email = "test1@mail.com",
                    FirstName = "Piotr",
                    LastName = "Nowicki",
                    PasswordHash = "hashedPassword123"
                },
                new User()
                {
                    Email = "test2@mail.com",
                    FirstName = "Tomasz",
                    LastName = "Kowalski",
                    PasswordHash = "hashedPassword112233"
                },
            };
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();
        }

        public static IEnumerable<object[]> GetNewUserDTOValidModelList()
        {
            var users = new List<UserRegisterCommand>()
            {
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test3@mail.com",
                        FirstName= "Adam",
                        LastName= "Nowak",
                        Password= "Password",
                        ConfirmPassword= "Password"
                    }
                },
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test4@mail.com",
                        FirstName= "Jan",
                        LastName= "Kowal",
                        Password= "Password123",
                        ConfirmPassword= "Password123"
                    }
                }
            };

            return users.Select(u => new object[] { u });
        }

        public static IEnumerable<object[]> GetNewUserDTOInvalidModelList()
        {
            var users = new List<UserRegisterCommand>()
            {
                // used Email
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test1@mail.com",
                        FirstName= "Adam",
                        LastName= "Nowak",
                        Password= "Password",
                        ConfirmPassword= "Password"
                    }
                },
                // wrong Email format
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "testmail.com",
                        FirstName= "Adam",
                        LastName= "Nowak",
                        Password= "Password",
                        ConfirmPassword= "Password"
                    }
                },
                // empty Email
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "",
                        FirstName= "Piotr",
                        LastName= "Kowal",
                        Password= "Password123",
                        ConfirmPassword= "Password123"
                    }
                },
                // Confirm password not the same as Password
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test4@mail.com",
                        FirstName= "Piotr",
                        LastName= "Kowal",
                        Password= "Password12",
                        ConfirmPassword= "Password123"
                    }
                },
                // Password shorter than 6 characters
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test4@mail.com",
                        FirstName= "Piotr",
                        LastName= "Kowal",
                        Password= "Pass",
                        ConfirmPassword= "Pass"
                    }
                },
                // empty FirstName
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test4@mail.com",
                        FirstName= "",
                        LastName= "Kowal",
                        Password= "Password123",
                        ConfirmPassword= "Password123"
                    }
                },
                // empty LastName
                new UserRegisterCommand()
                {
                    NewUser = new NewUserDTO()
                    {
                        Email = "test4@mail.com",
                        FirstName= "Piotr",
                        LastName= "",
                        Password= "Password123",
                        ConfirmPassword= "Password123"
                    }
                }
            };

            return users.Select(u => new object[] { u });
        }

        [Theory]
        [MemberData(nameof(GetNewUserDTOValidModelList))]
        public void Validate_ForValidModel_ReturnsSuccess(UserRegisterCommand model)
        {
            // arrange

            // act

            var result = _validator.TestValidate(model);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetNewUserDTOInvalidModelList))]
        public void Validate_ForInvalidModel_ReturnsError(UserRegisterCommand model)
        {
            // arrange

            // act

            var result = _validator.TestValidate(model);

            // assert

            result.ShouldHaveAnyValidationError();
        }
    }
}