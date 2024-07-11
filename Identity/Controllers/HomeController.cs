using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> _userManager;
        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
    
        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.GetUserAsync(HttpContext.User);
            string message = "Welcome " + appUser.UserName;
            return View((Object)message);
        }
    }
}
