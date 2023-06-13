using HomeBudget.Service.ModelsDTO.UserModels;
using MediatR;

namespace HomeBudget.Service.Actions.UserActions.Register
{
    public class UserRegisterCommand : IRequest<UserDTO>
    {
        public NewUserDTO NewUser { get; set; }
    }
}