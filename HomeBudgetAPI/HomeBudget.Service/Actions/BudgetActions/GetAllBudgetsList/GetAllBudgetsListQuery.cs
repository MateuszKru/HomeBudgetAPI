using HomeBudget.Service.ModelsDTO.BudgetModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Service.Actions.BudgetActions.GetAllBudgetsList
{
    public class GetAllBudgetsListQuery : IRequest<List<BudgetDTO>>
    {
    }
}
