using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;

        public AdminController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email

                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                return View(appUser);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string username, string email, string passwrod)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser != null)
            {
                if (!string.IsNullOrEmpty(username))
                    appUser.UserName = username;
                else
                    ModelState.AddModelError("", "Username field can't be empty");

                if (!string.IsNullOrEmpty(email))
                    appUser.Email = email;
                else
                    ModelState.AddModelError("", "Email field can't be empty");

                if (!string.IsNullOrEmpty(passwrod))
                    appUser.PasswordHash = _passwordHasher.HashPassword(appUser, passwrod);
                else
                    ModelState.AddModelError("", "Password field can't be empty");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(passwrod))
                {
                    IdentityResult result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        Error(result);
                }

            }
            else
                ModelState.AddModelError("", "Can't find the user in the database");
            return View(appUser);
        }

        private void Error(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else { Error(result); }
            }
            else
                ModelState.AddModelError("", "User does not exist");
            return View("Index", _userManager.Users);
        }
    }
}
