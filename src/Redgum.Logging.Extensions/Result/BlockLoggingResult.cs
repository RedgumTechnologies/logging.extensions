using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions.Result
{
    public struct BlockLoggingResult : IDisposable
    {
        private readonly Action<ILogger, string, Exception?> DisposeAction;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly Exception? Exception1;

        internal BlockLoggingResult(
            Action<ILogger, string, Exception?> disposeAction,
            ILogger logger,
            string str,
            Exception? ex
            )
        {
            disposedValue = false;
            DisposeAction = disposeAction;
            Logger = logger;
            String1 = str;
            Exception1 = ex;
        }

        private bool disposedValue;
        void IDisposable.Dispose()
        {
            if (!disposedValue)
            {
                DisposeAction(Logger, String1, Exception1);
                disposedValue = true;
            }
        }
    }

    //public ref struct BlockLoggingResult
    //{
    //    private readonly Action DisposeAction;

    //    internal BlockLoggingResult(Action disposeAction)
    //    {
    //        disposedValue = false;
    //        DisposeAction = disposeAction;
    //    }

    //    private bool disposedValue;
    //    public void Dispose()
    //    {
    //        if (!disposedValue)
    //        {
    //            DisposeAction();
    //            disposedValue = true;
    //        }
    //    }
    //}

    //public class BlockLoggingResult : IDisposable
    //{
    //    private readonly Action DisposeAction;

    //    public BlockLoggingResult(Action disposeAction)
    //    {
    //        DisposeAction = disposeAction;
    //    }

    //    private bool disposedValue;

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposedValue)
    //        {
    //            if (disposing)
    //            {
    //                DisposeAction();
    //            }

    //            disposedValue = true;
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //        Dispose(disposing: true);
    //        GC.SuppressFinalize(this);
    //    }
    //}
}