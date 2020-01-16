﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;

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
        public IActionResult DecrimentToCart(int id)
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
            var order = OrderService.CreateOrder(Model, CartService.TransformFromCart(), User.Identity.Name);
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