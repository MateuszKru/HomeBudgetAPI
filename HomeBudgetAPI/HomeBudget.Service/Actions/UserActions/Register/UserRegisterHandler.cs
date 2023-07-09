using AutoMapper;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using HomeBudget.Service.ModelsDTO.UserModels;
using HomeBudget.Service.Services.UserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeBudget.Service.Actions.UserActions.Register
{
    public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, UserDTO>
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRegisterHandler> _logger;
        private readonly IUserService _userService;

        public UserRegisterHandler(
            HomeBudgetDbContext dbContext,
            IMapper mapper,
            ILogger<UserRegisterHandler> logger,
            IUserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<UserDTO> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Request register invoked for email: {request.NewUser.Email}");

            User newUser = _mapper.Map<User>(request.NewUser);
            SetPasswordHash(newUser, request.NewUser.Password);
            SetUserRole(newUser, UserRoleEnum.User);

            await _dbContext.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            UserDTO appUser = _mapper.Map<UserDTO>(newUser);

            _logger.LogInformation($"New user with email {newUser.Email} was added to database");

            return appUser;
        }

        private void SetPasswordHash(User user, string password)
            => user.PasswordHash = _userService.HashPassword(user, password);

        public void SetUserRole(User user, UserRoleEnum userRole)
            => user.Role = _userService.GetRole(userRole);
    }
}