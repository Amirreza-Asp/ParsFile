using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Account
{
    public class ResetPasswordDto
    {
        public String UserId { get; set; }

        public String Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    }
}
