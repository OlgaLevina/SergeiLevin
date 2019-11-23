using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Base.Interfaces;

namespace SergeiLevin0.Domain.Entities
{
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set ; }
    }
}
