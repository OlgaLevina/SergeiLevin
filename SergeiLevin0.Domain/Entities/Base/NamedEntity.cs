using SergeiLevin0.Domain.Entities.Base.Interfaces;

namespace SergeiLevin0.Domain.Entities.Base
{
    /// <summary>
    /// именованная сущеность
    /// </summary>
    public abstract class NamedEntity : BaseEntity, INamedEntity
    {
        public string Name { get; set ; }
    }
}
