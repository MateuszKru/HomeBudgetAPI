using FluentValidation.TestHelper;
using HomeBudget.Service.Actions.UserActions.Login;
using HomeBudget.Service.Validators.UserValidators.cs;

namespace HomeBudget.Service.IntegrationTests.ValidatorsTests
{
    public class UserLoginCommandValidatorTests
    {
        private readonly UserLoginCommandValidator _validator;

        public UserLoginCommandValidatorTests()
        {
            _validator = new UserLoginCommandValidator();
        }

        public static IEnumerable<object[]> GetUserLoginValidModelList()
        {
            var users = new List<UserLoginCommand>()
            {
                new ()
                {
                    Email = "test1@mail.com",
                    Password = "Password",
                },
                new UserLoginCommand()
                {
                    Email = "test1123@mail.com",
                    Password = "Password",
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
            };

            return users.Select(u => new object[] { u });
        }

        [Theory]
        [MemberData(nameof(GetUserLoginValidModelList))]
        public void Validate_ForValidModel_ReturnsSuccess(UserLoginCommand model)
        {
            // arrange

            // act

            var result = _validator.TestValidate(model);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetUserLoginInvalidModelList))]
        public void Validate_ForInvalidModel_ReturnsError(UserLoginCommand model)
        {
            // arrange

            // act

            var result = _validator.TestValidate(model);

            // assert

            result.ShouldHaveAnyValidationError();
        }
    }
}