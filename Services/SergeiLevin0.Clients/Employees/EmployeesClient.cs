using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SergeiLevin0.Clients.Employees
{
    public class EmployeesClient: BaseClient, IEmpoyeesData
    {
        private readonly IConfiguration configuration;

        public EmployeesClient(IConfiguration configuration) : base(configuration, "api/employees") { }

        public void Add(EmployeeView employee) => Post(ServiceAddress, employee);

        public bool Delete(int id) => base.Delete($"{ServiceAddress}/{id}").IsSuccessStatusCode;

        public EmployeeView Edit(int id, EmployeeView employee)
        {
            var response = Put($"{ServiceAddress}/{id}", employee);
            return response.Content.ReadAsAsync<EmployeeView>().Result;
        }

        public IEnumerable<EmployeeView> GetAll() => Get<List<EmployeeView>>(ServiceAddress);

        public EmployeeView GetById(int id) => Get<EmployeeView>($"{ServiceAddress}/{id}");

        public void SaveChanges(){}
    }
}
