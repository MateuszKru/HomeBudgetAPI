using HomeBudget.Service.ModelsDTO.BudgetModels;
using MediatR;

namespace HomeBudget.Service.Actions.BudgetActions.GetAllBudgetsList
{
    public class GetAllBudgetsListQuery : IRequest<List<BudgetDTO>>
    {
    }
}