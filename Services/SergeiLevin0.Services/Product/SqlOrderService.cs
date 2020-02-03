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
using Microsoft.Extensions.Logging;
using SergeiLevin0.Services.Map;

namespace SergeiLevin0.Infrastructure.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly SergeiLevinContext Db;
        private readonly UserManager<User> UserManager;
        private readonly ILogger<SqlOrderService> logger;

        public SqlOrderService(SergeiLevinContext db, UserManager<User> userManager, Logger<SqlOrderService> Logger) { Db = db; UserManager = userManager;
            logger = Logger;
        }

        public OrderDTO CreateOrder(CreateOrderModel orderModel, string userName)
        {
            using (logger.BeginScope($"Create order of {userName}"))
            {
                var user = UserManager.FindByNameAsync(userName).Result;
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
                    logger.LogInformation("New order was created and added to db!");
                    foreach (var item in orderModel.OrderItems)//(product, quantity) -декомпозиция - есть дот.нет коре, но отсутствует в стандарте, поэтому нужно сделать ее самостоятельно
                    {
                        var product = Db.Products.FirstOrDefault(p => p.Id == item.Id);
                        if (product is null)
                            throw new InvalidOperationException($"Товар с идентификатором {item.Id} отсутствует!");
                        logger.LogInformation($"Product {product.Id} was found in db!");
                        var order_item = new OrderItem
                        {
                            Order = order,
                            Price = product.Price,
                            Quantity = item.Quantity,
                            Product = product
                        };
                        logger.LogInformation($"Product {product.Id} was got ready to add to db!");
                        Db.OrderItems.Add(order_item);
                        logger.LogInformation($"Product {product.Id} was added to order in db!");
                    }
                    Db.SaveChanges();
                    transaction.Commit();
                    logger.LogInformation($"New order with products was saved in db!");
                    return order.ToDTO();
                }
            }
        }

        public OrderDTO GetOrderById(int id)=>
            Db.Orders
              .Include(order => order.OrderItems)
              .FirstOrDefault(order => order.Id == id).ToDTO();

        public IEnumerable<OrderDTO> GetUserOrders(string userName) => Db.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.UserName == userName).Select(OderMapper.ToDTO);

    }
}
