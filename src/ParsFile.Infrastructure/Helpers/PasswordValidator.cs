using Microsoft.AspNetCore.Identity;
using ParsFile.Application;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Infrastructure.Helpers
{
    public class PasswordValidator : IPasswordValidator<User>
    {

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            String[] commonPasswords = File.ReadAllLines(SD.FileCommonPasswordPath);
            // C:\Users\TOP SYSTEM\source\repos\ParsFile\src\ParsFile.Web\wwwroot\file\txt\commonPassword.txt

            if (commonPasswords.Contains(password))
            {
                var result = IdentityResult.Failed(new IdentityError()
                {
                    Code = "CommonPassword",
                    Description = "رمز عبور ضعیف است"
                });
                return Task.FromResult(result);
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
