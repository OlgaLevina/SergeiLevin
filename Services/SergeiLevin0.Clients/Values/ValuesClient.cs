using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Interfaces.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SergeiLevin0.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration config) //сервисардрес обязатльного удалить - иначе будут ошибки!!!
            : base(config, "api/Values") { } //сервич-адрес - адрес айпи-контроллера

        public HttpStatusCode Delete(int id) => DeleteAsync(id).Result;

        public async Task<HttpStatusCode> DeleteAsync(int id) => (await Client.DeleteAsync($"{ServiceAddress}/delete/{id}")).StatusCode;        

        public IEnumerable<string> Get() => GetAsync().Result;//не лучший способ, не можем написать полноценно, т.к. у клиента все метод асинхронные

        public string Get(int id) => GetAsync(id).Result;

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await Client.GetAsync(ServiceAddress);
            if (response.IsSuccessStatusCode)//имеет статус 202
                return await response.Content.ReadAsAsync<List<string>>();
            return Array.Empty<string>();
        }

        public async Task<string> GetAsync(int id)
        {
            var response = await Client.GetAsync($"{ServiceAddress}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<string>();
            return string.Empty;
        }

        public Uri Post(string value) => PostAsync(value).Result;

        public async Task<Uri> PostAsync(string value)
        {
            var response = await Client.PostAsJsonAsync($"{ServiceAddress}/post", value);
            response.EnsureSuccessStatusCode();//проверяет, что статус 202, в противном случае бросаеь исключение
            return response.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value) => PutAsync(id, value).Result;

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await Client.PutAsJsonAsync($"{ServiceAddress}/put/{id}", value);
            return response.EnsureSuccessStatusCode().StatusCode;//выше тоже можно было так записать, а в delete - см. пример записи одной строкой
        }

        //в любом случае вызывая методы клиента, мы не вызываем методы контрллера, а отправляем соответствуюещие запросы http с соотвтствующими глаголами и параметрами (id, value). 
    }
}
