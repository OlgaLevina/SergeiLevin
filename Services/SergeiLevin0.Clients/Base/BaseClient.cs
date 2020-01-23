using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SergeiLevin0.Clients.Base
{
    public abstract class BaseClient : IDisposable

    {
        protected readonly string ServiceAddress;
        protected readonly HttpClient Client;

        protected BaseClient(IConfiguration config, string serviceAddress)// конфигарация - по сути джэйсон файл основного приложения, из которого мы должны достасть адрес клиента; сервисадресс - адрес конкретного контроллера внутри этого хостинга 
        {
            ServiceAddress = serviceAddress;
            Client = new HttpClient {BaseAddress = new Uri(config["ClientAddress"])};
            //далее настройка клиента, чтобы он отправляя свой запрос на сторону httpхостинга сразу в заголовках этого запроса указывал подтип данных, которые его интересуют.
            var headers = Client.DefaultRequestHeaders.Accept;
            headers.Clear();
            headers.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<T> GetAsync<T> (string url, CancellationToken Cancel=default) where T: new()//предусматриваем возможность отмены асинхронной операции через кансел
        {
            var response = await Client.GetAsync(url, Cancel);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<T>(Cancel);
            return new T();
        }
        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default) 
        {
            var response = await Client.PostAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;// в уроке видимо ошибка

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await Client.PutAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default) => await Client.DeleteAsync(url, Cancel);
        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

                     
        #region IDisposable

        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        private bool Disposed;
        protected virtual void Dispose(bool Disposing)
        {
            Disposed = true;
            if (!Disposing || Disposed) return;
            Client.Dispose();//при разрушении клиента, разрушаем и httpклиента
        }
        #endregion
    }
}
