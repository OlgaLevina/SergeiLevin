using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly SergeiLevinContext Db;
        private readonly UserManager<User> UserManager;

        public SqlOrderService(SergeiLevinContext db, UserManager<User> userManager) { Db = db; UserManager = userManager; }

        public Order CreateOrder(OrderViewModel orderModel, CartViewModel cartModel, string userName)
        {
            var user=UserManager.FindByNameAsync(userName).Result;
            using (var transaction = Db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = orderModel.Name,
                    Address = orderModel.Address,
                    Phone = orderModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };
                Db.Orders.Add(order);
                foreach (var(product_model, quantity) in cartModel.Items)//(product, quantity) -декомпозиция
                {
                    var product = Db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                        throw new InvalidOperationException($"Товар с идентификатором {product_model.Id} отсутствует!");
                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = quantity,
                        Product = product
                    };
                    Db.OrderItems.Add(order_item);
                }
                Db.SaveChanges();
                transaction.Commit();
                return order;
            }
        }

        public Order GetOrderById(int id) => Db.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefault(order => order.Id == id);

        public IEnumerable<Order> GetUserOrders(string userName) => Db.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.UserName == userName);
    }
}
