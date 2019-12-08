using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Infrastructure.Interfaces;
using SergeiLevin0.ViewModels;

namespace SergeiLevin0.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {


        public IActionResult Index()=> View();
        public IActionResult Orders([FromServices] IOrderService orderService)
        {
            var orders = orderService.GetUserOrders(User.Identity.Name);
            return View(orders.Select(order => new UserOrderViewModel
            {
                Address=order.Address,
                Id=order.Id,
                Name=order.Name,
                Phone=order.Phone,
                TotalSum=order.OrderItems.Sum(item => item.Price*item.Quantity)
            }));
        }
    }
}