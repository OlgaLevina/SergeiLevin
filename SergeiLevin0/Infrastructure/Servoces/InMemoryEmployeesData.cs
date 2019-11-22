using SergeiLevin0.Infrastructure.Interfaces;
using SergeiLevin0.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Servoces
{
    public class InMemoryEmployeesData : IEmpoyeesData
    {
        public readonly List<EmployeeView> EmployeesList = new List<EmployeeView>
        {
            new EmployeeView{Id=0, FirstName = "Ivan", SecondName = "Ivanov", Age = 38, Patronymic = "Ivanovich", Position = "Director", Role = "User"},
            new EmployeeView{Id=1, FirstName = "Petr", SecondName = "Petrov", Age = 25, Patronymic = "Petrovich", Position ="Meneger", Role = "Administrator"},
            new EmployeeView{Id=2, FirstName = "Sidr", SecondName = "Sidorov", Age = 17, Patronymic = "Sidorovich", Position = "Assistant", Role = "User"}
        };


        public void Add(EmployeeView employee)
        {
            if(employee is null) throw new ArgumentNullException(nameof(employee));
            employee.Id = EmployeesList.Count == 0 ? 1 : EmployeesList.Max(e => e.Id) + 1;
            EmployeesList.Add(employee);
        }

        public bool Delete(int id)
        {
            EmployeeView employeeOld = GetById(id);
            if (employeeOld is null) return false;
            return EmployeesList.Remove(employeeOld);
        }

        public void Edit(int id, EmployeeView employee) 
        {
            if(employee is null) throw new ArgumentNullException(nameof(employee));
            EmployeeView employeeOld = GetById(id);
            if (employeeOld is null) return;
            employeeOld.FirstName = employee.FirstName;
            employeeOld.Age = employee.Age;
            employeeOld.Patronymic = employee.Patronymic;
            employeeOld.Position = employee.Position;
            employeeOld.Role = employee.Role;
            employeeOld.SecondName = employee.SecondName;
        }

        public IEnumerable<EmployeeView> GetAll() => EmployeesList;

        public EmployeeView GetById(int id) => EmployeesList.FirstOrDefault(e => e.Id == id);

        public void SaveChanges()
        {
            //throw new NotImplementedException();
        }
    }
}
