using Microsoft.AspNetCore.Identity;
using ParsFile.Domain.Entities.Content;
using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [MaxLength(30)]
        [Display(Name = "نام")]
        public String? FirstName { get; set; } = null;

        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [MaxLength(30)]
        [Display(Name = "نام")]
        public String? LastName { get; set; } = null;

        public bool Deleted { get; set; } = false;



        public Wallet? Wallet { get; set; }
        public ICollection<Basket>? Baskets { get; set; }
        public ICollection<Content.File>? Files { get; set; }
        public ICollection<OrderHeader>? OrderHeaders { get; set; }
    }
}
