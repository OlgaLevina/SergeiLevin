using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.DTO.Orders;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SergeiLevin0.Clients.Orders
{
    public class OrdersClient: BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration configutation) : base(configutation, "api/orders") { }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName) => Post($"{ServiceAddress}/{UserName}", OrderModel)
            .Content.ReadAsAsync<OrderDTO>().Result;

        public OrderDTO GetOrderById(int id)=>Get<OrderDTO>($"{ServiceAddress}/{id}");

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => Get<List<OrderDTO>>($"{ServiceAddress}/user/{UserName}");
    }
}
