using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Services
{
    //схема создания сервисов по одному шаблону:
    //1.реализуем интерфейс
    //2.Создаем конструктор в который передаем нужные сервисы, на основе которых он должен работать.
    //3. передаем этот параметр в приватное только для чтения поле
    public class SqlProductData : IProductData //будет отвечать за добавление новых товаров, категорий, брэндов. По сути IProductData реализует пэттерн Unit of work - операется на Контекст и организует работу внутри себя с сущностями
    {
        private readonly SergeiLevinContext Db;

        public SqlProductData(SergeiLevinContext db) => Db = db;

        public IEnumerable<Brand> GetBrands() => Db.Brands
            .Include(brand => brand.Products)
            .AsEnumerable();

        public IEnumerable<Category> GetCategories() => Db.Categories //загрузка в память
            .Include(category => category.Products)
            .AsEnumerable();

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = Db.Products;
            if (filter?.CategoryId != null)
                query = query.Where(product => product.CategoryId == filter.CategoryId);
            if (filter?.BrandId != null)
                query = query.Where(product => product.BrandId == filter.BrandId);
            return query.AsEnumerable();//query.ToArray() - можно вместо asenumerable
        }
        public Product GetProductById(int id) => Db.Products
            .Include(p =>p.Brand)
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
    }
}
