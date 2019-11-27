using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SergeiLevin0.Domain.Entities
{
    [Table("Categories")]
    public class Category : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set ; }
        public int? ParentId { get; set; }//будем использовать как внешний ключ. Если мы не указываем внешний ключ сами, то на основе навигационного св-ва (см ниже) система создаст сама внешний ключ используя имя навигационного и добавив к нему _ID
        [ForeignKey(nameof(ParentId))]//т.к. мы сами назначаем внешний ключ, для навигациооного св-ва должны уточнить его через атрибут
        public virtual Category ParentCategories { get; set; }//навигационное св-во
        public virtual ICollection<Product> Products { get; set; }//образуется отношение  1 ко многим. В 6й версии EntityFraeworkCore отсутствует готовое отношение многие ко многим. В 5й версии можно просто описать 2 навигационных коллекциооных св-ва, связывающих м/у собой 2 таблицы - система автоматически генерировала промежуточную таблицу связей. В 6й версии такую таблицу нужно делать самим (например, создать сущность CategoryProduct и описать связи в ручную).
    }
}
