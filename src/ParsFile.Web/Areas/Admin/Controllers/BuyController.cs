using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsFile.Application.Contracts.Repositories.Identity;

namespace ParsFile.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class BuyController : Controller
    {
        private readonly IOrderDetailRepository _orderDetailRepo;

        public BuyController(IOrderDetailRepository orderDetailRepo)
        {
            _orderDetailRepo = orderDetailRepo;
        }

        public IActionResult Index()
        {
            String userName = User.Identity.Name;
            var buyes = _orderDetailRepo.GetAll<Domain.Entities.Content.File>(
                filter: u => u.OrderHeader.UserName == userName,
                include: source => source.Include(u => u.File),
                select: s => s.File);

            TempData["action"] = "buy";
            return View(buyes);
        }
    }
}
