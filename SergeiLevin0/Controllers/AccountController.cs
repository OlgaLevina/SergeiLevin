using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.ViewModels.Idntity;

namespace SergeiLevin0.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());//доступ через get-запрос

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in registration_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        } 


        public IActionResult Login() => View();

    }
}