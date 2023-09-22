using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions.Result
{
    public struct TypeBlockLoggingResult : IDisposable
    {
        private readonly Action<ILogger, string, string, Exception?> DisposeAction;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly string String2;
        private readonly Exception? Exception1;

        internal TypeBlockLoggingResult(
            Action<ILogger, string, string, Exception?> disposeAction,
            ILogger logger,
            string str1,
            string str2,
            Exception? ex
            )
        {
            disposedValue = false;
            DisposeAction = disposeAction;
            Logger = logger;
            String1 = str1;
            String2 = str2;
            Exception1 = ex;
        }

        private bool disposedValue;
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeAction(Logger, String1, String2, Exception1);
                disposedValue = true;
            }
        }
    }
}