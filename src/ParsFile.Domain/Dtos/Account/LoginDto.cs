using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public String UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public bool isPersistent { get; set; } = false;
    }
}
