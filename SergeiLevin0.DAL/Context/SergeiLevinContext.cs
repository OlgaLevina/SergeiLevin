using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.DAL.Context
{
    public class SergeiLevinContext: IdentityDbContext<User, Role, string > //DbContext -  после добавления идентификации - заменяем с учетом идентификации. Если используем только пользователей, то добавляем просто User. Для ролей нужно добавить еще и класс первичного ключа
    {
        //описываем таблицы, которые должны быть в базе - т.к. конфигурируем наш Context
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }//в принципе OrderItems можно было и не прописывать, т.к. отсылка имеется в Order и автоматически тоже подцепилось бы 
        public SergeiLevinContext(DbContextOptions<SergeiLevinContext> options):base(options) { }
        //для FluentApi - следующий  метод, позволяет дополнять структуру нашей модели либо переписывать ее полностью. При первом запуске, когда система анелезирует структуру, она заглянет и сюда - доп.инфо в метаните. Например, здесь можно сделать каскадное удаление данных при отношении 1 ко многим
        //protected override void OnModelCreating(ModelBuilder model)
        //{
        //    base.OnModelCreating(model);
        //}
    }
}
