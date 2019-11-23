using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.ViewModels
{
    public class EmployeeView
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Display(Name ="Имя")]// [Display(Name ="Имя", ResourceType =)] - для многоязычной версии
        public  string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Display(Name = "Права")]
        public string Role { get; set; }

    }
}
