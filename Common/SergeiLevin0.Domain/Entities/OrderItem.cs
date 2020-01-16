using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SergeiLevin0.Domain.Entities
{
    public class OrderItem: BaseEntity
    {
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        [Column(TypeName="decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
