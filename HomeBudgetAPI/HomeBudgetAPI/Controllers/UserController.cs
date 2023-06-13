using HomeBudget.Service.Actions.UserActions.Login;
using HomeBudget.Service.Actions.UserActions.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}