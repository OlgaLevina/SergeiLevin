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
using SergeiLevin0.Domain.DTO.Orders;

namespace SergeiLevin0.Infrastructure.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly SergeiLevinContext Db;
        private readonly UserManager<User> UserManager;

        public SqlOrderService(SergeiLevinContext db, UserManager<User> userManager) { Db = db; UserManager = userManager; }

        public OrderDTO CreateOrder(CreateOrderModel orderModel, string userName)
        {
            var user=UserManager.FindByNameAsync(userName).Result;
            using (var transaction = Db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = orderModel.OrderViewModel.Name,
                    Address = orderModel.OrderViewModel.Address,
                    Phone = orderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };
                Db.Orders.Add(order);
                foreach (var item in orderModel.OrderItems)//(product, quantity) -декомпозиция - есть дот.нет коре, но отсутствует в стандарте, поэтому нужно сделать ее самостоятельно
                {
                    var product = Db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if (product is null)
                        throw new InvalidOperationException($"Товар с идентификатором {item.Id} отсутствует!");
                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };
                    Db.OrderItems.Add(order_item);
                }
                Db.SaveChanges();
                transaction.Commit();
                return new OrderDTO
                {
                    Address = order.Address,
                    Date = order.Date,
                    Phone = order.Phone,
                    OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                    {
                        Id = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity
                    })
                };
            }
        }

        public OrderDTO GetOrderById(int id)
        {
            var o=Db.Orders
              .Include(order => order.OrderItems)
              .FirstOrDefault(order => order.Id == id);
            return o is null ? null : new OrderDTO
            {
                Phone = o.Phone,
                Address = o.Address,
                Date = o.Date,
                OrderItems = o.OrderItems.Select(item => new OrderItemDTO
                {
                    Id = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity
                })

            };
        }

        public IEnumerable<OrderDTO> GetUserOrders(string userName) => Db.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.UserName == userName).Select(o => new OrderDTO
            {
                Address=o.Address,
                Date=o.Date,
                Phone=o.Phone,
                OrderItems=o.OrderItems.Select(item => new OrderItemDTO
                {
                    Id=item.Id,
                    Price=item.Price,
                    Quantity=item.Quantity
                })
            });
    }
}
