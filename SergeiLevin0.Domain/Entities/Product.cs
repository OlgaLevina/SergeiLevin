using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SergeiLevin0.Domain.Entities
{
    //[Table("Products")] //воспользуемся автоматикой системы
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set ; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public int? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public virtual Brand Brand { get; set; }
        public string ImageUrl { get; set; }
        [Column(TypeName ="decimal(18, 2)")]//decimal в базе отображается криво, поэтому нужно указать доп.параметры, т.к. система не может сама определить точность. Раньше без этого вообще ничего не работала, сейчас будет выдавать предупреждения
        public decimal Price { get; set; }
        //[Column(name:"TestValueColumn")] // пример назначения свойству колонки
        //public string TestValue { get; set; }
        //[NotMapped]//указываем, что сво-во не должно попадать в бд - это просто св-во модели
        //public int NotMappedProperty { get; set; }
    }
}
