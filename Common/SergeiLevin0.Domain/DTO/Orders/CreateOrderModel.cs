using SergeiLevin0.Domain.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.DTO.Orders
{
    public class CreateOrderModel
    {
        public OrderViewModel OrderViewModel { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
