using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParsFile.Domain.Entities.Identity
{
    public class OrderDetail
    {
        [Key]
        public String? Id { get; set; }

        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public String? FileId { get; set; }
        [ForeignKey(nameof(FileId))]
        public Content.File? File { get; set; }

        [Required]
        public String? OrderHeaderId { get; set; }
        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader? OrderHeader { get; set; }

    }
}
