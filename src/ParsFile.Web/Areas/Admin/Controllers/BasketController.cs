using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsFile.Application;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.Entities.Content;

namespace ParsFile.Web.Areas.Admin.Controllers
{
	[Authorize]
	public class BasketController : Controller
	{
		private readonly IBasketRepository _basketRepo;

		public BasketController(IBasketRepository basketRepo)
		{
			_basketRepo = basketRepo;
		}

		public IActionResult Index()
		{
			String userName = User.Identity.Name;
			var files = _basketRepo.GetAll<Basket>(
					filter: u => u.UserName == userName,
					include: source => source.Include(u => u.File));

			return View(files);
		}

		[HttpPost]
		public IActionResult Buy(String payment)
		{
			if (payment == "wallet")
			{
				return RedirectToAction("BuyWithWallet", "Order", new { area = "" });
			}
			return RedirectToAction("BuyWithCart", "Order", new { area = "" });
		}

		public async Task<IActionResult> Add(String fileId, String callBack)
		{
			String userName = User.Identity.Name;

			if (_basketRepo.Exists(userName, fileId))
			{
				TempData[SD.WarningMessage] = "فایل مورد نظر در سبد خرید موجود است";

				return Redirect(callBack);
			}

			var basket = new Basket { UserName = userName, FileId = fileId };
			await _basketRepo.AddAsync(basket);

			if (await _basketRepo.SaveAsync())
				TempData[SD.SuccessMessage] = "فایل به سبد خرید شما اضافه شد";
			else
				TempData[SD.ErrorMessage] = "عملیات با شکست مواجه شد";

			return Redirect(callBack);
		}

		public async Task<IActionResult> Remove(String id)
		{
			_basketRepo.RemoveById(id);

			if (!await _basketRepo.SaveAsync())
			{
				TempData[SD.WarningMessage] = "عملیات با مشکل مواجه شد";
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
