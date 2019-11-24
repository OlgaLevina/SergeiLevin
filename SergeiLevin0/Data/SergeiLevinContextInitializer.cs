using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Data
{
    public class SergeiLevinContextInitializer //его нужно зарегистрировать в сервисах стратапа
    {
        private readonly SergeiLevinContext Db;

        public SergeiLevinContextInitializer(SergeiLevinContext db) => Db = db;

        public async Task InitializeAsync()//инициализатор бд следит за тем, чтобы она существовала и имела данные
        {
            //методы инизиализации базы
            //if (await Db.Database.EnsureDeletedAsync()) { } //проверка на удалимость, система удаляет и возвращает тру, если успешно
            //if (await Db.Database.EnsureCreatedAsync()) { }//проверка на созданность, система автоматически создает бд и возвращает тру, если все прошло успешно
            var db = Db.Database;
            await db.MigrateAsync();//автоматическое содание базы и выполнить все имеющиеся миграции до последней версии
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
    }
}
