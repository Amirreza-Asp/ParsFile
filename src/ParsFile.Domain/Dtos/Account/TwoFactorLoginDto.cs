using System.ComponentModel.DataAnnotations;

namespace ParsFile.Domain.Dtos.Account
{
    public class TwoFactorLoginDto
    {
        [Required]
        public String Provider { get; set; }

        [Required]
        public String Code { get; set; }

        public bool IsPersistent { get; set; }
    }
}
