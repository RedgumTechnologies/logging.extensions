using Microsoft.Extensions.Logging;
using Redgum.Logging.Extensions.Result;
using System;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions
{
    /// <summary>
    /// Extensions for logging blocks of code with a Using statement
    /// </summary>
    public static class AutoBlockLoggingExtensions
    {
        /// <summary>
        /// Delegate that can be used to define the function that will occur at the Beginging of a logged Block.
        /// </summary>
        /// <param name="logger">The ILogger</param>
        /// <param name="blockName">The Name of the Block of code to be logged.</param>
        public delegate void BeginBlockMessageDelegate(ILogger logger, string blockName);

        /// <summary>
        /// Delegate that can be used to define the function that will occur at the End of a logged Block.
        /// </summary>
        /// <param name="logger">The ILogger</param>
        /// <param name="blockName">The Name of the Block of code that was logged.</param>
        public delegate void EndBlockMessageDelegate(ILogger logger, string blockName);

        /// <summary>
        /// Delegate that can be used to define the function that will occur at the Begining of a logged Block.
        /// </summary>
        /// <param name="logger">The ILogger</param>
        /// <param name="blockName">The Name of the Block of code that was logged.</param>
        /// <param name="typeName">The type that the Block of code is dealing with</param>
        public delegate void BeginBlockTypeMessageDelegate(ILogger logger, string blockName, string typeName);

        /// <summary>
        /// Delegate that can be used to define the function that will occur at the End of a logged Block.
        /// </summary>
        /// <param name="logger">The ILogger</param>
        /// <param name="blockName">The Name of the Block of code that was logged.</param>
        /// <param name="typeName">The type that the Block of code is dealing with</param>
        public delegate void EndBlockTypeMessageDelegate(ILogger logger, string blockName, string typeName);

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            [CallerMemberName] string blockName = ""
            )
        {
            BlockLoggingExtensions.DefaultBeginBlockMessageAction(logger, blockName, null);
            return new BlockLoggingResult(BlockLoggingExtensions.DefaultEndBlockMessageAction, logger, blockName, null);
        }

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            LogLevel logLevel,
            [CallerMemberName] string blockName = ""
            )
        {
            BlockLoggingExtensions.GetBeginBlockAction(logLevel)(logger, blockName, null);
            return new BlockLoggingResult(BlockLoggingExtensions.GetEndBlockAction(logLevel), logger, blockName, null);
        }

        public static IDisposable BeginAutoBlock(
            this ILogger logger,
            Action<ILogger, string, Exception?> beginBlockMessageAction,
            Action<ILogger, string, Exception?> endBlockMessageAction,
            [CallerMemberName] string blockName = ""
            )
        {
            beginBlockMessageAction(logger, blockName, null);
            return new BlockLoggingResult(endBlockMessageAction, logger, blockName, null);
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
             [CallerMemberName] string blockName = ""
             )
        {
            var typeName = typeof(T).FullName;
            BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction(logger, blockName, typeName, null);
            return new TypeBlockLoggingResult(BlockLoggingExtensions.DefaultEndBlockTypeMessageAction, logger, blockName, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            LogLevel logLevel,
            [CallerMemberName] string blockName = ""
            )
        {
            var typeName = typeof(T).FullName;

            BlockLoggingExtensions.GetBeginBlockTypeAction(logLevel)(logger, blockName, typeName, null);
            return new TypeBlockLoggingResult(BlockLoggingExtensions.GetEndBlockTypeAction(logLevel), logger, blockName, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            Action<ILogger, string, string, Exception?> beginBlockMessageAction,
            Action<ILogger, string, string, Exception?> endBlockMessageAction,
            [CallerMemberName] string blockName = ""
            )
        {
            var typeName = typeof(T).FullName;

            beginBlockMessageAction(logger, blockName, typeName, null);
            return new TypeBlockLoggingResult(endBlockMessageAction, logger, blockName, typeName, null);
        }

        public static IDisposable BeginAutoBlock<T>(
            this ILogger logger,
            BeginBlockTypeMessageDelegate beginBlockMessageDelegate,
            EndBlockTypeMessageDelegate endBlockMessageDelegate,
            [CallerMemberName] string blockName = ""
            )
        {
            var typeName = typeof(T).FullName;
            beginBlockMessageDelegate(logger, blockName, typeName);
            return new TypeBlockLoggingDelegateResult(endBlockMessageDelegate, logger, blockName, typeName);
        }
        public static void Failed(this ILogger logger, Exception ex, [CallerMemberName] string blockName = "")
        {
            BlockLoggingExtensions.FailedAction(logger, blockName, ex);
        }
        public static void Failed(this ILogger logger, Exception ex, Action<ILogger, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, ex);
        }
        public static void Failed<T>(this ILogger logger, Exception ex, [CallerMemberName] string blockName = "")
        {
            BlockLoggingExtensions.FailedTypeAction(logger, blockName, typeof(T).FullName, ex);
        }
        public static void Failed<T>(this ILogger logger, Exception ex, Action<ILogger, string, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, typeof(T).FullName, ex);
        }
    }
}