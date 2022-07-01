using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Entities.Identity
{
    public class OrderHeader
    {
        [Required]
        public String? Id { get; set; }

        [Required]
        public DateTime? BuyTime { get; set; }

        [Required]
        public String? UserName { get; set; }
        public User? User { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
