using Microsoft.EntityFrameworkCore;
using SergeiLevin0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.DAL.Context
{
    public class SergeiLevinContext: DbContext
    {
        //описываем таблицы, которые должны быть в базе - т.к. конфигурируем наш Context
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public SergeiLevinContext(DbContextOptions<SergeiLevinContext> options):base(options) { }
        //для FluentApi - следующий  метод, позволяет дополнять структуру нашей модели либо переписывать ее полностью. При первом запуске, когда система анелезирует структуру, она заглянет и сюда - доп.инфо в метаните. Например, здесь можно сделать каскадное удаление данных при отношении 1 ко многим
        //protected override void OnModelCreating(ModelBuilder model)
        //{
        //    base.OnModelCreating(model);
        //}
    }
}
