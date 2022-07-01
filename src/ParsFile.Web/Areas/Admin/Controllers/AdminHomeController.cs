using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Dtos.Order;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly ICategoryRepsitory _categoryRepo;
        private readonly UserManager<User> _userManager;

        public AdminHomeController(IOrderDetailRepository orderDetailRepo, ICategoryRepsitory categoryRepo, UserManager<User> userManager)
        {
            _orderDetailRepo = orderDetailRepo;
            _categoryRepo = categoryRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            TempData["action"] = "dashboard";
            return View();
        }

        [AllowAnonymous]
        public IActionResult ContactUs()
        {
            TempData["action"] = "ticket";
            return View();
        }

        public IActionResult Categories()
        {
            var saleByCategoryDtos = _categoryRepo.GetAll<SaleByCategoryDto>(
                    select: s => new SaleByCategoryDto { CategoryName = s.Name }).ToList();

            var allItems = _orderDetailRepo.GetAll<SaleByCategoryDto>(
                     select: s => new SaleByCategoryDto
                     {
                         CategoryName = s.File.Category.Name,
                         TotalSale = s.Price
                     }
                 ).ToList();

            allItems.ForEach(item =>
            {
                if (saleByCategoryDtos.Any(u => u.CategoryName == item.CategoryName))
                    saleByCategoryDtos.Find(u => u.CategoryName == item.CategoryName).TotalSale += item.TotalSale;
                else
                    saleByCategoryDtos.Add(item);
            });

            return Json(saleByCategoryDtos);
        }

        public IActionResult TodaySales()
        {
            var yesterday = DateTime.Now.AddDays(-1);

            var sales = _orderDetailRepo.GetAll<decimal>(u => u.OrderHeader.BuyTime > yesterday, select: s => s.Price);
            return Json(sales.Sum(u => u));
        }

        public IActionResult UserCounts()
        {
            var usersCount = _userManager.Users.Count();
            return Json(usersCount);
        }

        public IActionResult BestSale()
        {
            var yesterday = DateTime.Now.AddDays(-1);


            var bestOrder = _orderDetailRepo.FirstOrDefault(
                    filter: u => u.OrderHeader.BuyTime > yesterday,
                    orderBy: orderDetail => orderDetail.OrderByDescending(o => o.Price)
                   );

            decimal bestSale = bestOrder == null ? 0 : bestOrder.Price;

            return Json(bestSale);
        }

        public IActionResult Sales()
        {
            var totalSales = _orderDetailRepo.GetAll<Decimal>(
                    select: s => s.Price
                ).Sum(u => u);

            return Json(totalSales);
        }
    }
}
