using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsFile.Application;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Content;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Domain.ViewModels;
using ZarinpalSandbox;

namespace ParsFile.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IWalletRepository _walletRepo;

        public OrderController(IBasketRepository basketRepo, IOrderHeaderRepository orderHeaderRepo,
            IOrderDetailRepository orderDetailRepo, IWalletRepository walletRepo)
        {
            _basketRepo = basketRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailRepo = orderDetailRepo;
            _walletRepo = walletRepo;
        }

        public IActionResult Index()
        {
            String username = User.Identity.Name;
            var orders = _orderDetailRepo.GetAll<OrderDetail>(
                u => u.File.UserName == username,
                include: source => source.Include(u => u.File)).ToList();

            OrdersListVM ordersListVM = new OrdersListVM();

            orders.ForEach(order =>
            {
                if (ordersListVM.Files.Any(u => u.Id == order.FileId))
                    ordersListVM.Files.
                        First(u => u.Id == order.FileId).SaleCount += 1;
                else
                    ordersListVM.Files.Add(new OrdersListVM.File
                    {
                        Id = order.File.Id,
                        Name = order.File.Name,
                        CreateTime = order.File.CreateTime,
                        Image = order.File.Image,
                        SaleCount = 1,
                        Price = order.File.Price
                    });

                ordersListVM.TotalPrice += (double)order.Price;
            });

            TempData["action"] = "sale";
            return View(ordersListVM);
        }

        public IActionResult BuyWithCart()
        {
            String userName = User.Identity.Name;
            var prices = _basketRepo.GetAll<double>(u => u.UserName == userName, select: s => s.File.Price);

            var payment = new Payment((int)prices.Sum(u => u));
            var result = payment.PaymentRequest(
                description: $"پرداخت فاکتور {userName}",
                callbackUrl: "https://localhost:7146/Order/Payment",
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
                String username = User.Identity.Name;

                String authority = HttpContext.Request.Query["Authority"].ToString();

                AddOrder(username);
                return RedirectToAction("index");
            }
            return NotFound();
        }

        public IActionResult BuyWithWallet()
        {
            String userName = User.Identity.Name;

            var prices = _basketRepo.GetAll<double>(u => u.UserName == userName, select: s => s.File.Price);

            if (!_walletRepo.Exists(userName))
            {
                AddWallet(userName);
                _walletRepo.Save();
            }

            var wallet = _walletRepo.FirstOrDefault(u => u.UserName == userName);
            if (wallet.Cash < (decimal)prices.Sum(u => u))
            {
                TempData[SD.ErrorMessage] = "موجودی شما برای انجام عملیات کافی نیست";
                return Redirect("/Admin/Basket/Index");
            }

            _walletRepo.UpdateCash(userName, -1 * (double)prices.Sum(u => u));
            AddOrder(userName);

            return RedirectToAction("Index");
        }

        private Boolean AddOrder(String username)
        {
            // create new order header
            var orderHeader = new OrderHeader
            {
                Id = Guid.NewGuid().ToString(),
                BuyTime = DateTime.Now,
                UserName = username
            };
            _orderHeaderRepo.AddAsync(orderHeader);

            // get all baskets
            var baskets = _basketRepo.GetAll<Basket>(
                            filter: u => u.UserName == username,
                            include: source => source.Include(u => u.File),
                            isTracking: true);

            // create list of orderDetails and add to db
            var orderDetails = baskets.Select(
                    basket => new OrderDetail
                    {
                        FileId = basket.FileId,
                        OrderHeaderId = orderHeader.Id,
                        Price = Convert.ToDecimal(basket.File.Price * 0.7)
                    });
            _orderDetailRepo.AddMany(orderDetails);

            baskets.ToList().ForEach(basket => _basketRepo.Remove(basket));
            SetMoneyForPublishers(baskets);

            return _orderDetailRepo.Save();
        }

        private void SetMoneyForPublishers(IEnumerable<Basket> baskets)
        {
            foreach (var basket in baskets)
            {
                if (!_walletRepo.Exists(basket.File.UserName))
                {
                    AddWallet(basket.File.UserName);
                }

                _walletRepo.UpdateCash(basket.File.UserName, basket.File.Price * 0.7);
            }
        }

        private void AddWallet(String userName)
        {
            var wallet = new Wallet { Cash = 0, UserName = userName };
            _walletRepo.Add(wallet);
        }
    }
}
