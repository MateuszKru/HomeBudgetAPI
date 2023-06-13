using FluentValidation;
using HomeBudget.Service.Actions.UserActions.Login;

namespace HomeBudget.Service.Validators.UserValidators.cs
{
    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithName(x => $"{nameof(x.Email)}");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName(x => $"{nameof(x.Password)}");
        }
    }
}