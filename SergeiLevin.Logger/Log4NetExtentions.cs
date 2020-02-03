using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace SergeiLevin.Logger
{
    public static class Log4NetExtentions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string ConfigurationFile="log4net.config")
        {
            if(!Path.IsPathRooted(ConfigurationFile))//чтобы в случае неотносительного пути, система могла его найти
            {
                var assambly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Не удалось определить сборку с точкой входа в приложение");
                var dir = Path.GetDirectoryName(assambly.Location) ?? throw new InvalidOperationException("Не удалось определить каталог исполнительного файла");
                ConfigurationFile = Path.Combine(dir, ConfigurationFile);
            }
            factory.AddProvider(new Log4NetProvider(ConfigurationFile));//регистрируем наш собственный провайдер

            return factory;
        }
    }
}
