using HomeBudget.Service.Actions.BudgetActions.GetAllBudgetsList;
using HomeBudget.Service.ModelsDTO.BudgetModels;
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
        public async Task<List<BudgetDTO>> GetAllBudgetsList()
        {
            return await _mediator.Send(new GetAllBudgetsListQuery());
        }
    }
}
