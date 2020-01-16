using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Services
{
    public class SergeiLevinContextInitializer //его нужно зарегистрировать в сервисах стратапа
    {
        private readonly SergeiLevinContext Db;
        private readonly UserManager<User> UserManager;
        private readonly RoleManager<Role> RoleManager;

        public SergeiLevinContextInitializer(SergeiLevinContext db, UserManager<User> userManager, RoleManager<Role> roleManager)//в методичке иначе - здесь проще
        {
            Db = db;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task InitializeAsync()//инициализатор бд следит за тем, чтобы она существовала и имела данные
        {
            //методы инизиализации базы
            //if (await Db.Database.EnsureDeletedAsync()) { } //проверка на удалимость, система удаляет и возвращает тру, если успешно
            //if (await Db.Database.EnsureCreatedAsync()) { }//проверка на созданность, система автоматически создает бд и возвращает тру, если все прошло успешно
            var db = Db.Database;
            await db.MigrateAsync();//автоматическое содание базы и выполнить все имеющиеся миграции до последней версии
            await IdentityInicialiseAsync();//запуск идентификации
            if (await Db.Products.AnyAsync()) return;//если в продуктах есть что-нибудь, то не инициализируем базу. Можно использовать другой признак для отказа от инициализации
            using (var transaction =await Db.Database.BeginTransactionAsync())
            {
                await Db.Categories.AddRangeAsync(TestData.Categories);//добавляем тестовые данные - если после этого выполнить сохранение данных в базе, то выдаст ошибку - т.к. у нас есть индекс, а его устанавливать имеет право только сама база
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Categories] ON");//создаем ручное управление первичными ключами - отправляем команду в базу, чтобы мы могли менять индексы вместо базы
                //одновременно в ручном режиме управления первичными ключами может находиться только одна таблица
                await Db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Categories] OFF");//выключаем ручное управление первичными ключами - обязательно выключать, чтобы не было проблем с дальнейшми действиями
                transaction.Commit();
            }
            using (var transaction = await Db.Database.BeginTransactionAsync())
            {
                await Db.Brands.AddRangeAsync(TestData.Brands);//добавляем тестовые данные - если после этого выполнить сохранение данных в базе, то выдаст ошибку - т.к. у нас есть индекс, а его устанавливать имеет право только сама база
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");//создаем ручное управление первичными ключами - отправляем команду в базу, чтобы мы могли менять индексы вместо базы
                //одновременно в ручном режиме управления первичными ключами может находиться только одна таблица
                await Db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");//выключаем ручное управление первичными ключами - обязательно выключать, чтобы не было проблем с дальнейшми действиями
                transaction.Commit();
            }
            using (var transaction = await Db.Database.BeginTransactionAsync())
            {
                await Db.Products.AddRangeAsync(TestData.Products);//добавляем тестовые данные - если после этого выполнить сохранение данных в базе, то выдаст ошибку - т.к. у нас есть индекс, а его устанавливать имеет право только сама база
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");//создаем ручное управление первичными ключами - отправляем команду в базу, чтобы мы могли менять индексы вместо базы
                //одновременно в ручном режиме управления первичными ключами может находиться только одна таблица
                await Db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");//выключаем ручное управление первичными ключами - обязательно выключать, чтобы не было проблем с дальнейшми действиями
                transaction.Commit();
            }

        }

        private async Task IdentityInicialiseAsync()
        {
            //if(!await RoleManager.RoleExistsAsync(Role.Administrator)) 
            //{
            //    await RoleManager.CreateAsync(new Role {Name=Role.Administrator });
            //}
            await CheckRoleAsync(Role.Administrator);//если роль отсутствует - создаем
            await CheckRoleAsync(Role.User);
            if (await UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator,
                    //Email="administrarot.com" 
                };
                var creation_result = await UserManager.CreateAsync(admin, User.AdminPasswordDefault);
                if(creation_result.Succeeded)
                    await UserManager.AddToRoleAsync(admin, Role.Administrator);//добавляем, если отсутствует
                else
                    throw new InvalidOperationException($"Ошибка при создании администратора в БД {string.Join(", ", creation_result.Errors.Select(e => e.Description))}");
            }
        }

        private async Task CheckRoleAsync(string role)
        {
            if (!await RoleManager.RoleExistsAsync(role))
            {
                await RoleManager.CreateAsync(new Role { Name = role });
            }
        }

    }
}
