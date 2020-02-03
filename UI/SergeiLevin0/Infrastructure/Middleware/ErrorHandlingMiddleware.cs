using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger) {
            next = Next;//делегат последующего конвеера
            logger = Logger;
        }
        public async Task Invoke(HttpContext Context)
        {
            //таким образом можем контролировать вызывать следующий этап конвеера или нет, иметь прямой доступ к контектсу и ответу, который формируется конвеером. Мы можем не вызывать метод некст, а самостоятельно обработать контекст и вернуть результат  
            // например, в случае проблем присваивается Context.Response.StatusCode = 403; и пользователю возвращается запрот на доступ
            //в нашем случае мы обработаем ошибки после конвеера
            try
            {
                await next(Context);
            }
            catch (Exception e)
            {
                HandleExepption(Context, e);
                throw;
            }
        }

        private void HandleExepption(HttpContext Context, Exception Error)//в методичке асинхронный, но нет смысла, т.к. нет асинхронных методов
        {
            logger.LogError(Error, $"Ошибка при обработке запроса {Context.Request.Path}");
        }
    }
}
