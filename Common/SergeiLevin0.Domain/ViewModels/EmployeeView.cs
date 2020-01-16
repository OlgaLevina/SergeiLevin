//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Domain.ViewModels
{
    /// <summary>
    /// модель сотрудника
    /// </summary>
    public class EmployeeView
    {
        //во многих атрибутах можно использовать рестурсы, для многоязычных версий
        //[HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Имя")]// [Display(Name ="Имя", ResourceType =)] - для многоязычной версии. Атрибут позволяет указать - как мы хотим визуализировать данные модели
        [Required] //атрибут обязательности. В случае его отсутствия в Post запросе ModelState.IsValid будет false (см. контроллер  Edit)
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Ups")]//валидация по длине (макс, мин)
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Обязательный параметр", AllowEmptyStrings = false)]//атрибудт можно конфигурировать, а можно оставить без параметров (сообщение при ошибке, запрет пустых строк), также для разных языков можно использовать ресурсы
        [RegularExpression(@"(?:[А-ЯЁ][а-яё]+)|(?:[A-Z][a-z]+)", ErrorMessage ="can't be Russian or English letters")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        [Required(ErrorMessage ="Ups")]//валидация по возрасту - в контроллере
        public int Age { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Display(Name = "Права")]
        public string Role { get; set; }

    }
}
