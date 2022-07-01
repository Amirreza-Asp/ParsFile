using ParsFile.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParsFile.Domain.Entities.Content
{
    public class Basket
    {
        [Key]
        public String? Id { get; set; }

        public bool Bought { get; set; } = false;

        [Required]
        public String? UserName { get; set; }
        public User? User { get; set; }

        [Required]
        public String? FileId { get; set; }
        [ForeignKey(nameof(FileId))]
        public File? File { get; set; }
    }
}
