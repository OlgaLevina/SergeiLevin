using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.ViewModels;


namespace SergeiLevin0.Controllers
{
    public class EmployeesController : Controller
    {

        public static readonly List<EmployeeView> EmployeesList = new List<EmployeeView>
        {
            new EmployeeView{Id=0, FirstName = "Ivan", SecondName = "Ivanov", Age = 38, Patronymic = "Ivanovich", Position = "Director", Role = "User"},
            new EmployeeView{Id=1, FirstName = "Petr", SecondName = "Petrov", Age = 25, Patronymic = "Petrovich", Position ="Meneger", Role = "Administrator"},
            new EmployeeView{Id=2, FirstName = "Sidr", SecondName = "Sidorov", Age = 18, Patronymic = "Sidorovich", Position = "Assistant", Role = "User"}
        };


        public IActionResult Index()
        {
            ViewBag.Title = "List of employees";//динамический объект, позволяет устанавлеивать любые сво-ва 
            ViewData["Description"] = "All employees";//спец.словарик куда можно вкладывать любые значения по любым ключам. Test - ключ
            //оба предыдущих элемента для передачи в предстваление каких-то малозначащих данных
            return View(EmployeesList);
        }
        public IActionResult EmployeeDatails(int? id)
        {
            if(id is null)  return BadRequest(); 
            EmployeeView employeee = EmployeesList.Find(i => i.Id == id);
            if (employeee is null) return NotFound();
            //ViewBag.Title = $"Employee Id{id}";
            ViewData["Description"] = "Only one employee";
            return View(employeee);
        }
    }
}