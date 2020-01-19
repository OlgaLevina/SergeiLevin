//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Domain.ViewModels
{
    public class UserOrderViewModel
    {
        //[HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Сумма")]
        public decimal TotalSum { get; set; }
    }
}
