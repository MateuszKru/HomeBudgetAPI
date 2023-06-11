using FluentValidation;
using HomeBudget.Core;
using HomeBudget.Service.Actions.UserActions.Register;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.Validators.UserValidators.cs
{
    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        private readonly HomeBudgetDbContext _dbContext;

        public UserRegisterCommandValidator(HomeBudgetDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.NewUser.FirstName)
                .NotEmpty()
                .WithName(x => $"{nameof(x.NewUser.FirstName)}");

            RuleFor(x => x.NewUser.LastName)
                .NotEmpty()
                .WithName(x => $"{nameof(x.NewUser.LastName)}");

            RuleFor(x => x.NewUser.Email)
                .NotEmpty()
                .EmailAddress()
                .Must(IsEmailNotUsed)
                .WithMessage("Spróbuj użyć innego adresu email.")
                .WithName(x => $"{nameof(x.NewUser.Email)}");

            RuleFor(x => x.NewUser.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithName(x => $"{nameof(x.NewUser.Password)}");

            RuleFor(x => x.NewUser.ConfirmPassword)
                .Equal(x => x.NewUser.Password)
                .WithMessage(x => $"Pole {nameof(x.NewUser.ConfirmPassword)} musi być takie samo jak pole {nameof(x.NewUser.Password)}.")
                .WithName(x => $"{nameof(x.NewUser.ConfirmPassword)}");
        }

        private bool IsEmailNotUsed(string email)
            => !_dbContext.Users.AsNoTracking().Any(x => x.Email == email);
    }
}