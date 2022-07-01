using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParsFile.Application;
using ParsFile.Application.Contracts.Services;
using ParsFile.Domain.Dtos.Account;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public IActionResult Manage()
        {
            var user = _userManager.FindByNameAsync(User.Identity?.Name).Result;

            var userInfo = new AccountInfoDto()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = $"{user.FirstName} {user.LastName}",
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            TempData["action"] = "profile";
            return View(userInfo);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Index", "Home");
        }

        #region Register
        public IActionResult Register()
        {
            return View("Login-Register");
        }

        [HttpPost]
        public IActionResult Register(LoginRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                TempData[SD.ErrorMessage] = "لطفا اطلاعات خود را کامل کنید";
                return View(registerDto);
            }

            var user = _mapper.Map<User>(registerDto);
            user.UserName = registerDto.Email;

            var result = _userManager.CreateAsync(user, registerDto.Password).GetAwaiter().GetResult(); ;
            if (result.Succeeded)
            {
                return RedirectToAction("Login", registerDto);
            }

            String errors = "";
            foreach (var item in result.Errors)
            {
                errors += item.Description + "\n";
            }
            TempData[SD.ErrorMessage] = errors;
            return View("Login-Register", registerDto);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View("Login-Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginRegisterDto loginDto)
        {
            if (String.IsNullOrEmpty(loginDto.Password) || String.IsNullOrEmpty(loginDto.Email))
            {
                TempData[SD.ErrorMessage] = "لطفا اطلاعات را کامل کنید";
                return View("Login-Register", loginDto);
            }

            var user = _userManager.FindByNameAsync(loginDto.Email).Result;
            if (user == null || user.Deleted)
            {
                TempData[SD.WarningMessage] = $"هیچ کاربری با نام کاربری {loginDto.Email} وجود ندارد";
                return RedirectToAction("Register");
            }

            var result = _signInManager.PasswordSignInAsync(user, loginDto.Password, true, true).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.RequiresTwoFactor)
            {
                TempData[SD.InfoMessage] = "در حال ارسال ایمیل";
                return RedirectToAction("TwoFactorLogin", new { loginDto.Email, isPersistent = true });
            }
            if (result.IsLockedOut)
            {
                TempData[SD.ErrorMessage] = $"کاربر {loginDto.Email} قفل شده است";
            }

            return View("Login-Register");
        }
        #endregion

        #region TwoFactorLogin
        public IActionResult TwoFactorEnabled()
        {
            var user = _userManager.FindByNameAsync(User.Identity?.Name).Result;
            var result = _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled).Result;
            if (result.Succeeded)
            {
                if (!user.TwoFactorEnabled)
                {
                    TempData[SD.WarningMessage] = "ورود دو مرحله ای برای شما غیر فعال شد";
                }
                else
                {
                    TempData[SD.SuccessMessage] = "ورود دو مرحله ای برای شما فعال شد";
                }

                return RedirectToAction("Manage");
            }
            return BadRequest();
        }

        public IActionResult TwoFactorLogin(String userName, bool isPersistant)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            if (user == null)
            {
                return BadRequest();
            }

            var providers = _userManager.GetValidTwoFactorProvidersAsync(user).Result;
            var twoFactorLogin = new TwoFactorLoginDto();
            twoFactorLogin.IsPersistent = isPersistant;

            if (providers.Contains("Email"))
            {
                String emailCode = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;

                String body = $"کد ورود دو مرحله ای {emailCode}";
                String subject = "ورود دو مرحله ای";
                _emailService.Execute(user.Email, body, subject);

                twoFactorLogin.Provider = "Email";
            }
            return View(twoFactorLogin);
        }

        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorLoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = _signInManager.TwoFactorSignInAsync(login.Provider, login.Code, login.IsPersistent, true).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData[SD.ErrorMessage] = "کد وارد شده اشتباه است";
            login.Code = "";
            return View(login);
        }
        #endregion

        #region ForgetPssword
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordDto forgetDto)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetDto);
            }

            var user = _userManager.FindByEmailAsync(forgetDto.Email).Result;
            if (user == null)
            {
                return NotFound();
            }

            String token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            String? callBackUrl = Url.Action("ResetPassword", "Account",
            new
            {
                UserId = user.Id,
                Token = token
            }, protocol: Request.Scheme);

            String body = $"برای بازیابی رمز عبور بر روی لینک زیر کلیک کنید <br/><a href ={callBackUrl}>Reset Link</a>";
            String subject = "بازیابی رمز عبور";
            _emailService.Execute(user.Email, body, subject);
            TempData[SD.InfoMessage] = "لینک بازیابی رمز عبور برای شما ارسال شد";
            return View();
        }

        public IActionResult ResetPassword(String userId, String token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }

            var resetPassword = new ResetPasswordDto()
            {
                Token = token,
                UserId = userId
            };
            return View(resetPassword);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var user = _userManager.FindByIdAsync(reset.UserId).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result = _userManager.ResetPasswordAsync(user, reset.Token, reset.Password).Result;
            if (result.Succeeded)
            {
                TempData[SD.SuccessMessage] = "عملیات با موفقیت انجام شد";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            TempData[SD.ErrorMessage] = "عملیات با شکست مواجه شد";
            return View(reset);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
    }
}
