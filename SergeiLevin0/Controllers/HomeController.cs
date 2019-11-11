using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SergeiLevin0.Controllers
{
    //[Controller]
    public class HomeController : Controller //если не наследовать от класса Controller, то нужно подписать атрибутом Controller.
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();//вызовет предоставление из папки Home с название Index. Если хотим вызвать иное представление: return View("OtherView"); - которое тоже будет искаться в папке Хоум
        }

        public IActionResult ReadConfig()
        {
            //return Content("Hello, it's firts contorller");
            return Content(_Configuration["CustomData"]);
        }
    }
}