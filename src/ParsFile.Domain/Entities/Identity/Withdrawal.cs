using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Entities.Identity
{
    public class Withdrawal
    {
        [Key]
        public String? Id { get; set; }

        public int Amount { get; set; }

        public DateTime? RequestTime { get; set; }

        public bool Accept { get; set; }
        public DateTime? AcceptTime { get; set; }

        public String? WalletId { get; set; }
        public Wallet? Wallet { get; set; }
    }
}
