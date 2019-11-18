using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Infrastructure.Interfaces;
using SergeiLevin0.ViewModels;


namespace SergeiLevin0.Controllers
{
    //[Route("Users")] // новый маршрут для контроллера целиком, но для него нужно перенести представление в папку Users
    public class EmployeesController : Controller
    {
        private readonly IEmpoyeesData EmpoyeesData;

        //public static readonly List<EmployeeView> EmployeesList = new List<EmployeeView>
        //{
        //    new EmployeeView{Id=0, FirstName = "Ivan", SecondName = "Ivanov", Age = 38, Patronymic = "Ivanovich", Position = "Director", Role = "User"},
        //    new EmployeeView{Id=1, FirstName = "Petr", SecondName = "Petrov", Age = 25, Patronymic = "Petrovich", Position ="Meneger", Role = "Administrator"},
        //    new EmployeeView{Id=2, FirstName = "Sidr", SecondName = "Sidorov", Age = 18, Patronymic = "Sidorovich", Position = "Assistant", Role = "User"}
        //};

        public EmployeesController(IEmpoyeesData empoyeesData) { EmpoyeesData = empoyeesData; }

        public IActionResult Index()
        {
            ViewBag.Title = "List of employees";//динамический объект, позволяет устанавлеивать любые сво-ва 
            ViewData["Description"] = "All employees";//спец.словарик куда можно вкладывать любые значения по любым ключам. Test - ключ
            //оба предыдущих элемента для передачи в предстваление каких-то малозначащих данных
            return View(EmpoyeesData.GetAll());
        }

        //[Route("id")]//маршрут на конкретного действия
        public IActionResult EmployeeDatails(int? id)
        {
            if (id is null) return BadRequest();
            EmployeeView employeee = EmpoyeesData.GetById((int)id);
            if (employeee is null) return NotFound();
            //ViewBag.Title = $"Employee Id{id}";
            //ViewData["Description"] = "Only one employee";
            return View(employeee);
        }

        public IActionResult DetailsName(string firstName, string lastName)
        {
            if (firstName is null || lastName is null) return BadRequest();
            IEnumerable<EmployeeView> employees = EmpoyeesData.GetAll();
            if (!string.IsNullOrWhiteSpace(firstName)) employees = employees.Where(e => e.FirstName == firstName);
            if (!string.IsNullOrWhiteSpace(lastName)) employees = employees.Where(e => e.SecondName == lastName);
            var employee = employees.FirstOrDefault();
            if (employee is null) return NotFound();
            return View(nameof(EmployeeDatails), employee);

        }

        //[HttpPost] //вызывается только под действием формата post к контроллеру, в этом случае можно передать много параметров - например все тело страницы
        //public IActionResult Edit(int id, [FromBody] EmployeeView employee)//данные модели - из тела запроса, а не из строки адреса - frombody
        //{
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Edit(int id) //для выбора редактрования
        {
            if (id < 0) return BadRequest();
            EmployeeView employeee = EmpoyeesData.GetById((int)id);
            if (employeee is null) return NotFound();
            return View(employeee);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeView employee) //для отработки сформированных данных, различия только в параметрах недостаточно, нужно разделение по атрибутам
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (!ModelState.IsValid) View(employee);
            int id = employee.Id;
            EmpoyeesData.Edit(id, employee);
            EmpoyeesData.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}