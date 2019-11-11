﻿using System;
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

        public static  readonly List<EmployeeView> EmployeesList=new List<EmployeeView>
        {
            new EmployeeView{Id=0, FirstName = "Ivan", SecondName = "Ivanov", Age = 38, Patronymic = "Ivanovich"},
            new EmployeeView{Id=1, FirstName = "Petr", SecondName = "Petrov", Age = 25, Patronymic = "Petrovich"},
            new EmployeeView{Id=2, FirstName = "Sidr", SecondName = "Sidorov", Age = 18, Patronymic = "Sidorovich"}
        };

        public IActionResult GetEmployees()
        {
            ViewBag.SomeData = "Hello World!";//динамический объект, позволяет устанавлеивать любые сво-ва 
            ViewData["Test"] = "TestData";//спец.словарик куда можно вкладывать любые значения по любым ключам. Test - ключ
            //оба предыдущих элемента для передачи в предстваление каких-то малозначащих данных
            return View(EmployeesList);
        }
    }
}