using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsFile.Application;
using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Helpers;
using ZarinpalSandbox;

namespace ParsFile.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IWalletRepository _walletRepo;
        public readonly IWithdrawalRepository _withdrawalRepo;

        public WalletController(IWalletRepository walletRepo, IWithdrawalRepository withdrawalRepo)
        {
            _walletRepo = walletRepo;
            _withdrawalRepo = withdrawalRepo;
        }

        public IActionResult Index()
        {
            String userName = User.Identity.Name;

            if (!_walletRepo.Exists(userName))
            {
                _walletRepo.Add(new Wallet { UserName = userName });
                _walletRepo.Save();
            }

            var wallet = _walletRepo.FirstOrDefault(u => u.UserName == userName);

            return View(wallet);
        }

        [HttpPost]
        public IActionResult IncreaseCash(int amount)
        {
            String userName = User.Identity.Name;
            HttpContext.Session.Set("cash", amount);

            var payment = new Payment(amount);
            var result = payment.PaymentRequest(
                description: $"پرداخت فاکتور {userName}",
                callbackUrl: "https://localhost:7146/Wallet/Payment",
                email: userName).Result;

            if (result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Payment()
        {
            if (HttpContext.Request.Query["status"] != ""
                && HttpContext.Request.Query["status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                String userName = User.Identity.Name;

                String authority = HttpContext.Request.Query["Authority"].ToString();
                var cash = HttpContext.Session.Get<int>("cash");


                _walletRepo.UpdateCash(userName, cash);
                _walletRepo.Save();

                return Redirect("/Admin/Wallet/Index");
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult SendWithdrawalRequest(int amount)
        {
            String userName = User.Identity.Name;
            var wallet = _walletRepo.FirstOrDefault(u => u.UserName == userName);

            if (wallet.Cash < amount)
            {
                TempData[SD.ErrorMessage] = "موجودی شما کمتر از مقدار درخواستی است";
                return Redirect("/Admin/Wallet/Index");
            }

            var withdrawal = new Withdrawal { Amount = amount, RequestTime = DateTime.Now, WalletId = wallet.Id };
            _withdrawalRepo.Add(withdrawal);
            if (_withdrawalRepo.Save())
                TempData[SD.SuccessMessage] = "درخواست شما با موفقیت ارسال شد";
            else
                TempData[SD.ErrorMessage] = "ارسال درخواست با شکست مواجه شد";

            return Redirect("/Admin/Wallet/Index");
        }

        public IActionResult Withdrawals()
        {
            TempData["action"] = "withdrawal";

            if (User.IsInRole("Admin"))
            {
                var withdrawals = _withdrawalRepo.GetAll<Withdrawal>(
                        orderBy: withdrawal => withdrawal
                                    .OrderByDescending(s => s.AcceptTime == null)
                                    .OrderByDescending(o => o.RequestTime),
                        include: source =>
                                        source.Include(u => u.Wallet)
                                                        .ThenInclude(u => u.User));

                return View(withdrawals);
            }
            else
            {
                String userName = User.Identity.Name;

                var withdrawals = _withdrawalRepo.GetAll<Withdrawal>(
                        filter: u => u.Wallet.UserName == userName,
                        orderBy: o => o.OrderByDescending(u => u.RequestTime));

                return View(withdrawals);
            }
        }

        public IActionResult SubmitWithdrawal(String id, Boolean response)
        {
            var withdrawal = _withdrawalRepo.Find(id);
            withdrawal.Accept = response;
            withdrawal.AcceptTime = DateTime.Now;
            _withdrawalRepo.Update(withdrawal);
            _withdrawalRepo.Save();
            return RedirectToAction(nameof(Withdrawals));
        }
    }

}
