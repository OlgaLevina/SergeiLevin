using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Domain.DTO.Orders;

namespace SergeiLevin0.Controllers
{
    //!!!см. пример в AccountController. В реальном проекте все контроллеры, которые каким-то образом изменяют данные, должны выполнять логирование своих действий. То, что в итоге будет попадать в лог-файл(-сервер) регулируется файлом конфигурации.
    public class CartController : Controller
    {
        private ICartService CartService;
        public CartController(ICartService cartService) => CartService = cartService;

        public IActionResult Details() => View(new OrderDetailsViewModel{
            CartViewModel=CartService.TransformFromCart(),
            OrderViewModel=new OrderViewModel()
        });
        public IActionResult AddToCart(int id)
        {
            CartService.AddToCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult DecrimentFromCart(int id)
        {
            CartService.DecrimentFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveFromCart(int id)
        {
            CartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveAllFromCart()
        {
            CartService.RemoveAll();
            return RedirectToAction("Details");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel Model, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new OrderDetailsViewModel
                {
                    CartViewModel=CartService.TransformFromCart(),
                    OrderViewModel=Model
                });
            var create_order_model = new CreateOrderModel
            {
                OrderViewModel = Model,
                OrderItems = CartService.TransformFromCart().Items
                .Select(item => new OrderItemDTO
                {
                    Id = item.Key.Id,
                    Price = item.Key.Price,
                    Quantity = item.Value
                }).ToList()
            };
            var order = OrderService.CreateOrder(create_order_model, User.Identity.Name);
            CartService.RemoveAll();
            return RedirectToAction("OrderConfirmed", new {id=order.Id });
        }
        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.Orderid = id;
            return View();
        }
    }
}