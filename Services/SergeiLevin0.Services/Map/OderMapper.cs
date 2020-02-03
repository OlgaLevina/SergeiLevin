using SergeiLevin0.Domain.DTO.Orders;
using SergeiLevin0.Domain.Entities;
using System.Linq;

namespace SergeiLevin0.Services.Map
{
    static public class OderMapper
    {
        public static OrderDTO ToDTO(this Order order) => order is null ? null : new OrderDTO
        {
            Id = order.Id,
            Name=order.Name,
            Date=order.Date,
            Address=order.Address,
            Phone=order.Phone,
            OrderItems=order.OrderItems.Select(OrderItemMapper.ToDTO)
        };
        public static Order FromDTO(this OrderDTO order) => order is null ? null : new Order
        {
            Id = order.Id,
            Name = order.Name,
            Date = order.Date,
            Address = order.Address,
            Phone = order.Phone,
            OrderItems = order.OrderItems.Select(OrderItemMapper.FromDTO).ToArray()
        };
    }
}
