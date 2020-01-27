using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

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
                //конфигурация на этапе запуска приложения возможно, но лучше это делать в файле конфигурации приложения
                //.ConfigureLogging((host, log)=>//2 перегрузки с одним параметром и расширенный - с нашим контекстом
                //{
                //    //способы конфигурации провайдеров:
                //    //log.ClearProviders();//очистка всех провайдеров и тем самым отключение системы логирования
                //    //log.AddConsole(opt => opt.IncludeScopes = true);//подключаем консольный провайдер
                //    //log.AddDebug();//добавление еще одного провайдера
                //    //способы конфигурации фильтров:
                //    //log.AddFilter("System", LogLevel.Error);//провайдеры будут отображать для пространства имен System только ошибки
                //    //log.AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Error);//установка фильтра для конкретного провайдера
                //    //log.AddFilter<ConsoleLoggerProvider>((Namespace, level) =>!Namespace.StartsWith("Microsoft") || level >= LogLevel.Error);//фильтр нацелен не только на пространство имен, но и на уровень

                //}) 
                //.UseUrls("http://0.0.0.0:8080")//http://localhost:8080/ - подключение к сайту через порт; подключение с телефона - https://remontka.pro/connect-android-windows-lan/; https://issue.life/questions/48262739
                .UseStartup<Startup>();
    }
}
