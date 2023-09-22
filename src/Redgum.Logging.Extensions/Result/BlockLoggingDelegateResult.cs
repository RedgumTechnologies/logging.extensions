using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions.Result
{
    public struct BlockLoggingDelegateResult : IDisposable
    {
        private readonly AutoBlockLoggingExtensions.EndBlockMessageDelegate DisposeDelegate;
        private readonly ILogger Logger;
        private readonly string String1;

        internal BlockLoggingDelegateResult(
            AutoBlockLoggingExtensions.EndBlockMessageDelegate disposeDelegate,
            ILogger logger,
            string str
            )
        {
            disposedValue = false;
            DisposeDelegate = disposeDelegate;
            Logger = logger;
            String1 = str;
        }

        private bool disposedValue;
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeDelegate(Logger, String1);
                disposedValue = true;
            }
        }
    }
}