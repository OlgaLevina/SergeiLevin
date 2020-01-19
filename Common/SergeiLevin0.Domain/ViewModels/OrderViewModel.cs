using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Domain.ViewModels
{/// <summary>
 /// модель-представление заказа        [Required(ErrorMessage = "Имя является обязательным")]

    /// </summary>
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Имя является обязательным")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указан номер телефона для связи")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Адрес является обязательным")]
        public string Address { get; set; }

    }
}
