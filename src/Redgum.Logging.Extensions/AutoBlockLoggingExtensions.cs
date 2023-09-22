using Microsoft.Extensions.Logging;
using Redgum.Logging.Extensions.Result;
using System;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions
{
    public static class AutoBlockLoggingExtensions
    {
        public delegate void BeginBlockMessageDelegate(ILogger logger, string blockName);
        public delegate void EndBlockMessageDelegate(ILogger logger, string blockName);

        public delegate void BeginBlockTypeMessageDelegate(ILogger logger, string blockName, string typeName);
        public delegate void EndBlockTypeMessageDelegate(ILogger logger, string blockName, string typeName);

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BlockLoggingExtensions.DefaultBeginBlockMessageAction(logger, process, null);
            return new BlockLoggingResult(BlockLoggingExtensions.DefaultEndBlockMessageAction, logger, process, null);
        }

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            LogLevel logLevel,
            [CallerMemberName] string process = ""
            )
        {
            BlockLoggingExtensions.GetBeginBlockAction(logLevel)(logger, process, null);
            return new BlockLoggingResult(BlockLoggingExtensions.GetEndBlockAction(logLevel), logger, process, null);
        }

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            Action<ILogger, string, Exception?> beginBlockMessageAction,
            Action<ILogger, string, Exception?> endBlockMessageAction,
            [CallerMemberName] string process = ""
            )
        {
            beginBlockMessageAction(logger, process, null);
            return new BlockLoggingResult(endBlockMessageAction, logger, process, null);
        }

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            BeginBlockMessageDelegate beginBlockMessageDelegate,
            EndBlockMessageDelegate endBlockMessageDelegate,
            [CallerMemberName] string process = ""
            )
        {
            beginBlockMessageDelegate(logger, process);
            return new BlockLoggingDelegateResult(endBlockMessageDelegate, logger, process);
        }

        public static IDisposable BeginAutoBlock<T>(
             this ILogger logger,
             [CallerMemberName] string process = ""
             )
        {
            var typeName = typeof(T).FullName;
            BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction(logger, process, typeName, null);
            return new TypeBlockLoggingResult(BlockLoggingExtensions.DefaultEndBlockTypeMessageAction, logger, process, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            LogLevel logLevel,
            [CallerMemberName] string process = ""
            )
        {
            var typeName = typeof(T).FullName;

            BlockLoggingExtensions.GetBeginBlockTypeAction(logLevel)(logger, process, typeName, null);
            return new TypeBlockLoggingResult(BlockLoggingExtensions.GetEndBlockTypeAction(logLevel), logger, process, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            Action<ILogger, string, string, Exception?> beginBlockMessageAction,
            Action<ILogger, string, string, Exception?> endBlockMessageAction,
            [CallerMemberName] string process = ""
            )
        {
            var typeName = typeof(T).FullName;

            beginBlockMessageAction(logger, process, typeName, null);
            return new TypeBlockLoggingResult(endBlockMessageAction, logger, process, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            BeginBlockTypeMessageDelegate beginBlockMessageDelegate,
            EndBlockTypeMessageDelegate endBlockMessageDelegate,
            [CallerMemberName] string process = ""
            )
        {
            var typeName = typeof(T).FullName;
            beginBlockMessageDelegate(logger, process, typeName);
            return new TypeBlockLoggingDelegateResult(endBlockMessageDelegate, logger, process, typeName);
        }
        public static void Failed(this ILogger logger, Exception ex, [CallerMemberName] string process = "")
        {
            BlockLoggingExtensions.FailedAction(logger, process, ex);
        }
        public static void Failed(this ILogger logger, Exception ex, Action<ILogger, string, Exception?> loggerMessageAction, [CallerMemberName] string process = "")
        {
            loggerMessageAction(logger, process, ex);
        }
        public static void Failed<T>(this ILogger logger, Exception ex, [CallerMemberName] string process = "")
        {
            BlockLoggingExtensions.FailedTypeAction(logger, process, typeof(T).FullName, ex);
        }
        public static void Failed<T>(this ILogger logger, Exception ex, Action<ILogger, string, string, Exception?> loggerMessageAction, [CallerMemberName] string process = "")
        {
            loggerMessageAction(logger, process, typeof(T).FullName, ex);
        }
    }
}