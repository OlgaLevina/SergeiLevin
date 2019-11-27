﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Data;
using SergeiLevin0.Infrastructure.Convenctions;
using SergeiLevin0.Infrastructure.Interfaces;
using SergeiLevin0.Infrastructure.Services;

namespace SergeiLevin0
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Config) => Configuration = Config;

        //набиваем сервисами, с которыми в дальнейшем будет работать наше приложение
        public void ConfigureServices(IServiceCollection services) // станадртный конетейнер может быть заменен на контейнер сторонних разработчиков - как с см. в инете
        {// недостаток стандартнгого контейнера - то что он конфигурирвуется в виде кода, не станадартные позволяют делать это в виде xml- конфигурации, который прикладывается к проекту и без перекомпеляции вносит изменения
         //контейнер сервисов представляет их коллекцюю IServiceCollection services
         //все вервисы должны быть зарегистрированы в контейнере!!
            //есть подход по проектированию взаимодействия с базами данных, когда пишуться изолированные контексты. В этом случае в приложении может быть несколько контекстов. Между собо они могут быть связаны внешними ключами между таблицами или не свяаны. При этом описываются несколько классов с контектами данных и каждый из них регистрируется, как ниже. Это может понадобиться, если в одном контексте храняться, например, данные пользователей, а в другом данные товаров. Контексты не связаны между собой, могут разрабатываться и тестироваться по отдельности. При этом сами БД могут лежать на разных серверах, либо это может быть 1 БД, к которой формируются 2 контекста
            services.AddDbContext<SergeiLevinContext>(opt=>opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//здесь можно добавлять к строке подключания данные логина и пароля, чтобы не размещать их в файле конфигурации приложения, здесь же можно менять иные параметры строки подключения
            services.AddTransient<SergeiLevinContextInitializer>();
            // методы добавления сервисов:
            //1. самостоятельное добавление сервисов:
            services.AddSingleton<IEmpoyeesData, InMemoryEmployeesData>(); //в режиме singleton  - создается только 1 экземляр класса (единого объекта на все время жизни приложения с момента 1го обращения к нему), который будет раздается всем желающим в дальнейшем; можно регитриовтаь просто класс без интерфейса!
            //services.AddTransient<>(); //один отдельный объект сосздается при каждом запросе.
            //services.AddScoped<>();//один отельный объект на время обработки одного входящего запроса (жизни области), что-то типа using
            //в параметрах могут быть не только классы, но и типы!!!
        //2. интегрированные методы расширения, включает в себя уже все, что нужно.  
            //services.AddSession(); // сервис к app.UseSession()
           // services.AddScoped<IProductData,InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddMvc();//запуск mvc без внесения изменений в конвенции.
            //services.AddMvc(opt => opt.Conventions.Add(new CustomControllerConvention())); //пример внесения изменений в конвенции на этапе старта - то что в скобках мое предположение.

        }

        //содержит промежуточное ПО, которое добавляется к нашему приложению через app.Use..., и формируют конвеер обработки входящих запросов в той послеовательности, как добавленяется.
        //потом удалить параметр data или переделать!!!!
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEmpoyeesData data, SergeiLevinContextInitializer db) //в параметрах можно указать зарегистрированные сервисы, например IEmpoyeesData; сервисы можно заправшивать так же в контроллерах  и других частях приложения
        {
            db.InitializeAsync().Wait();//инициализация нашей базы
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //ПО занимающиеея отслеживанием ошибок - позволяет отселдить, что не так
                app.UseBrowserLink();
            }

            //app.UseStaticFiles(new StaticFileOptions {ServeUnknownFileTypes = true});//будут подсоединяться любые файлы
            app.UseStaticFiles();//будут подсоединяться файлы стандартного содрежимого, т.е. картинки, html-страницы, css и т.п. 
            app.UseDefaultFiles();
            //app.UseCookiePolicy();//ППО для подстверждения политики куков, особенно в европе
            //app.UseResponseCaching();//кэширование ответа
            //app.UseResponseCompression();//ППО, которое работает после того, как отработал контроллер, берет сформированные данные и пытается их сжать, чтобы уменьшить объем данных на строне клиента

            //app.UseWelcomePage("/welcome");//пример промежуточного ПО

            //app.Run(async context=>await context.Response.WriteAsync("Hello world"));//промежуточное ПО в виде терминального отдельного действия (т.е. является последним и дельнейшее в конвеере не выполняется). метод ран позволяет выполнить некоторое действие над контекстом входящего соединения и, например, отправить ответ

            //app.Map("/Hello", applicattion=> applicattion.Run(async context => await context.Response.WriteAsync("Hello world")));// промежуточное дейстие в виде отдельного действия. Указываем путь и действие, действие к "новому" приложению, а не к app, чтобы не открывалось сразу. Таким образом можно создавать, например, адреса для технических нужд.

            //можно прописать промежуточного ПО в виде отдельного класса - https://metanit.com/sharp/aspnet5/2.2.php

            //app.UseAuthentication();//промежуточное ПО для авторизации. Будет отслеживать заголовки входящих сообщений и искать в них соотвествующие элементы

            //app.UseSession();//ППО для использования сессий в приложении - К этому промежуточному ПО надо добавить его сервисы в контейнер.!!
            //app.UseMvcWithDefaultRoute(); автоматическая конфигурация того, что ниже
            app.UseMvc(routes => // доступ к инфрастуктуре mvc - конфигурирует объект, который добавляется в конвеер, занимается распаковкой данных из запроса, запуском кнтроллеров представлений и отправкой ответов обратно
            {
                routes.MapRoute(name:"default", template:"{controller=Home}/{action=Index}/{id?}"); //?-опциональный параметр, ! - обязательный параметр (как и ничего) , если параметр обязательный, то его отсуттвие в вводе пользователя приведет к ошибке
                // для action  - можно ничего не устанавливать, либо установить значение по умолчанию, в данном случае Index
                // аналогично для конроллера, можно утсановить значение по умолчанию Home, например, когда сервер обращается сам к себе
            }); // новый обработчик
            
            // убираем прежный обработчик, т.к. UseMvc является термиральным и дальше него обработка не идет

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(Configuration["CustomData"]);
            //});
        }
    }
}
