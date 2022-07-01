using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Entities.Identity
{
    public class Wallet
    {
        [Key]
        public String? Id { get; set; } = null;

        [Range(0, double.MaxValue)]
        [Display(Name = "موجودی")]
        public Decimal Cash { get; set; }

        public String? UserName { get; set; } = null;
        public User? User { get; set; } = null;

        public ICollection<Withdrawal>? Withdrawals { get; set; }
    }
}
