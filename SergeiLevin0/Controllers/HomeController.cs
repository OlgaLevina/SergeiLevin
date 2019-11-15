using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SergeiLevin0.ViewModels;

namespace SergeiLevin0.Controllers
{
    //[Controller]
    public class HomeController : Controller //если не наследовать от класса Controller, то нужно подписать атрибутом Controller.
    {
        //private readonly IConfiguration _Configuration;

        //public HomeController(IConfiguration configuration)
        //{
        //    _Configuration = configuration;
        //}

        public HomeController() { }

        public IActionResult Index()
        {
            return View();//вызовет предоставление из папки Home с название Index. Если хотим вызвать иное представление: return View("OtherView"); - которое тоже будет искаться в папке Хоум
        }
        public IActionResult ContactUs() { return View(); }
        public IActionResult Login() { return View(); }
        public IActionResult ProductDetails() { return View(); }
        public IActionResult Shop() { return View(); }
        public IActionResult _404() { return View(); }
        public IActionResult BlogSingle() { return View(); }
        public IActionResult Blog() { return View(); }
        public IActionResult Cart() { return View(); }
        public IActionResult Checkout() { return View(); }

        //public IActionResult ReadConfig()
        //{
        //    //return Content("Hello, it's firts contorller");
        //    return Content(_Configuration["CustomData"]);
        //}

    }
}