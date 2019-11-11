using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SergeiLevin0
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Config) => Configuration = Config;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //набиваем сервисами, с которыми в дальнейшем будет работать наше приложение
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //app.UseMvcWithDefaultRoute(); автоматическая конфигурация того, что ниже
            app.UseMvc(routes =>
            {
                routes.MapRoute(name:"default", template:"{controller=Home}/{action=Index}/{id?}"); //?-опциональный параметр, ! - обязательный параметр (как и ничего) , если параметр обязательный, то его отсуттвие в вводе пользователя приведет к ошибке
                // для action  - можно ничего не устанавливать, либо установить значение по умолчанию, в данном случае Index
                // аналогично для конроллера, можно утсановить значение по умолчанию Home, например, когда сервер обращается сам к себе
            }); // новый обработчик
            
            // убираем прежный обработчик, т.к. существует и работает только первый

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(Configuration["CustomData"]);
            //});
        }
    }
}
