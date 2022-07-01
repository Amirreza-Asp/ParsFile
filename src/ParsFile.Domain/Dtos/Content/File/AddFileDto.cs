using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Content.File
{
    public class AddFileDto
    {
        [Required]
        [Display(Name = "نام")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "قیمت")]
        public double Price { get; set; }

        [Display(Name = "فایل")]
        public string? Path { get; set; } = null;

        [Display(Name = "عکس")]
        public string? Image { get; set; } = null;

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
