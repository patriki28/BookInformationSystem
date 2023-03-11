using BookInformationSystem.Models.Domain;
using BookInformationSystem.Repositories.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;


namespace BookInformationSystem.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AuthorController : Controller
    {
        private readonly IAuthorService service;
        public AuthorController(IAuthorService service)
        {
            this.service = service;
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Author model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = service.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }


        public IActionResult Update(int id)
        {
            var record = service.FindById(id);
            return View(record);
        }

        [HttpPost]
        public IActionResult Update(Author model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = service.Update(model);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }


        public IActionResult Delete(int id)
        {

            var result = service.Delete(id);
            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> GetAll( string searchString, int page = 0)
        {
            var author = service.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                author = author.Where(s => s.AuthorName!.Contains(searchString));
            }

            const int PageSize = 3;

            var count = author.Count();

            var data = author.Skip(page * PageSize).Take(PageSize).OrderBy(s => s.AuthorName).ToList();

            this.ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;


            return View(data);
        }
    }
}
