using HomeBudget.Service.ModelsDTO.UserModels;
using MediatR;

namespace HomeBudget.Service.Actions.UserActions.Login
{
    public class UserLoginCommand : IRequest<AppUserDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}