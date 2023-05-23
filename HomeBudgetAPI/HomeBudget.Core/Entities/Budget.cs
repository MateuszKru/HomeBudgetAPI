using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Core.Entities
{
    public class Budget
    {
        public Guid Id { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal FullAmount { get; set; }

        [Range(1, 12)]
        public byte BugdetMonth { get; set; }
    }
}
