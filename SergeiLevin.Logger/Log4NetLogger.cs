using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace SergeiLevin.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog log;
        public Log4NetLogger(string CategoryName, XmlElement Configuration)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            log = LogManager.GetLogger(logger_repository.Name, CategoryName);
            log4net.Config.XmlConfigurator.Configure(logger_repository, Configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel Level)//позволяет проверить включен ли логгер для указанного уровня ведения журнала
        {
            switch (Level)
            {
                default: throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
                case LogLevel.Trace: 
                case LogLevel.Debug: return log.IsDebugEnabled;
                case LogLevel.Information:return log.IsInfoEnabled;
                case LogLevel.Warning:return log.IsWarnEnabled;
                case LogLevel.Error:return log.IsErrorEnabled;
                case LogLevel.Critical:return log.IsFatalEnabled;
                case LogLevel.None:return false;
            };
        }

        public void Log<TState>(LogLevel Level, EventId EventId, TState State, Exception Exception, Func<TState, Exception, string> Formatter)
        {
            if (Formatter is null) throw new ArgumentNullException(nameof(Formatter));
            if(!IsEnabled(Level)) return;
            var log_message = Formatter(State, Exception);
            if (string.IsNullOrEmpty(log_message) && Exception is null) return;
            switch (Level)
            {
                default: throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
                case LogLevel.Trace:
                case LogLevel.Debug:log.Debug(log_message);break;
                case LogLevel.Information:log.Info(log_message); break;
                case LogLevel.Warning:log.Warn(log_message); break;
                case LogLevel.Error: log.Error(log_message ?? Exception.ToString()); break;
                case LogLevel.Critical: log.Fatal(log_message ?? Exception.ToString()); break;
                case LogLevel.None:break;
            }
        }
    }
}
