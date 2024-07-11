using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> inManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> inManager)
        {
            this.userManager = userManager;
            this.inManager = inManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnURL)
        {
            Login login = new Login();
            login.ReturnUrl = returnURL;
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                if(appUser !=null) {

                    await inManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult resutl = 
                        await inManager.PasswordSignInAsync(appUser, login.Password,false,false);

                    if(resutl.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Due to wrong username or password");

            }
            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await inManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
