using FluentValidation;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.Actions.UserActions.Login;
using HomeBudget.Service.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.Validators.UserValidators.cs
{
    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        private readonly IUserService _userService;
        private readonly HomeBudgetDbContext _dbContext;

        public UserLoginCommandValidator(IUserService userService, HomeBudgetDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithName(x => $"{nameof(x.Email)}");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName(x => $"{nameof(x.Password)}");

            RuleFor(x => x)
                .Must(IsCorrectCredentials)
                .WithName(x => "Credentials")
                .WithMessage("Niepoprawny email lub hasło");
        }

        private bool IsCorrectCredentials(UserLoginCommand userLogin)
        {
            User user = _dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefault(x => x.Email == userLogin.Email);

            if (user == null)
                return false;

            var result = _userService.VerifyHashedPassword(user, userLogin.Password);

            if (result == PasswordVerificationResult.Failed)
                return false;

            return true;
        }
    }
}