using Microsoft.Extensions.Logging;
using System;

namespace Redgum.Logging.Extensions
{
    //public interface IMessageDefinitions
    //{
    //    Action<ILogger, string, Exception?> Failed { get; }
    //    Action<ILogger, string, string?, Exception?> FailedType { get; }
    //    Action<ILogger, string, Exception?> EndBlockDebug { get; }
    //    Action<ILogger, string, Exception?> EndBlockInformation { get; }
    //    Action<ILogger, string, Exception?> EndBlockTrace { get; }
    //    Action<ILogger, string, string?, Exception?> EndBlockTypeDebug { get; }
    //    Action<ILogger, string, string?, Exception?> EndBlockTypeInformation { get; }
    //    Action<ILogger, string, string?, Exception?> EndBlockTypeTrace { get; }
    //    Action<ILogger, string, string?, Exception?> EndBlockTypeWarning { get; }
    //    Action<ILogger, string, Exception?> EndBlockWarning { get; }
    //    Action<ILogger, string, Exception?> BeginBlockDebug { get; }
    //    Action<ILogger, string, Exception?> BeginBlockInformation { get; }
    //    Action<ILogger, string, Exception?> BeginBlockTrace { get; }
    //    Action<ILogger, string, string?, Exception?> BeginBlockTypeDebug { get; }
    //    Action<ILogger, string, string?, Exception?> BeginBlockTypeInformation { get; }
    //    Action<ILogger, string, string?, Exception?> BeginBlockTypeTrace { get; }
    //    Action<ILogger, string, string?, Exception?> BeginBlockTypeWarning { get; }
    //    Action<ILogger, string, Exception?> BeginBlockWarning { get; }
    //}

    //public class DefaultMessageDefinitions : IMessageDefinitions
    //{
    //    public Action<ILogger, string, Exception?> BeginBlockTrace => StaticMessageDefinitions.BeginBlockTrace;
    //    public Action<ILogger, string, string?, Exception?> BeginBlockTypeTrace => StaticMessageDefinitions.BeginBlockTypeTrace;
    //    public Action<ILogger, string, Exception?> BeginBlockDebug => StaticMessageDefinitions.BeginBlockDebug;
    //    public Action<ILogger, string, string?, Exception?> BeginBlockTypeDebug => StaticMessageDefinitions.BeginBlockTypeDebug;
    //    public Action<ILogger, string, Exception?> BeginBlockInformation => StaticMessageDefinitions.BeginBlockInformation;
    //    public Action<ILogger, string, string?, Exception?> BeginBlockTypeInformation => StaticMessageDefinitions.BeginBlockTypeInformation;
    //    public Action<ILogger, string, Exception?> BeginBlockWarning => StaticMessageDefinitions.BeginBlockWarning;
    //    public Action<ILogger, string, string?, Exception?> BeginBlockTypeWarning => StaticMessageDefinitions.BeginBlockTypeWarning;

    //    public Action<ILogger, string, Exception?> EndBlockTrace => StaticMessageDefinitions.EndBlockTrace;
    //    public Action<ILogger, string, string?, Exception?> EndBlockTypeTrace => StaticMessageDefinitions.EndBlockTypeTrace;
    //    public Action<ILogger, string, Exception?> EndBlockDebug => StaticMessageDefinitions.EndBlockDebug;
    //    public Action<ILogger, string, string?, Exception?> EndBlockTypeDebug => StaticMessageDefinitions.EndBlockTypeDebug;
    //    public Action<ILogger, string, Exception?> EndBlockInformation => StaticMessageDefinitions.EndBlockInformation;
    //    public Action<ILogger, string, string?, Exception?> EndBlockTypeInformation => StaticMessageDefinitions.EndBlockTypeInformation;
    //    public Action<ILogger, string, Exception?> EndBlockWarning => StaticMessageDefinitions.EndBlockWarning;
    //    public Action<ILogger, string, string?, Exception?> EndBlockTypeWarning => StaticMessageDefinitions.EndBlockTypeWarning;

    //    public Action<ILogger, string, Exception?> Failed => StaticMessageDefinitions.Failed;
    //    public Action<ILogger, string, string?, Exception?> FailedType => StaticMessageDefinitions.FailedType;
    //}
    public static class StaticMessageDefinitions
    {
        internal static readonly Action<ILogger, string, Exception?> BeginBlockNone =
          LoggerMessage.Define<string>(LogLevel.None, new EventId(1, "BeginBlock"), "BeginBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> BeginBlockTypeNone =
          LoggerMessage.Define<string, string?>(LogLevel.None, new EventId(2, "BeginBlock<T>"), "BeginBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> BeginBlockTrace =
          LoggerMessage.Define<string>(LogLevel.Trace, new EventId(1, "BeginBlock"), "BeginBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> BeginBlockTypeTrace =
          LoggerMessage.Define<string, string?>(LogLevel.Trace, new EventId(2, "BeginBlock<T>"), "BeginBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> BeginBlockDebug =
          LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "BeginBlock"), "BeginBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> BeginBlockTypeDebug =
          LoggerMessage.Define<string, string?>(LogLevel.Debug, new EventId(2, "BeginBlock<T>"), "BeginBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> BeginBlockInformation =
          LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "BeginBlock"), "BeginBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> BeginBlockTypeInformation =
          LoggerMessage.Define<string, string?>(LogLevel.Information, new EventId(2, "BeginBlock<T>"), "BeginBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> BeginBlockWarning =
          LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1, "BeginBlock"), "BeginBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> BeginBlockTypeWarning =
          LoggerMessage.Define<string, string?>(LogLevel.Warning, new EventId(2, "BeginBlock<T>"), "BeginBlock {blockName} type: {type}");


        internal static readonly Action<ILogger, string, Exception?> EndBlockNone =
            LoggerMessage.Define<string>(LogLevel.None, new EventId(3, "EndBlock"), "EndBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> EndBlockTypeNone =
            LoggerMessage.Define<string, string?>(LogLevel.None, new EventId(4, "EndBlock<T>"), "EndBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> EndBlockTrace =
            LoggerMessage.Define<string>(LogLevel.Trace, new EventId(3, "EndBlock"), "EndBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> EndBlockTypeTrace =
            LoggerMessage.Define<string, string?>(LogLevel.Trace, new EventId(4, "EndBlock<T>"), "EndBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> EndBlockDebug =
            LoggerMessage.Define<string>(LogLevel.Debug, new EventId(3, "EndBlock"), "EndBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> EndBlockTypeDebug =
            LoggerMessage.Define<string, string?>(LogLevel.Debug, new EventId(4, "EndBlock<T>"), "EndBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> EndBlockInformation =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "EndBlock"), "EndBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> EndBlockTypeInformation =
            LoggerMessage.Define<string, string?>(LogLevel.Information, new EventId(4, "EndBlock<T>"), "EndBlock {blockName} type: {type}");
        internal static readonly Action<ILogger, string, Exception?> EndBlockWarning =
            LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3, "EndBlock"), "EndBlock {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> EndBlockTypeWarning =
            LoggerMessage.Define<string, string?>(LogLevel.Warning, new EventId(4, "EndBlock<T>"), "EndBlock {blockName} type: {type}");

        internal static readonly Action<ILogger, string, Exception?> Failed =
            LoggerMessage.Define<string>(LogLevel.Error, new EventId(5, "Failed"), "Failed to {blockName}");
        internal static readonly Action<ILogger, string, string, Exception?> FailedType =
            LoggerMessage.Define<string, string?>(LogLevel.Error, new EventId(6, "Failed<T>"), "Failed to {blockName} type: {type}");

    }
}
