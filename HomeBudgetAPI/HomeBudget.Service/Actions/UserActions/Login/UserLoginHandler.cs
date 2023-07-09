using AutoMapper;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.ModelsDTO.UserModels;
using HomeBudget.Service.Services.UserServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Service.Actions.UserActions.Login
{
    public class UserLoginHandler : IRequestHandler<UserLoginCommand, AppUserDTO>
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserLoginHandler(
            HomeBudgetDbContext dbContext,
            IMapper mapper,
            IUserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<AppUserDTO> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            AppUserDTO appUser = _mapper.Map<AppUserDTO>(user);

            appUser.Token = _userService.GenerateTokenJWT(user);

            return appUser;
        }
    }
}