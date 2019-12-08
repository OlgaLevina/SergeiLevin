using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Infrastructure.Interfaces;

namespace SergeiLevin0.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles =Role.Administrator)]//соглашение для контроллеров внутри областей не прописаны, можно их объявить самостоятельно в  public void ConfigureServices - services.AddMvc(); (Startup), тогда уже не нужно будет писать этот атрибут 
    public class HomeController : Controller
    {
        //private readonly IProductData ProductData;

        //public HomeController(IProductData productData) => ProductData = productData;


        public IActionResult Index()=>     View();
        //public IActionResult List() => View(ProductData.GetProducts());//работа с продуктами вынесена в отдельный контроллекр
        //public IActionResult Edit(int? id) => View();
        //public IActionResult Delete(int? id) => View();
        //[HttpPost, ActionName(nameof(Delete))]
        //public IActionResult ProductDeleteConfirm(int? id) => RedirectToAction("List");
    }
}