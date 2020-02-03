using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Domain.Models;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Interfaces.Services;
using SergeiLevin0.Services;
using Assert = Xunit.Assert;

namespace SergeiLeivn0.Services.Tests.Product
{
    class CartServiceTest
    {
        [TestClass]
        public class CartServiceTests
        {
            //начинаем с тестирования моделей, которые у нас используются
            [TestMethod]
            public void Cart_Class_ItemsCount_Returns_Correct_Quantity()//проверяем, что модель корзины возвращает корректное число товаров, содержащихся внутри 
            {
                const int expected_count = 4;
                var cart = new Cart//новая модель корзины
                {
                    Items = new List<CartItem>
                    {
                        new CartItem { Productid = 1, Quantity = 1 },
                        new CartItem { Productid = 2, Quantity = 3 },
                    }
                };
                var actual_count = cart.ItemsCount;
                Assert.Equal(expected_count, actual_count);
            }

            [TestMethod]
            public void CartViewModel_Returns_Correct_ItemsCount()//проверьем работы вьюмодели корзины, что она возрвщаем верное кол-во
            {
                const int expected_count = 4;
                var cart_view_model = new CartViewModel
                {
                    Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel {Id = 1, Name = "Product 1", Price = 0.5m}, 1 },
                    { new ProductViewModel {Id = 2, Name = "Product 2", Price = 1.5m}, 3 },
                }
                };
                var actual_count = cart_view_model.ItemsCount;
                Assert.Equal(expected_count, actual_count);
            }
            //начинаем проверять сервис самой корзины
            [TestMethod]
            public void CartService_AddToCart_WorkCorrect()//верность работы метода добваления в коризину
            {
                var cart = new Cart { Items = new List<CartItem>() };//создаем пустую корзину
                var product_data_mock = new Mock<IProductData>();
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                const int expected_id = 5;
                cart_service.AddToCart(expected_id);
                Assert.Equal(1, cart.ItemsCount);
                Assert.Single(cart.Items);
                Assert.Equal(expected_id, cart.Items[0].Productid);
            }

            [TestMethod]
            public void CartService_RemoveFromCart_Remove_Correct_Item()
            {
                const int item_id = 1;
                var cart = new Cart
                {Items = new List<CartItem>{
                        new CartItem { Productid = item_id, Quantity = 1 },
                        new CartItem { Productid = 2, Quantity = 3 }
                    }
                };
                var product_data_mock = new Mock<IProductData>();
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                cart_service.RemoveFromCart(item_id);
                Assert.Single(cart.Items);
                Assert.Equal(2, cart.Items[0].Productid);
            }

            [TestMethod]
            public void CartService_RemoveAll_ClearCart()
            {
                const int item_id = 1;
                var cart = new Cart
                {Items = new List<CartItem>{
                        new CartItem { Productid = item_id, Quantity = 1 },
                        new CartItem { Productid = 2, Quantity = 3 }
                    }
                };
                var product_data_mock = new Mock<IProductData>();
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                cart_service.RemoveAll();
                Assert.Empty(cart.Items);
            }

            [TestMethod]
            public void CartService_Decrement_Correct()
            {
                const int item_id = 1;
                var cart = new Cart
                {Items = new List<CartItem>{
                        new CartItem { Productid = item_id, Quantity = 3 },
                        new CartItem { Productid = 2, Quantity = 5 }
                    }
                };
                var product_data_mock = new Mock<IProductData>();
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                cart_service.DecrimentFromCart(item_id);
                Assert.Equal(7, cart.ItemsCount);
                Assert.Equal(2, cart.Items.Count);
                Assert.Equal(item_id, cart.Items[0].Productid);
                Assert.Equal(2, cart.Items[0].Quantity);
            }

            [TestMethod]
            public void CartService_Remove_Item_When_Decrement_to_0()
            {
                const int item_id = 1;
                var cart = new Cart
                { Items = new List<CartItem>                {
                        new CartItem { Productid = item_id, Quantity = 1 },
                        new CartItem { Productid = 2, Quantity = 5 }
                    }
                };
                var product_data_mock = new Mock<IProductData>();
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                cart_service.DecrimentFromCart(item_id);
                Assert.Equal(5, cart.ItemsCount);
                Assert.Single(cart.Items);
            }

            [TestMethod]
            public void CartService_TransformFromCart_WorkCorrect()
            {
                var cart = new Cart
                {Items = new List<CartItem>{
                        new CartItem { Productid = 1, Quantity = 1 },
                        new CartItem { Productid = 2, Quantity = 5 }
                   }
                };
                var products = new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO {Id = 1, Name = "Brand 1"}
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO {Id = 1, Name = "Brand 1"}
                    },
                };
                var product_data_mock = new Mock<IProductData>();
                product_data_mock
                   .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                   .Returns(products);
                var cart_store_mock = new Mock<ICartStore>();
                cart_store_mock.Setup(c => c.Cart).Returns(cart);
                var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);
                var result = cart_service.TransformFromCart();
                Assert.Equal(6, result.ItemsCount);
                Assert.Equal(1.1m, result.Items.First().Key.Price);
            }
        }
    }
}
