using Microsoft.AspNetCore.Mvc;
using ParsFile.Application;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.Entities.Content;

namespace ParsFile.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepsitory _categoryRepo;

        public CategoryController(ICategoryRepsitory categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            TempData["action"] = "category";
            return View();
        }

        public IActionResult GetAllCategories()
        {
            return Json(new { data = _categoryRepo.GetAll<Category>() });
        }

        public IActionResult Create()
        {

            TempData["action"] = "category";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {

            TempData["action"] = "category";
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage));

                TempData[SD.WarningMessage] = errors;
                return View(category);
            }

            _categoryRepo.Add(category);

            if (_categoryRepo.Save())
            {
                TempData[SD.SuccessMessage] = "اطلاعات با موفقیت ذخیره شد";
                return Redirect("/Admin/Category/Index");
            }

            TempData[SD.ErrorMessage] = "ذخیره اطلاعات با شکست مواجه شد";
            return Redirect("/Admin/Category/Index");
        }

        public IActionResult Edit(int id)
        {

            TempData["action"] = "category";
            var category = _categoryRepo.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            TempData["action"] = "category";
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage));

                TempData[SD.WarningMessage] = errors;
                return View(category);
            }

            _categoryRepo.Update(category);

            if (_categoryRepo.Save())
            {
                TempData[SD.SuccessMessage] = "اطلاعات با موفقیت بروزرسانی شد";
                return Redirect("/Admin/Category/Index");
            }

            TempData[SD.ErrorMessage] = "بروزرسانی اطلاعات با شکست مواجه شد";
            return Redirect("/Admin/Category/Index");
        }

        public IActionResult Remove(int id)
        {
            _categoryRepo.RemoveById(id);
            if (_categoryRepo.Save())
                return Json(new { success = true, message = "حذف فایل با موفقیت انجام شد" });
            return Json(new { success = false, message = "حذف فایل با شکست مواجه شد" });
        }
    }
}
