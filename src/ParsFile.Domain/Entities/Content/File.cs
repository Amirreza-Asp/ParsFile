using ParsFile.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParsFile.Domain.Entities.Content
{
    public class File
    {
        [Key]
        public String? Id { get; set; }

        [Required]
        [Display(Name = "نام")]
        [MaxLength(50)]
        public String? Name { get; set; }

        [Display(Name = "فایل")]
        [Required]
        public String? Path { get; set; }

        [Required]
        [Display(Name = "عکس")]
        public String? Image { get; set; }

        [Display(Name = "توضیحات")]
        public String? Description { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime CreateTime { get; set; }

        [Range(0, Double.MaxValue)]
        [Display(Name = "قیمت")]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Downloads { get; set; }

        [Display(Name = "افزودن")]
        public bool Accepted { get; set; } = false;

        public bool Deleted { get; set; } = false;

        public String? UserName { get; set; }
        public User? User { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
