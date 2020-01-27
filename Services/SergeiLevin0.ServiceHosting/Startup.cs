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
using Swashbuckle.AspNetCore.Swagger;
using SergeiLevin.Logger;


namespace SergeiLevin0.ServiceHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)=> Configuration = configuration;

        public IConfiguration Configuration { get; }
        /// <summary>
        /// конфигурация приложения
        /// </summary>
        /// <param name="services">интерфейс сервисов</param>
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
            services.AddSwaggerGen(opt=> //система вэб документации, которая автоматически формирует джэйсон-документацию по нашему вэбайпи, а также формирует вэб-интерфэйс, который показывает эту документацию и позволяет тестировать методы вэб-запросы (не только get, но и остальные)
            {
                opt.SwaggerDoc("v1", new Info { Title = "SergeiLevin.API", Version = "v1" });//конфигурация 1й версии документации
                opt.IncludeXmlComments("SergeiLevin0.ServiceHosting.xml");//подключение информации из xml-файла
            }
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SergeiLevinContextInitializer db, ILoggerFactory log)
        {
            log.AddLog4Net();
            db.InitializeAsync().Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(opt => {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "SergeiLevin.API");//метсо размещения файла документации
                opt.RoutePrefix = string.Empty;//прификс доступа к вэб-интерфэйсу - в нашем случае пусто
            });

            app.UseMvc();
        }
    }
}
