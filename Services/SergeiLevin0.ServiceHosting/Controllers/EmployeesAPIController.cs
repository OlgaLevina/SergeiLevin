using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Interfaces;

namespace SergeiLevin0.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]// если оставить путь api/[controller], то выглядеть он будет как api/EmployeesAPI, поэтому меняем на свой путь
    [Route("api/employees")]
    [ApiController]
    public class EmployeesAPIController : ControllerBase, IEmpoyeesData
    {
        private readonly IEmpoyeesData empoyeesData;
        public EmployeesAPIController(IEmpoyeesData empoyeesData) => this.empoyeesData = empoyeesData;
        [HttpPost, ActionName("Post")]
        public void Add(EmployeeView employee) => empoyeesData.Add(employee);
        [HttpDelete("{id}"), ActionName("Delete")]
        public bool Delete(int id) => empoyeesData.Delete(id);
        [HttpPut("{id}"), ActionName("Put")]
        public void Edit(int id, EmployeeView employee) => empoyeesData.Edit(id, employee);
        [HttpGet, ActionName("Get")]//второй атрибут можно и не добавлять: если добавить то путь будет api/employees/Get, а без этого - просто путь к котроллеру
        public IEnumerable<EmployeeView> GetAll() => empoyeesData.GetAll();
        [HttpGet("{id}"), ActionName("Get")]//атрибут с параметром, важно, чтобы имя точно повторяло парметр метода!
        public EmployeeView GetById(int id)=> empoyeesData.GetById(id);
        [NonAction]//является частью интерфэйса, не не вэбAPI, поэтому не должен участовать в обработке входящих запросов
        public void SaveChanges()=>empoyeesData.SaveChanges();
    }
}