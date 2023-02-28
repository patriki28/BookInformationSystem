using BookInformationSystem.Models.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookInformationSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin model)
        {
            if (ModelState.IsValid)
            {
                var User = from m in _context.Admin select m;
                User = User.Where(s => s.Username.Contains(model.Username));
                if (User.Count() != 0)
                {
                    if (User.First().Password == model.Password)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,model.Username),
                            new Claim("OtherProperties", "ExampleRole") 
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                        ClaimsPrincipal claimUser = HttpContext.User;
                        if(claimUser.Identity.IsAuthenticated)
                            return RedirectToAction("GetAll", "Book");
                    }  
                }
            } 

            
            return View();
        }
    }
}
