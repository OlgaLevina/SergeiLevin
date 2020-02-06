using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly ICartService CartService;
        public CartViewComponent(ICartService cartService) => CartService = cartService;
        public IViewComponentResult Invoke() => View(CartService.TransformFromCart());
    }
}
