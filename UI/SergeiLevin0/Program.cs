using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SergeiLevin0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseUrls("http://0.0.0.0:8080")//http://localhost:8080/ - подключение к сайту через порт; подключение с телефона - https://remontka.pro/connect-android-windows-lan/; https://issue.life/questions/48262739
                .UseStartup<Startup>();
    }
}
