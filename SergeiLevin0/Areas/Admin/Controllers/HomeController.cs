using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities.Identity;

namespace SergeiLevin0.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles =Role.Administrator)]//соглашение для контроллеров внутри областей не прописаны, можно их объявить самостоятельно в  public void ConfigureServices - services.AddMvc(); (Startup), тогда уже не нужно будет писать этот атрибут 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}