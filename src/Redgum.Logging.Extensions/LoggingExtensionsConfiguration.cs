using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions
{
    public static class LoggingExtensionsConfiguration
    {
        public static void Configure(LogLevel defaultLogLevel)
        {
            switch (defaultLogLevel)
            {
                case LogLevel.Trace:
                    BlockLoggingExtensions.DefaultBeginBlockMessageAction = BlockLoggingExtensions.BeginBlockTraceAction;
                    BlockLoggingExtensions.DefaultEndBlockMessageAction = BlockLoggingExtensions.EndBlockTraceAction;
                    BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = BlockLoggingExtensions.BeginBlockTypeTraceAction;
                    BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = BlockLoggingExtensions.EndBlockTypeTraceAction;
                    break;
                case LogLevel.Debug:
                    BlockLoggingExtensions.DefaultBeginBlockMessageAction = BlockLoggingExtensions.BeginBlockDebugAction;
                    BlockLoggingExtensions.DefaultEndBlockMessageAction = BlockLoggingExtensions.EndBlockDebugAction;
                    BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = BlockLoggingExtensions.BeginBlockTypeDebugAction;
                    BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = BlockLoggingExtensions.EndBlockTypeDebugAction;
                    break;
                case LogLevel.Information:
                    BlockLoggingExtensions.DefaultBeginBlockMessageAction = BlockLoggingExtensions.BeginBlockInformationAction;
                    BlockLoggingExtensions.DefaultEndBlockMessageAction = BlockLoggingExtensions.EndBlockInformationAction;
                    BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = BlockLoggingExtensions.BeginBlockTypeInformationAction;
                    BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = BlockLoggingExtensions.EndBlockTypeInformationAction;
                    break;
                case LogLevel.Warning:
                    BlockLoggingExtensions.DefaultBeginBlockMessageAction = BlockLoggingExtensions.BeginBlockWarningAction;
                    BlockLoggingExtensions.DefaultEndBlockMessageAction = BlockLoggingExtensions.EndBlockWarningAction;
                    BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = BlockLoggingExtensions.BeginBlockTypeWarningAction;
                    BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = BlockLoggingExtensions.EndBlockTypeWarningAction;
                    break;
                case LogLevel.Error:
                    throw new ArgumentOutOfRangeException($"{nameof(Configure)} can't be called with logLevel: {defaultLogLevel}");
                case LogLevel.Critical:
                    throw new ArgumentOutOfRangeException($"{nameof(Configure)} can't be called with logLevel: {defaultLogLevel}");
                case LogLevel.None:
                    BlockLoggingExtensions.DefaultBeginBlockMessageAction = BlockLoggingExtensions.BeginBlockNoneAction;
                    BlockLoggingExtensions.DefaultEndBlockMessageAction = BlockLoggingExtensions.EndBlockNoneAction;
                    BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = BlockLoggingExtensions.BeginBlockTypeNoneAction;
                    BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = BlockLoggingExtensions.EndBlockTypeNoneAction;
                    break;
                default:
                    break;
            }
        }

        public static void Configure(
            Action<ILogger, string, Exception?> defaultBeginBlockMessageAction,
            Action<ILogger, string, Exception?> defaultEndBlockMessageAction,
            Action<ILogger, string, string, Exception?> defaultBeginBlockTypeMessageAction,
            Action<ILogger, string, string, Exception?> defaultEndBlockTypeMessageAction
            )
        {
            BlockLoggingExtensions.DefaultBeginBlockMessageAction = defaultBeginBlockMessageAction;
            BlockLoggingExtensions.DefaultEndBlockMessageAction = defaultEndBlockMessageAction;
            BlockLoggingExtensions.DefaultBeginBlockTypeMessageAction = defaultBeginBlockTypeMessageAction;
            BlockLoggingExtensions.DefaultEndBlockTypeMessageAction = defaultEndBlockTypeMessageAction;
        }
    }
}