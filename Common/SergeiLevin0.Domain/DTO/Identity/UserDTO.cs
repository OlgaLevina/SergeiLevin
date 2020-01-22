using Microsoft.AspNetCore.Identity;
using SergeiLevin0.Domain.Entities.Identity;
using System;
using System.Security.Claims;
using System.Text;

namespace SergeiLevin0.Domain.DTO.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }

    public class AddLoginDTO: UserDTO //каждый раз, когда система айдентити регистрирует корректный вход в систему (ввели логин и пароль, а затем с помощью  юзерсайнменеджера залогинились в систему) данный класс отправляется на сторону вэбайпи и содержать информацию по юзеру (юзердто) и юзерлогининфо (структура, содержащая параметры вошедшего пользователя, в т.ч. провайдер)
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDTO: UserDTO //модель для хранения хешьсуммы
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDTO: UserDTO //для установления даты окончания блокировки; если она равна No - то блокировка бессрочная - используется для забанивания пользователя
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }

}
