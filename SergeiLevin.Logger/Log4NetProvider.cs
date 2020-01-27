using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace SergeiLevin.Logger
{
    public class Log4NetProvider:ILoggerProvider
    {
        private readonly string configurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers = new ConcurrentDictionary<string, Log4NetLogger>(); //для хранения категорий
        public Log4NetProvider(string ConfigurationFile) => configurationFile = ConfigurationFile;

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName,category => 
            {
                var xml = new XmlDocument();
                xml.Load(configurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
