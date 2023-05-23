using AutoMapper;
using HomeBudget.Core;
using HomeBudget.Service.ModelsDTO.BudgetModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Service.Actions.BudgetActions.GetAllBudgetsList
{
    public class GetAllBudgetsListHandler : IRequestHandler<GetAllBudgetsListQuery, List<BudgetDTO>>
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllBudgetsListHandler(HomeBudgetDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<BudgetDTO>> Handle(GetAllBudgetsListQuery request, CancellationToken cancellationToken)
        {
            var budgetList = await _dbContext.Budgets.Select(x => _mapper.Map<BudgetDTO>(x)).ToListAsync(cancellationToken: cancellationToken);
            return budgetList;
        }
    }
}
