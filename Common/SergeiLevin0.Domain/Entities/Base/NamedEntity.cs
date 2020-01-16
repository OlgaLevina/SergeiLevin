using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SergeiLevin0.Domain.Entities.Base
{
    /// <summary>
    /// именованная сущеность
    /// </summary>
    public abstract class NamedEntity : BaseEntity, INamedEntity
    {
        [Required]//обязательное поле, не допускаем null
        public string Name { get; set ; }
    }
}
