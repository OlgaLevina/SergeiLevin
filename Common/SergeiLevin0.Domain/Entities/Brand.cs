using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace SergeiLevin0.Domain.Entities
{
    [Table("Brands")]//назначаем таблицу, необзятально, если оно повторяет имя сласса во множественном числе
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set ; }
        //навигационное св-во для установления связи между таблицами брендов и товаров
        public virtual ICollection<Product> Products { get; set; }//обязательно сделать виртуальным, иначе связ с таблицей появиться, но оно св-во не будет навигационным, т.е. в него данные автоматически загружаться не будут
    }
}
