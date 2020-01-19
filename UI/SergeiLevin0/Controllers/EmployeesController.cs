using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;


namespace SergeiLevin0.Controllers
{
    //[Route("Users")] // новый маршрут для контроллера целиком, но для него нужно перенести представление в папку Users
    [Authorize]//доступ только дл авторизованных пользователей
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
        [Authorize(Roles =Role.Administrator)]//ограничиваем доступ - только администратор
        public IActionResult Edit(int? id) //для выбора редактрования; для создания используем отдельный блок действий Create, либо используем null индекс в эдит
        {
            if (id is null) return View(new EmployeeView()); // для создания нового сотрудника вместо Create
            //if (id < 0) return BadRequest();
            EmployeeView employeee = EmpoyeesData.GetById((int)id); 
            if (employeee is null) return NotFound();
            return View(employeee);
        }
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public IActionResult Edit(EmployeeView employee, bool creat) //для отработки сформированных данных, различия только в параметрах недостаточно, нужно разделение по атрибутам
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (employee.Age < 18) ModelState.AddModelError(nameof(EmployeeView.Age), "must be >= 18");
            if (employee.FirstName == "Иван" && employee.SecondName == "Грозный" && employee.Patronymic == "Васильевич") ModelState.AddModelError("", "Внимание! Он уже умер!"); //валидаций всей модели
            if (!ModelState.IsValid) return View(employee);
            int id = employee.Id;
            if (creat) EmpoyeesData.Add(employee);
            else EmpoyeesData.Edit(id, employee);
            EmpoyeesData.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create() => View(new EmployeeView());

        [HttpPost]
        public IActionResult Create(EmployeeView employee)
        {
            if (!ModelState.IsValid) return View(employee);
            EmpoyeesData.SaveChanges();
            return RedirectToAction("EmployeeDatails", new { employee.Id });
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete( int id) //просто удалять или редактировать нельзя, только черзе HTTPP-запросы (атрибуты) - иначе может произойти несанкциониованное изменене сразу черзе метод
        {
            var employee = EmpoyeesData.GetById(id);
            if (employee is null) return NotFound();
            return View(employee);

            //EmpoyeesData.Delete((int)id); // пример удаления напрямую - очень плохой вариант
            //return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]//если указать роли через запятую - то значит пользователь должен удовлетворять им всем одновременно, а есть указать несколько атрибутов, то будет доступ у всех указанных ролей (либо наоборот - проверить)
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            EmpoyeesData.Delete(id);
            return RedirectToAction("Index");
        }

    }
}