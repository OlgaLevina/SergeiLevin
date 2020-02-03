using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SergeiLevin0.Controllers;
using SergeiLevin0.Interfaces.Api;
using Assert = Xunit.Assert;


namespace SergeiLevin0.Tests.Controllers
{
    [TestClass]
    public class WebAPITestControllerTests
    {
        [TestMethod]
        public async  Task Index_Return_View_With_Values()//т.к. метод контроллера асинхронный, то и тест асинхронный
        {
            var expected_values = new[] { "1", "2", "3" };//создаем тестовый набор данных, который должен вернуть метод
            var value_service_mock = new Mock<IValuesService>();//создаем макет интерфэйса
            value_service_mock.Setup(service => service.GetAsync())//конфигурируем макет, настраивая на поведение: указываем, что когда будет вызван метод гет-асинк - вернуть в качестве результата 
                .ReturnsAsync(expected_values);
            var controller = new WebAPITestController(value_service_mock.Object);//вызываем контроллер и в параметрах макет интерфэйса, как объекта
            var result =await controller.Index();//вызываем метод
            var view_result=Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);//извлекаем модель, которая упаковывается контрллером в наш вьюрезалт
            Assert.Equal(expected_values.Length, model.Count());//тестируем модель на количество возвращаемых данных
            value_service_mock.Verify(sercive => sercive.GetAsync());//проверяем, что был вызов гетасинк указанное кол-во раз
            value_service_mock.VerifyNoOtherCalls();//проверяем, что никакие иные методы, кроме гетасинка не вызывались
        }
    }
}
