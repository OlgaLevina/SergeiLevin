using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Interfaces
{

    /// <summary>
    /// сервис сотрудников
    /// </summary>
    public interface IEmpoyeesData
    {
        /// <summary>
        /// получить всех сотрудников
        /// </summary>
        /// <returns>все сотрудники, известные сервису</returns>
        IEnumerable<EmployeeView> GetAll();

        /// <summary>
        /// найти сотрудника по id
        /// </summary>
        /// <param name="id"> id  сотрудника</param>
        /// <returns>Сотрудник в указанным id</returns>
        EmployeeView GetById(int id);

        /// <summary>
        ///  добавление сотрудника
        /// </summary>
        /// <param name="employee">новый сотрудник</param>
        void Add(EmployeeView employee);

        /// <summary>
        /// удаление сотрудника
        /// </summary>
        /// <param name="id">id сотрудника</param>
        bool Delete(int id);
        
        /// <summary>
        /// внесение изменений
        /// </summary>
        /// <param name="id">id изменяемого сотрудника</param>
        /// <param name="employee">новые значения</param>
        EmployeeView Edit(int id, EmployeeView employee);
        /// <summary>
        /// сохранение изменений
        /// </summary>
        void SaveChanges();


    }
}
