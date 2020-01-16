using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.ViewModels.Idntity;

namespace SergeiLevin0.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly ILogger<AccountController> Loger;//отслеживание всего, что происходит на сайте

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> loger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Loger = loger;
        }

        public async Task<IActionResult> IsNameFree(string UserName)
        {
            var user = await UserManager.FindByNameAsync(UserName);
            if (user is null) return Json("true");
            return Json("Пользователь с таким именем уже существует!");
        }

        public IActionResult Register() => View(new RegisterUserViewModel());//доступ через get-запрос

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            Loger.LogWarning($"Пользователь {Model.UserName} пытается зарегистрироваться");
            if (!ModelState.IsValid) return View(Model);
            var user = new User
            {
                UserName = Model.UserName
            };
            Loger.LogInformation($"Регистрация нового пользователя {Model.UserName}");
            var registration_result = await UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user, Role.User);//наделение нового пользователя правами пользователя
                Loger.LogInformation($"Пользователь {Model.UserName} успешно зарегистрирован");
                await SignInManager.SignInAsync(user, false);
                Loger.LogInformation($"Пользователь {Model.UserName} вошел в систему");
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);
            Loger.LogWarning($"Ошибка при регистрации нового пользователя {Model.UserName}: {string.Join(' ', registration_result.Errors.Select(e => e.Description))} ");
            return View(Model);
        } 


        public IActionResult Login(string returnUrl) => View(new LoginViewModel {ReturnUrl=returnUrl} );
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            Loger.LogWarning($"Пользователь {Model.UserName} пытается войти");
            if (!ModelState.IsValid) return View(Model);

            var login_result = await SignInManager.PasswordSignInAsync(
                Model.UserName, 
                Model.Password,
                Model.RememberMe,//https://stackoverflow.com/questions/2452656/asp-net-mvc-rememberme
                false);//блокировка в случае максимального кол-ва ошибок - пока отключена, потом исправить на true!!!!
            if (login_result.Succeeded)
            {
                Loger.LogInformation($"Пользователь {Model.UserName} вошел в систему");
                if (Url.IsLocalUrl(Model.ReturnUrl)) return Redirect(Model.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "неверные имя или паролль");
            Loger.LogWarning($"Ошибка при входе пользователя {Model.UserName}");
            return View(Model);
        }

        [HttpPost, ValidateAntiForgeryToken]//т.к. у нас это httppost, то соответствующий пунтк меню нужно переделать в форму!
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            Loger.LogInformation($"Пользователь {User.Identity.Name} вышел в систему");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenieded() => View();
    }
}