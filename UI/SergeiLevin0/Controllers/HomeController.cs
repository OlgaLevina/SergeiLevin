using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using SergeiLevin0.Domain.ViewModels;

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
            //throw new InvalidOperationException("Отладочная ошибка в приложении");
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

        public IActionResult TestAction()
        {
            //return View();//вызов представления TestAction
            //return new ViewResult(); //ручное создаение представления

            //return new JsonResult(new { Customer = "Ivanon", Id = 1, Date = DateTime.Now }); //передаем для сериализации либо объект либо анонимный класс, ли так, либо см. следующий вариант
            //return Json(new { Customer = "Ivanon", Id = 1, Date = DateTime.Now });//вариант предыдущего

            //return Redirect("http://www.yandex.ru");//перенаправить
            //return new RedirectResult("http://www.yandex.ru");

            //return new RedirectToActionResult("Index", "Employees", null);//перенаправление внутри сайта с использование системы маршрутизации
            //return RedirectToAction("Index", "Employees"); //здесь система должна предлагать варианты, если она этого не делать - значит проблемы с маршрутизащией либо c vs

            //return Content("Hello world");
            //return new ContentResult { Content = "Hello world", ContentType = "application/text" };

            //return File(Encoding.UTF8.GetBytes("Hello world"), "application/text", "Hellowold.txt"); //передача в виде файла
            //return new FileContentResult (Encoding.UTF8.GetBytes("Hello world"), new MediaTypeHeaderValue("application/text")); //передача в виде файла
            //return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes("Hello world")), "application/text");

            //return StatusCode(405);
            //return new StatusCodeResult(500);

            return NoContent();
            //return new NoContentResult();
        }

        //public IActionResult ReadConfig()
        //{
        //    //return Content("Hello, it's firts contorller");
        //    return Content(_Configuration["CustomData"]);
        //}

    }
}