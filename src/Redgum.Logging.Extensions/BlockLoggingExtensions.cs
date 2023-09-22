using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions
{
    public static class BlockLoggingExtensions
    {
        internal static Action<ILogger, string, Exception?> BeginBlockTraceAction { get; set; } = StaticMessageDefinitions.BeginBlockTrace;
        internal static Action<ILogger, string, string, Exception?> BeginBlockTypeTraceAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeTrace;
        internal static Action<ILogger, string, Exception?> BeginBlockDebugAction { get; set; } = StaticMessageDefinitions.BeginBlockDebug;
        internal static Action<ILogger, string, string, Exception?> BeginBlockTypeDebugAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeDebug;
        internal static Action<ILogger, string, Exception?> BeginBlockInformationAction { get; set; } = StaticMessageDefinitions.BeginBlockInformation;
        internal static Action<ILogger, string, string, Exception?> BeginBlockTypeInformationAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeInformation;
        internal static Action<ILogger, string, Exception?> BeginBlockWarningAction { get; set; } = StaticMessageDefinitions.BeginBlockWarning;
        internal static Action<ILogger, string, string, Exception?> BeginBlockTypeWarningAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeWarning;
        internal static Action<ILogger, string, Exception?> BeginBlockNoneAction { get; set; } = StaticMessageDefinitions.BeginBlockNone;
        internal static Action<ILogger, string, string, Exception?> BeginBlockTypeNoneAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeNone;

        internal static Action<ILogger, string, Exception?> EndBlockTraceAction { get; set; } = StaticMessageDefinitions.EndBlockTrace;
        internal static Action<ILogger, string, string, Exception?> EndBlockTypeTraceAction { get; set; } = StaticMessageDefinitions.EndBlockTypeTrace;
        internal static Action<ILogger, string, Exception?> EndBlockDebugAction { get; set; } = StaticMessageDefinitions.EndBlockDebug;
        internal static Action<ILogger, string, string, Exception?> EndBlockTypeDebugAction { get; set; } = StaticMessageDefinitions.EndBlockTypeDebug;
        internal static Action<ILogger, string, Exception?> EndBlockInformationAction { get; set; } = StaticMessageDefinitions.EndBlockInformation;
        internal static Action<ILogger, string, string, Exception?> EndBlockTypeInformationAction { get; set; } = StaticMessageDefinitions.EndBlockTypeInformation;
        internal static Action<ILogger, string, Exception?> EndBlockWarningAction { get; set; } = StaticMessageDefinitions.EndBlockWarning;
        internal static Action<ILogger, string, string, Exception?> EndBlockTypeWarningAction { get; set; } = StaticMessageDefinitions.EndBlockTypeWarning;
        internal static Action<ILogger, string, Exception?> EndBlockNoneAction { get; set; } = StaticMessageDefinitions.EndBlockNone;
        internal static Action<ILogger, string, string, Exception?> EndBlockTypeNoneAction { get; set; } = StaticMessageDefinitions.EndBlockTypeNone;

        internal static Action<ILogger, string, Exception?> FailedAction { get; set; } = StaticMessageDefinitions.Failed;
        internal static Action<ILogger, string, string, Exception?> FailedTypeAction { get; set; } = StaticMessageDefinitions.FailedType;



        internal static Action<ILogger, string, Exception?> DefaultBeginBlockMessageAction { get; set; } = StaticMessageDefinitions.BeginBlockTrace;
        internal static Action<ILogger, string, string, Exception?> DefaultBeginBlockTypeMessageAction { get; set; } = StaticMessageDefinitions.BeginBlockTypeDebug;
        internal static Action<ILogger, string, Exception?> DefaultEndBlockMessageAction { get; set; } = StaticMessageDefinitions.EndBlockTrace;
        internal static Action<ILogger, string, string, Exception?> DefaultEndBlockTypeMessageAction { get; set; } = StaticMessageDefinitions.EndBlockTypeTrace;


        public static void BeginBlock(this ILogger logger, Action<ILogger, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, null);
        }

        public static void BeginBlock<T>(this ILogger logger, Action<ILogger, string, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, typeof(T).FullName, null);
        }

        public static void EndBlock(this ILogger logger, Action<ILogger, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, null);
        }

        public static void EndBlock<T>(this ILogger logger, Action<ILogger, string, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, typeof(T).FullName, null);
        }

        public static void Failed(this ILogger logger, Exception ex, Action<ILogger, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, ex);
        }

        public static void Failed<T>(this ILogger logger, Exception ex, Action<ILogger, string, string, Exception?> loggerMessageAction, [CallerMemberName] string blockName = "")
        {
            loggerMessageAction(logger, blockName, typeof(T).FullName, ex);
        }

        /// <summary>
        /// Writes a 'BeginBlock' message to the <see cref="ILogger"/>. <br/>
        /// The Log will be written at a the default log level of <see cref="LogLevel.Trace"/> unless the default log level has been set differently via <seealso cref="Configure"/>
        /// </summary>
        /// <param name="logger">The ILogger to log messages to</param>
        /// <param name="blockName">If not supplied will use the current method name. You usually don't want to supply this parameter.</param>
        public static void BeginBlock(this ILogger logger, [CallerMemberName] string blockName = "")
        {
            DefaultBeginBlockMessageAction(logger, blockName, null);
        }

        public static void BeginBlock(this ILogger logger, LogLevel logLevel, [CallerMemberName] string blockName = "")
        {
            GetBeginBlockAction(logLevel)(logger, blockName, null);
        }

        internal static Action<ILogger, string, Exception?> GetBeginBlockAction(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => BeginBlockTraceAction,
                LogLevel.Debug => BeginBlockDebugAction,
                LogLevel.Information => BeginBlockInformationAction,
                LogLevel.Warning => BeginBlockWarningAction,
                LogLevel.Error => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.Critical => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.None => BeginBlockNoneAction,
                _ => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockAction)} can't be called with logLevel: {logLevel}"),
            };
        }

        /// <summary>
        /// Writes a 'BeginBlock' message with type information to the <see cref="ILogger"/> at a log level of <see cref="LogLevel.Trace"/>
        /// </summary>
        /// <typeparam name="T">The type used to provide the type information that will be included in the 'BeginBlock' message.</typeparam>
        /// <param name="logger">The ILogger to log messages to</param>
        /// <param name="blockName">If not supplied will use the current method name. You usually don't want to supply this parameter.</param>
        public static void BeginBlock<T>(this ILogger logger, [CallerMemberName] string blockName = "")
        {
            BeginBlockTypeTraceAction(logger, blockName, typeof(T).FullName, null);
        }

        public static void BeginBlockType(this ILogger logger, string typeName, [CallerMemberName] string blockName = "")
        {
            BeginBlockTypeTraceAction(logger, blockName, typeName, null);
        }

        public static void BeginBlock<T>(this ILogger logger, LogLevel logLevel, [CallerMemberName] string blockName = "")
        {
            GetBeginBlockTypeAction(logLevel)(logger, blockName, typeof(T).FullName, null);
        }

        internal static Action<ILogger, string, string, Exception?> GetBeginBlockTypeAction(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => BeginBlockTypeTraceAction,
                LogLevel.Debug => BeginBlockTypeDebugAction,
                LogLevel.Information => BeginBlockTypeInformationAction,
                LogLevel.Warning => BeginBlockTypeWarningAction,
                LogLevel.Error => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockTypeAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.Critical => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockTypeAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.None => BeginBlockTypeNoneAction,
                _ => throw new ArgumentOutOfRangeException($"{nameof(GetBeginBlockTypeAction)} can't be called with logLevel: {logLevel}"),
            };
        }

        /// <summary>
        /// Writes a 'EndBlock' message to the <see cref="ILogger"/> at a log level of <see cref="LogLevel.Trace"/>
        /// </summary>
        /// <param name="logger">The ILogger to log messages to</param>
        /// <param name="blockName">If not supplied will use the current method name. You usually don't want to supply this parameter.</param>
        public static void EndBlock(this ILogger logger, [CallerMemberName] string blockName = "")
        {
            EndBlockTraceAction(logger, blockName, null);
        }

        public static void EndBlock(this ILogger logger, LogLevel logLevel, [CallerMemberName] string blockName = "")
        {
            GetEndBlockAction(logLevel)(logger, blockName, null);
        }

        internal static Action<ILogger, string, Exception?> GetEndBlockAction(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => EndBlockTraceAction,
                LogLevel.Debug => EndBlockDebugAction,
                LogLevel.Information => EndBlockInformationAction,
                LogLevel.Warning => EndBlockWarningAction,
                LogLevel.Error => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.Critical => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.None => EndBlockNoneAction,
                _ => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockAction)} can't be called with logLevel: {logLevel}"),
            };
        }

        /// <summary>
        /// Writes a 'EndBlock' message with type information to the <see cref="ILogger"/> at a log level of <see cref="LogLevel.Trace"/>
        /// </summary>
        /// <typeparam name="T">The type used to provide the type information that will be included in the 'EndBlock' message.</typeparam>
        /// <param name="logger">The ILogger to log messages to</param>
        /// <param name="blockName">If not supplied will use the current method name. You usually don't want to supply this parameter.</param>
        public static void EndBlock<T>(this ILogger logger, [CallerMemberName] string blockName = "")
        {
            EndBlockTypeTraceAction(logger, blockName, typeof(T).FullName, null);
        }

        public static void EndBlock<T>(this ILogger logger, LogLevel logLevel, [CallerMemberName] string blockName = "")
        {
            GetEndBlockTypeAction(logLevel)(logger, blockName, typeof(T).FullName, null);
        }

        internal static Action<ILogger, string, string, Exception?> GetEndBlockTypeAction(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => EndBlockTypeTraceAction,
                LogLevel.Debug => EndBlockTypeDebugAction,
                LogLevel.Information => EndBlockTypeInformationAction,
                LogLevel.Warning => EndBlockTypeWarningAction,
                LogLevel.Error => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockTypeAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.Critical => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockTypeAction)} can't be called with logLevel: {logLevel}"),
                LogLevel.None => EndBlockTypeNoneAction,
                _ => throw new ArgumentOutOfRangeException($"{nameof(GetEndBlockTypeAction)} can't be called with logLevel: {logLevel}"),
            };
        }

        public static void Failed(this ILogger logger, Exception ex, [CallerMemberName] string blockName = "")
        {
            FailedAction(logger, blockName, ex);
        }

        public static void Failed<T>(this ILogger logger, Exception ex, [CallerMemberName] string blockName = "")
        {
            FailedTypeAction(logger, blockName, typeof(T).FullName, ex);
        }

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
