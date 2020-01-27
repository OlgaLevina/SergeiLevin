using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Infrastructure.Services;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Services;

namespace SergeiLevin0.ServiceHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)=> Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SergeiLevinContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//здесь можно добавлять к строке подключания данные логина и пароля, чтобы не размещать их в файле конфигурации приложения, здесь же можно менять иные параметры строки подключения
            services.AddTransient<SergeiLevinContextInitializer>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<SergeiLevinContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 3;//устанавливаем требования к данным, например политику паролей (для новый пользоватедей или смены паролей)
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;//требование к уникальности символов в пароле 

                opt.Lockout.AllowedForNewUsers = true;//иначе, новые пользователи автоматически блокируются и только администратор может их разблокировать - подтверждение регистрации администратором
                opt.Lockout.MaxFailedAccessAttempts = 10;//кол-во неудачных попыток ввода данных пароль-логин
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);//тайм-оut блокировки пользователя после максимального количества неудачных попыток ввода данных пароль-логин

                opt.User.AllowedUserNameCharacters = "QWERTYUIOPASDFGHJKLZXCVBNMЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮqwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролджэячсмитьбю1234567890";//список всех доступных символов для имен? Добавить символы
                opt.User.RequireUniqueEmail = false;//отключение уникальности мэйлов (логинов) - важно на этапе отладки

                //просмотреть другие возможности на этапе своего!!!
            });//после регистрации - сконфигурировать сервисы
            services.AddSingleton<IEmpoyeesData, InMemoryEmployeesData>(); 
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IOrderService, SqlOrderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SergeiLevinContextInitializer db)
        {
            db.InitializeAsync().Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
