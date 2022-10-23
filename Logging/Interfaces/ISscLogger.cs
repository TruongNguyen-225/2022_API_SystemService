using System;
using Microsoft.Extensions.Logging;

namespace SystemServiceAPICore3.Logging.Interfaces
{
    public interface ISscLogger
    {
        void Log(LogLevel logLevel, Exception exception, string message, params object[] @params);
    }
}

