using HomeBudget.Service.Actions.BudgetActions.GetAllBudgetsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllBudgetsList")]
        public async Task<ActionResult> GetAllBudgetsList()
        {
            var response = await _mediator.Send(new GetAllBudgetsListQuery());
            return Ok(response);
        }
    }
}