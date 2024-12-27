using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Models;
using Project.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager; 

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public IActionResult Login()
        {
            ViewData["ActivePage"] = "Login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(login.Email);

                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("HomePage", "Items");
                }
                else
                {
                    ModelState.AddModelError("", "That was an invalid email or password. Try again! ");
                    ViewData["ActivePage"] = "Login";
                    return View(login);
                }
            }
            ViewData["ActivePage"] = "Login";
            return View(login);
        }

        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Register";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if(ModelState.IsValid)
            {
                Users users = new Users
                {
                    FullName = register.Name,
                    Email = register.Email,
                    UserName = register.Email,

                };
                var result = await userManager.CreateAsync(users, register.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ViewData["ActivePage"] = "Register";
                    return View(register);
                }
            }
            ViewData["ActivePage"] = "Register";
            return View(register);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("HomePage", "Items");
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePass)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(changePass.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, changePass.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(changePass);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email Not Found!");
                    return View(changePass);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong! try again.");
                return View(changePass);
            }
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel verify)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(verify.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Your email doesn't found");
                    return View(verify);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new {username = user.UserName});
                }
            }
            else
            {
                ModelState.AddModelError("", "smeothing went error");
                return View(verify);
            }
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

    }
}
