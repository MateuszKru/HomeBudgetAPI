using AutoMapper;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.Exceptions;
using HomeBudget.Service.ModelsDTO.UserModels;
using HomeBudget.Service.Services.UserServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.Actions.UserActions.Login
{
    public class UserLoginHandler : IRequestHandler<UserLoginCommand, AppUserDTO>
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserLoginHandler(
            HomeBudgetDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper,
            IUserService userService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<AppUserDTO> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
                throw new RestException(StatusCodeEnum.BadRequest, "Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new RestException(StatusCodeEnum.BadRequest, "Invalid username or password");

            AppUserDTO appUser = _mapper.Map<AppUserDTO>(user);

            appUser.Token = _userService.GenerateTokenJWT(user);

            return appUser;
        }
    }
}