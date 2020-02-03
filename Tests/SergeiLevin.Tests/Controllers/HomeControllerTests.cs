using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SergeiLevin0.Controllers;
using Assert = Xunit.Assert;

namespace SergeiLevin0.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Return_View()
        {
            var controller = new HomeController();//вызываем контроллер
            var result = controller.Index();//вызываем метод
            Assert.IsType<ViewResult>(result); //проверяем, что в рузельтате то, что мы ожидаем
        }
        [TestMethod]
        public void Blog_Return_View()
        {
            var controller = new HomeController();
            var result = controller.Blog();
            Assert.IsType<ViewResult>(result); 
        }
        [TestMethod]
        public void BlogSingle_Return_View()
        {
            var controller = new HomeController();
            var result = controller.BlogSingle();
            Assert.IsType<ViewResult>(result); 
        }
        [TestMethod]
        public void ContactUs_Return_View()
        {
            var controller = new HomeController();
            var result = controller.ContactUs();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Error404_Return_View()
        {
            var controller = new HomeController();
            var result = controller.Error404();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ErrorStatus_404_Redirect_to_Error404()//в контроллере нет - досоздаем
        {
            var controller = new HomeController();
            var result = controller.ErrorStatus("404");
            var redirect_to_action = Assert.IsType<RedirectToActionResult>(result);//проверяем,что результатом является объект редирект ту экшен ризалт
            Assert.Null(redirect_to_action.ControllerName);//проверяем, что имя контроллера не равно нулю
            Assert.Equal(nameof(HomeController.Error404), redirect_to_action.ActionName);//проверяем, что действие, куда нас направили является Эррор404
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void ThrowException_throw_Application()//в контроллере нет - досоздаем
        {
            var controller = new HomeController();
            var result = controller.ThrowException();
        }
        
    }
}
