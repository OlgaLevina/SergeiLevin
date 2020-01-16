using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.ViewModels.Idntity
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(256)]
        [Display(Name = "Имя Пользователя")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]//указываем, что это пароль и его нужно скрыть звездочками
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запомнить!")]
        public bool RememberMe { get; set; }
        [HiddenInput(DisplayValue =false)]//скрытый элемент управления
        public string ReturnUrl { get; set; }//если пользователь попытался войти в недоступную область и был перенаправлен на форму авторизации, то сохраняем инфу, куда он хотел войти, чтобы его после авторизации туда перенаправить
    }
}
