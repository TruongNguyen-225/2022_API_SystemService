using System;
using Microsoft.Extensions.Logging;
using SystemServiceAPICore3.Logging.Interfaces;

namespace SystemServiceAPICore3.Logging
{
    public class SscLogger : ISscLogger
    {
        #region -- Variables --

        private readonly ILogger _logger;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public SscLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Ssc.Core.Logger");
        }

        #endregion

        #region -- Overrides --

        #endregion

        #region -- Methods --

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] @params)
        {
            try
            {
                if (exception != null)
                {
                    _logger?.Log(logLevel, exception, message, @params);
                }
                else
                {
                    _logger?.Log(logLevel, message, @params);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, nameof(SscLogger), null);
            }
        }

        #endregion
    }
}
