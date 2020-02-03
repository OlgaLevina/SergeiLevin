using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SergeiLevin0.Domain.Models;
using SergeiLevin0.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Services
{
    public class CookiesCartStore : ICartStore//хранилище корзины, которое хранит данные кукис
    {
        private readonly string cartName;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Cart Cart
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[cartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCookie(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookie(httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(cartName);
            cookies.Append(cartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(15) });
        }

        public CookiesCartStore(IHttpContextAccessor HttpContextAccessor)
        {
            httpContextAccessor = HttpContextAccessor;
            var user = HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            cartName = $"cart[{user_name}]";
        }
    }
}
