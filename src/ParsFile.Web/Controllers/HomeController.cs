using Microsoft.AspNetCore.Mvc;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.ViewModels;
using System.Diagnostics;

namespace ParsFile.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileRepository _fileRepo;

        public HomeController(ILogger<HomeController> logger, IFileRepository fileRepo)
        {
            _logger = logger;
            _fileRepo = fileRepo;
        }

        public IActionResult Index()
        {
            var files = _fileRepo.GetAll<Domain.Entities.Content.File>().Take(10);
            return View(files);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}