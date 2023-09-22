using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions.Result
{
    public struct TypeBlockLoggingDelegateResult : IDisposable
    {
        private readonly AutoBlockLoggingExtensions.EndBlockTypeMessageDelegate DisposeDelegate;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly string String2;

        internal TypeBlockLoggingDelegateResult(
            AutoBlockLoggingExtensions.EndBlockTypeMessageDelegate disposeDelegate,
            ILogger logger,
            string str,
            string str2
            )
        {
            disposedValue = false;
            DisposeDelegate = disposeDelegate;
            Logger = logger;
            String1 = str;
            String2 = str2;
        }

        private bool disposedValue;
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeDelegate(Logger, String1, String2);
                disposedValue = true;
            }
        }
    }
}