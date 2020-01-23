using SergeiLevin0.Domain.DTO.Orders;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetUserOrders(string UserName);
        OrderDTO GetOrderById(int id);
        //Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName); - упрощаем через модель дто
        OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName);
    }
}
