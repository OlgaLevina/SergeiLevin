using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Infrastructure.Interfaces;

namespace SergeiLevin0.Controllers
{
    public class CartController : Controller
    {
        private ICartService CartService;
        public CartController(ICartService cartService) => CartService = cartService;

        public IActionResult Details() => View(CartService.TransformFromCart());
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
        public IActionResult RemoveAllFromCart(int id)
        {
            CartService.RemoveAll();
            return RedirectToAction("Details");
        }

    }
}