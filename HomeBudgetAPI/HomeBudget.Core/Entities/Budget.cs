using System.ComponentModel.DataAnnotations;

namespace HomeBudget.Core.Entities
{
    public class Budget : Entity
    {
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal FullAmount { get; set; }

        [Range(1, 12)]
        public byte BugdetMonth { get; set; }
    }
}