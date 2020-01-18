using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Clients.Employees
{
    public class EmployeesClient: BaseClient, IEmpoyeesData
    {
        private readonly IConfiguration configuration;

        public EmployeesClient(IConfiguration configuration) : base(configuration, "api/employees") { }

        public void Add(EmployeeView employee)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(int id, EmployeeView employee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeView> GetAll()
        {
            throw new NotImplementedException();
        }

        public EmployeeView GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
