using FastKart.DAL.Entities;
using FastKart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastKart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _singInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> singInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _singInManager = singInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null) 
            {
                ModelState.AddModelError("", "Bu adda isdifadeci movcuddur");
                return View();
            }

            var createdUser = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Fullname = model.FullName
            };

            var result = await _userManager.CreateAsync(createdUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(createdUser, RoleConstans.User);
            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
               return View();
            }
            var existUser = await _userManager.FindByNameAsync(model.UserName);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Yalnis melumat daxil etmisiniz");
                return View();
            }

            var result = await _singInManager.PasswordSignInAsync(existUser, model.Password, true, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "You are blocked");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Yalnis melumat daxil etmisiniz");
                return View();
            }

            return RedirectToAction("index", "home");


        }

        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("index", "home");

        }


        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword (ForgetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await _userManager.FindByEmailAsync(model.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("" , "Isdifadeci movcud deil");
                return View();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(existUser);
            var resetLink= Url.Action(nameof(ResetPassword),"Account", new {model.Email, resetToken},Request.Scheme, Request.Host.ToString());

            return View(nameof(EmailView),resetLink);
        }
        public IActionResult EmailView()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string email, string resetToken)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await _userManager.FindByEmailAsync(email);
            if (existUser == null) return BadRequest();
            var result = await _userManager.ResetPasswordAsync(existUser, resetToken, model.Password);
            return RedirectToAction(nameof(Login));
        }




    }
}
