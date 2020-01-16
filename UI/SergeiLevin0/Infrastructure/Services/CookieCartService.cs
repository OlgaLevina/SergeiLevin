using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.Models;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Services
{
    public class CookieCartService : ICartService
    {
        private readonly string CartName;
        private readonly IProductData ProductData;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private Cart Cart
        {
            get
            {
                var context = HttpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[CartName];
                if(cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaseCookie(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaseCookie(HttpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaseCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(CartName);
            cookies.Append(CartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(15) });
        }
        public CookieCartService(IProductData productData,IHttpContextAccessor httpContextAccessor)//именно интерфейсы!!
        {
            ProductData = productData;
            HttpContextAccessor = httpContextAccessor;
            var user = httpContextAccessor.HttpContext.User;
            var user_name=user.Identity.IsAuthenticated ? user.Identity.Name : null;
            CartName = $"cart[{user_name}]";
        }

        public void AddToCart(int id)
        {
            var cart=Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) cart.Items.Add(new CartItem { Productid = id, Quantity = 1 });
            else item.Quantity++;
            Cart = cart;
        }

        public void DecrimentFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) return;
            if(item.Quantity>0) item.Quantity--;
            if (item.Quantity == 0) cart.Items.Remove(item);
            Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) return;
            cart.Items.Remove(item);
            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.Productid).ToList()
            });
            var products_view_models = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Order = p.Order,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name
            });
            return new CartViewModel
            {
                Items = Cart.Items.ToDictionary(
                    x => products_view_models.First(p => p.Id == x.Productid),
                    x => x.Quantity
                    )
            };
        }
    }
}
