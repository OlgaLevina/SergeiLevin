using SergeiLevin0.Domain.Entities;
using SergeiLevin0.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetUserOrders(string UserName);
        Order GetOrderById(int id);
        Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName);
    }
}
