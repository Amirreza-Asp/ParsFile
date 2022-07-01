using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Account
{
    public class ForgetPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }
}
