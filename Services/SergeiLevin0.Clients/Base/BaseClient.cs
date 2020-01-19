using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

        public void Dispose() => Dispose(true);
        private bool Disposed;
        protected virtual void Dispose(bool Disposing)
        {
            Disposed = true;
            if (!Disposing || Disposed) return;
            Client.Dispose();//при разрушении клиента, разрушаем и httpклиента
        }
    }
}
