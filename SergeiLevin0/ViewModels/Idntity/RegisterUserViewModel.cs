using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.ViewModels.Idntity
{
    public class RegisterUserViewModel
    {
        [Required]
        [MaxLength(256)]
        [Display(Name ="Имя Пользователя")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]//указываем, что это пароль и его нужно скрыть звездочками
        [Display(Name ="Пароль")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Подтвердите введенный пароль")]
        [Compare(nameof(Password))] //автоматическое сравнение корректности двух полей
        public string ConfirmPassword { get; set; }
    }
}
