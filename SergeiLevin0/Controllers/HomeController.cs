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
            //return Content("Hello, it's firts contorller");
            return Content(_Configuration["CustomData"]);
        }
    }
}