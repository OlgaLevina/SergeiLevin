using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.DTO.Orders;
using SergeiLevin0.Interfaces;

namespace SergeiLevin0.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersAPIController : ControllerBase, IOrderService
    {
        private readonly IOrderService orderService;

        public OrdersAPIController(IOrderService OrderService) => orderService = OrderService;
        [HttpPost("{UserName?}")]//опциональный параметр
        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)=> orderService.CreateOrder(OrderModel, UserName);
        [HttpGet("{id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int id)=> orderService.GetOrderById(id);
        [HttpGet("user/{UserName}"), ActionName("Get")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName)=> orderService.GetUserOrders(UserName);
    }
}