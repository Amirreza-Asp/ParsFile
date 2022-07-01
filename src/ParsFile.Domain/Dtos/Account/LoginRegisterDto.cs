using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Account
{
    public class LoginRegisterDto
    {
        [Required]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        public string PasswordConfirmed { get; set; }
    }
}
