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
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

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
                .UseStartup<Startup>()
                .UseSerilog((host,log)=>
                {
                    log.ReadFrom.Configuration(host.Configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(//параметры вывода можно и не расписывать
                        outputTemplate: "[{Timstamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                    .WriteTo.RollingFile($".\\Logs\\SergeiLevin0[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")//файл для записи - сохранение в текстовом виде
                    .WriteTo.File(new JsonFormatter(",", true), $".\\Logs\\SergeiLevin0[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")//сохранение в json или xml
                    .WriteTo.Seq("http://localhost:50420");
                });
    }
}
