using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SergeiLevin0.Interfaces.Api
{
    public interface IValuesService//методы интерфэйса должы соответствовать методам в самом контроллере, для которого ваяем интерфэйс
    {
        IEnumerable<string> Get(); // получить все
        Task<IEnumerable<string>> GetAsync();
        string Get(int id);
        Task<string> GetAsync(int id); //получить по айди
        Uri Post(string value);//отправить элемент на сервер и получить обратно адрес элемента внутри этого сервиса
        Task<Uri> PostAsync(string value);
        HttpStatusCode Put(int id, string value);//изменить значения
        Task<HttpStatusCode> PutAsync(int id, string value);
        HttpStatusCode Delete(int id);
        Task<HttpStatusCode> DeleteAsync(int id);
    }
}
