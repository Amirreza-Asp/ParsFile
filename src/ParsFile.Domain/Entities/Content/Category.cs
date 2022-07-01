using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Entities.Content
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String? Name { get; set; }
    }
}
