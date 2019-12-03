using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SergeiLevin0.Infrastructure.Interfaces;
using SergeiLevin0.Models;
using SergeiLevin0.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Services
{
    public class CookieCartService : ICartService
    {
        private string CartName;
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
            throw new NotImplementedException();
        }

        public void DecrementFromCart(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int id)
        {
            throw new NotImplementedException();
        }

        public CartViewModel TransformFromCart()
        {
            throw new NotImplementedException();
        }
    }
}
