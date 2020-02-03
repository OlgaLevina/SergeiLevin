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
using SergeiLevin0.Interfaces.Services;

namespace SergeiLevin0.Services
{
    public class CartService : ICartService
    {
        private readonly IProductData ProductData;
        private readonly ICartStore cartStore;

        public CartService(IProductData productData,ICartStore CartStore)//именно интерфейсы!!
        {
            ProductData = productData;
            cartStore = CartStore;
        }

        public void AddToCart(int id)
        {
            var cart=cartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) cart.Items.Add(new CartItem { Productid = id, Quantity = 1 });
            else item.Quantity++;
            cartStore.Cart = cart;
        }

        public void DecrimentFromCart(int id)
        {
            var cart = cartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) return;
            if(item.Quantity>0) item.Quantity--;
            if (item.Quantity == 0) cart.Items.Remove(item);
            cartStore.Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = cartStore.Cart;
            cart.Items.Clear();
            cartStore.Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = cartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.Productid == id);
            if (item is null) return;
            cart.Items.Remove(item);
            cartStore.Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = ProductData.GetProducts(new ProductFilter
            {
                Ids = cartStore.Cart.Items.Select(item => item.Productid).ToList()
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
                Items = cartStore.Cart.Items.ToDictionary(
                    x => products_view_models.First(p => p.Id == x.Productid),
                    x => x.Quantity
                    )
            };
        }
    }
}
