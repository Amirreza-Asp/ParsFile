using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParsFile.Application;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.Dtos.Content.File;
using ParsFile.Infrastructure.Helpers;

namespace ParsFile.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileRepository _fileRepo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IMapper _mapper;
        private readonly ICategoryRepsitory _catRepo;

        public FileController(IFileRepository fileRepo, IWebHostEnvironment hostEnv, IMapper mapper, ICategoryRepsitory catRepo)
        {
            _fileRepo = fileRepo;
            _hostEnv = hostEnv;
            _mapper = mapper;
            _catRepo = catRepo;
        }

        public IActionResult Index()
        {
            String username = User.Identity.Name;

            var files = _fileRepo.GetAll<FileDto>(
                    filter: file => file.UserName == username && file.Deleted == false,
                    orderBy: order => order.OrderByDescending(x => x.CreateTime),
                    select: file => _mapper.Map<FileDto>(file));

            TempData["action"] = "manage-file";
            return View(files);
        }

        public IActionResult Add()
        {
            TempData["action"] = "manage-file";
            var categories = _catRepo.GetAll<SelectListItem>(
                    select: s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            var addFileDto = new AddFileDto { Categories = categories };

            return View(addFileDto);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddFileDto addFile)
        {
            TempData["action"] = "manage-file";

            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                TempData[SD.ErrorMessage] = errors;

                addFile.Categories = _catRepo.GetAll<SelectListItem>(
                    select: s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

                return View(addFile);
            }

            var files = HttpContext.Request.Form.Files;
            if (files.Count() != 2)
            {
                addFile.Categories = _catRepo.GetAll<SelectListItem>(
                    select: s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

                TempData[SD.ErrorMessage] = "لطفا اطلاعات را کامل کنید";
                return View(addFile);
            }

            var file = _mapper.Map<Domain.Entities.Content.File>(addFile);

            file.Path = SaveFile(files[0], SD.UserFilesPath);
            file.Image = SaveFile(files[1], SD.FileImagesPath);

            file.UserName = User.Identity.Name;

            await _fileRepo.AddAsync(file);
            if (await _fileRepo.SaveAsync())
            {
                TempData[SD.SuccessMessage] = "فایل با موفقیت ذخیره شد";
                return Redirect("/Admin/File");
            }

            addFile.Categories = _catRepo.GetAll<SelectListItem>(
                    select: s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            TempData[SD.ErrorMessage] = "ذخیره فایل با مشکلی مواجه شد";
            return View(file);
        }

        public IActionResult Remove(String id)
        {
            var file = _fileRepo.Find(id);
            if (file.Accepted)
            {
                file.Deleted = true;
                _fileRepo.Update(file);
            }
            else
            {
                _fileRepo.Remove(file);
            }


            if (_fileRepo.Save())
                TempData[SD.SuccessMessage] = "فایل با موفقیت حذف شد";
            else
                TempData[SD.ErrorMessage] = "حذف فایل با شکست مواجه شد";

            return Redirect("/Admin/File/index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            TempData["action"] = "files";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAllFiles()
        {
            var files = _fileRepo.GetAll<ShowFilesDto>(
                select: s => new ShowFilesDto
                {
                    Name = s.Name,
                    Accepted = s.Accepted,
                    Downloads = s.Downloads,
                    Id = s.Id,
                    Price = s.Price,
                    CreateTime = s.CreateTime.ToShamsi(),
                    Username = s.UserName,
                    Deleted = s.Deleted
                }); ;

            return Json(new { data = files });
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Accept(String id)
        {
            var file = _fileRepo.Find(id);

            if (file == null)
            {
                TempData[SD.WarningMessage] = "فایل مورد نظر وجود ندارد";
                return Redirect("/Admin/File/Manage");
            }

            file.Accepted = !file.Accepted;
            _fileRepo.Update(file);
            _fileRepo.Save();

            return Redirect("/Admin/File/Manage");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AdminRemove(String id)
        {
            var file = _fileRepo.Find(id);

            if (file == null)
            {
                return NotFound();
            }

            RemoveFile(SD.FileImagesPath, file.Image);
            RemoveFile(SD.UserFilesPath, file.Path);

            _fileRepo.RemoveById(id);
            if (_fileRepo.Save())
            {
                return Json(new { success = true, message = "حذف با موفقیت انجام شد" });
            }
            return Json(new { success = false, message = "حذف با شکست مواجه شد" });
        }

        public IActionResult Show(String id)
        {
            var file = _fileRepo.Find(id);
            if (file == null) return NotFound();

            return View(file);
        }

        public IActionResult FilesList(String filter)
        {
            var files = _fileRepo.GetAll<Domain.Entities.Content.File>(
                    u => String.IsNullOrEmpty(filter) || u.Name.Contains(filter) && u.Accepted,
                    include: source => source.Include(u => u.Category),
                    orderBy: order => order.OrderByDescending(x => x.CreateTime)).Take(10);

            return View(files);
        }

        public IActionResult SearchFilter(String filter)
        {
            var files = _fileRepo.GetAll<FileSearchDto>(
                    u => u.Name.Contains(filter) && u.Accepted,
                    orderBy: order => order.OrderByDescending(x => x.CreateTime),
                    select: file => _mapper.Map<FileSearchDto>(file));

            return Json(new { data = files });
        }



        private String SaveFile(IFormFile file, String directoryPath)
        {
            String upload = _hostEnv.WebRootPath + directoryPath;

            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }

            String extension = Path.GetExtension(file.FileName);
            String fileName = Guid.NewGuid().ToString();

            // create file in specified directory
            using (var fileStram = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStram);
            }

            return fileName + extension;
        }

        private void RemoveFile(String directoryPath, String fileName)
        {
            String upload = _hostEnv.WebRootPath + directoryPath;
            if (System.IO.File.Exists(upload + fileName))
            {
                System.IO.File.Delete(upload + fileName);
            }
        }
    }
}
