using SergeiLevin0.Domain.DTO.Orders;
using SergeiLevin0.Domain.Entities;

namespace SergeiLevin0.Services.Map
{
    static public class OrderItemMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem orderItem) => orderItem is null ? null : new OrderItemDTO
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
        public static OrderItem FromDTO(this OrderItemDTO orderItem) => orderItem is null ? null : new OrderItem
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
    }
}
