using Microsoft.AspNetCore.Identity;
using SergeiLevin0.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Interfaces.Services
{
    public interface IUsersClient: //наследует от стандартных интерфэйсов. Можно было бы сделать под каждй из них свой наш интерфэйс и контроллер, но так компактнее
        IUserRoleStore<User>, //хранилище ролей
        IUserPasswordStore<User>, //..паролей
        IUserEmailStore<User>,//..имэйлов
        IUserPhoneNumberStore<User>,//..телефонов
        IUserClaimStore<User>,//..утреждений
        IUserTwoFactorStore<User>,//.. информации о двухфакторной авторизации
        IUserLockoutStore<User>,//..блокировок
        IUserLoginStore<User>//..информацию по входам пользователя в систему
    {
    }
}
