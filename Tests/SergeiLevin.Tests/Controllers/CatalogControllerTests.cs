using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SergeiLevin0.Controllers;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Domain.ViewModels;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Interfaces.Api;
using Assert = Xunit.Assert;


namespace SergeiLevin0.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_With_Correct_View()//для случая, когда найден товар по индексу
        {
            //A-A-A = Arrange - Act - Assert - важно, чтобы этот принцип построения модульных тестов соблюдался
            #region Arrange - размещение данных

            const int expected_id = 1;
            const decimal expected_price = 10m;
            var expected_name = $"Item id {expected_id}";
            var expected_brand_name = $"Brand of item {expected_id}";
            var product_data_mock = new Mock<IProductData>();//макет
            product_data_mock
               .Setup(p => p.GetProductById(It.IsAny<int>()))//получаем любое значение целого типа
               .Returns<int>(id => new ProductDTO//сконфигурированные данные должны совпасть с ожидаемыми
               {
                   Id = id,
                   Name = $"Item id {id}",
                   ImageUrl = $"Image_id_{id}.png",
                   Order = 0,
                   Price = expected_price,
                   Brand = new BrandDTO
                   {
                       Id = 1,
                       Name = $"Brand of item {id}"
                   }
               });
            var controller = new CatalogController(product_data_mock.Object);
            var logger_mock = new Mock<ILogger<CatalogController>>();//мок под логгер
            #endregion

            #region Act - выполнение тестируемого кода
            var result = controller.ProductDetails(expected_id, logger_mock.Object);
            #endregion

            #region Assert - проверка утверждений
            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);
            Assert.Equal(expected_id, model.Id);//здесь и далее проверяем модель на корректность
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand);
            #endregion
        }

        [TestMethod]
        public void ProductDetails_Returns_NotFound_if_Product_not_Exists()//для случая, если не найден товар
        {
            var logger_mock = new Mock<ILogger<CatalogController>>();
            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns(default(ProductDTO));//т.е. нулл
            var controller = new CatalogController(product_data_mock.Object);
            var result = controller.ProductDetails(1, logger_mock.Object);
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Returns_Correct_View()
        {
            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
               .Returns<ProductFilter>(filter => new[]//тест должен вернуть 2 дто-модели
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                        Order = 0,
                        Price = 10m,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO
                        {
                            Id = 1,
                            Name = "Brand 1"
                        }
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2",
                        Order = 1,
                        Price = 20m,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDTO
                        {
                            Id = 2,
                            Name = "Brand 2"
                        }
                    }
                });
            var controller = new CatalogController(product_data_mock.Object);
            const int expected_category_id = 1;
            const int expected_brand_id = 5;
            var result = controller.Shop(expected_category_id, expected_brand_id);
            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);
            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_category_id, model.CategoryId);
            Assert.Equal("Brand 1", model.Products.First().Brand);//проверяем, что хотя бы 1й продукт возвращает верный брэнд
        }
    }
}
