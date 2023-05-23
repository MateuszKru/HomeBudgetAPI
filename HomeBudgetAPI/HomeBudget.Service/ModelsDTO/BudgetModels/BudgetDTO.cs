using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Service.ModelsDTO.BudgetModels
{
    public class BudgetDTO
    {
        public decimal FullAmount { get; set; }
        public decimal FixedCharges { get; set; }
        public decimal IrregularExpenses { get; set; }
        public decimal RetirementInvestments { get; set; }
        public decimal Savings { get; set; }
        public decimal EntertainmentFund { get; set; }
        public string BugdetMonth { get; set; } = string.Empty;
    }
}
