using SergeiLevin0.Domain.Entities.Base;

namespace SergeiLevin0.Domain.DTO.Orders
{
    public class OrderItemDTO: BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
