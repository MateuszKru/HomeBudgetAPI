using AutoMapper;
using HomeBudget.Core.Entities;
using HomeBudget.Service.ModelsDTO.BudgetModels;
using System.Globalization;

namespace HomeBudget.Service.Mapping.BudgetProfile
{
    public class BudgetMappingProfile : Profile
    {
        public BudgetMappingProfile()
        {
            CreateMap<Budget, BudgetDTO>()
                .ForMember(m => m.FixedCharges, c => c.MapFrom(s => s.FullAmount * 0.6m))
                .ForMember(m => m.IrregularExpenses, c => c.MapFrom(s => s.FullAmount * 0.1m))
                .ForMember(m => m.RetirementInvestments, c => c.MapFrom(s => s.FullAmount * 0.1m))
                .ForMember(m => m.EntertainmentFund, c => c.MapFrom(s => s.FullAmount * 0.1m))
                .ForMember(m => m.Savings, c => c.MapFrom(s => s.FullAmount * 0.1m))
                .ForMember(m => m.BugdetMonth, c => c.MapFrom(s => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.BugdetMonth)));
        }
    }
}