using BookInformationSystem.Models.Domain;
using BookInformationSystem.Repositories.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookInformationSystem.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PublisherController : Controller
    {
        private readonly IPublisherService service;
        public PublisherController(IPublisherService service)
        {
            this.service = service;
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Publisher model)
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
        public IActionResult Update(Publisher model)
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

        public async Task<IActionResult> GetAll(string searchString, int page = 0)
        {

            var publisher = service.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                publisher = publisher.Where(s => s.PublisherName!.Contains(searchString));
            }

            const int PageSize = 3;

            var count = publisher.Count();

            var data = publisher.Skip(page * PageSize).Take(PageSize).OrderBy(s => s.PublisherName).ToList();

            this.ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;
            return View(data);
        }
    }
}
