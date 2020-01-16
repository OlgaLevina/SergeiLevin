using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SergeiLevin0.Domain.Entities.Base
{
    /// <summary>
    /// базовая сущность
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//атрибуты можно 2мя отдельными, 2й указывает, что ключ должен быть уникальным
        public int Id { get ; set ; }//указывать атрибут не обязательно, система сама принимает за первичный ключ свойства с именем Id! Но если имя отличается - то атрибут обязателен
    }
}
