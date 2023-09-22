using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions.Justifications
{
    // 
    // |                            Method |      Mean |     Error |    StdDev |
    // |---------------------------------- |----------:|----------:|----------:|
    // | StaticReadOnlyFieldDefinedMessage | 11.154 ns | 0.2199 ns | 0.2057 ns |
    // |      StaticPropertyDefinedMessage |  9.903 ns | 0.1343 ns | 0.1256 ns |
    // |   NonStaticPropertyDefinedMessage |  9.624 ns | 0.0728 ns | 0.0681 ns |
    // 
    // Seems there's no performance penalty for using a Property instead of a ReadOnly Field

    public class MessageDefinitionsBenchmarks
    {
        private ILogger _logger = null!;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = serviceProvider.GetRequiredService<ILogger<MessageDefinitionsBenchmarks>>();

            // Access and use all the fields and properties to 'prime' them - probably unnecessary, but also doesn't hurt

            var staticReadOnlyFieldBeginBlockMessage = StaticReadOnlyFieldBeginBlockMessageAction;
            _logger.LogTrace("BeginBlockInformation type name: {BeginBlockInformationTypeName}", staticReadOnlyFieldBeginBlockMessage.GetType().Name);
            var staticReadOnlyFieldEndBlockMessage = StaticReadOnlyFieldEndBlockMessageAction;
            _logger.LogTrace("EndBlockInformation type name: {BeginBlockInformationTypeName}", staticReadOnlyFieldEndBlockMessage.GetType().Name);
            var staticPropertyBeginBlockMessage = StaticPropertyBeginBlockMessageAction;
            _logger.LogTrace("BeginBlockInformationAction type name: {BeginBlockInformationTypeName}", staticPropertyBeginBlockMessage.GetType().Name);
            var staticPropertyEndBlockMessage = StaticPropertyEndBlockMessageAction;
            _logger.LogTrace("EndBlockInformationAction type name: {BeginBlockInformationTypeName}", staticPropertyEndBlockMessage.GetType().Name);
            var nonStaticPropertyBeginBlockMessage = NonStaticPropertyBeginBlockMessageAction;
            _logger.LogTrace("BeginBlockInformationAction type name: {BeginBlockInformationTypeName}", nonStaticPropertyBeginBlockMessage.GetType().Name);
            var nonStaticPropertyEndBlockMessage = NonStaticPropertyEndBlockMessageAction;
            _logger.LogTrace("EndBlockInformationAction type name: {BeginBlockInformationTypeName}", nonStaticPropertyEndBlockMessage.GetType().Name);
        }
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
        }


        internal static readonly Action<ILogger, string, Exception?> StaticReadOnlyFieldBeginBlockMessageAction =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "BeginBlock"), @"BeginBlock {process}");
        internal static readonly Action<ILogger, string, Exception?> StaticReadOnlyFieldEndBlockMessageAction =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "EndBlock"), @"EndBlock {process}");

        [Benchmark]
        public void StaticReadOnlyFieldDefinedMessage()
        {
            StaticReadOnlyFieldDefinedMessageCallerMemberName();
        }

        public void StaticReadOnlyFieldDefinedMessageCallerMemberName([CallerMemberName] string methodName = "")
        {
            StaticReadOnlyFieldBeginBlockMessageAction(_logger, methodName, null);
            StaticReadOnlyFieldEndBlockMessageAction(_logger, methodName, null);
        }


        internal static Action<ILogger, string, Exception?> StaticPropertyBeginBlockMessageAction { get; set; } = StaticReadOnlyFieldBeginBlockMessageAction;
        internal static Action<ILogger, string, Exception?> StaticPropertyEndBlockMessageAction { get; set; } = StaticReadOnlyFieldBeginBlockMessageAction;

        [Benchmark]
        public void StaticPropertyDefinedMessage()
        {
            StaticPropertyMessageCallerMemberName();
        }

        public void StaticPropertyMessageCallerMemberName([CallerMemberName] string methodName = "")
        {
            StaticPropertyBeginBlockMessageAction(_logger, methodName, null);
            StaticPropertyEndBlockMessageAction(_logger, methodName, null);
        }

        internal Action<ILogger, string, Exception?> NonStaticPropertyBeginBlockMessageAction { get; set; } = StaticReadOnlyFieldBeginBlockMessageAction;
        internal Action<ILogger, string, Exception?> NonStaticPropertyEndBlockMessageAction { get; set; } = StaticReadOnlyFieldBeginBlockMessageAction;

        [Benchmark]
        public void NonStaticPropertyDefinedMessage()
        {
            StaticPropertyMessageCallerMemberName();
        }

        public void NonStaticPropertyMessageCallerMemberName([CallerMemberName] string methodName = "")
        {
            NonStaticPropertyBeginBlockMessageAction(_logger, methodName, null);
            NonStaticPropertyEndBlockMessageAction(_logger, methodName, null);
        }
    }


    //public class Benchmarks
    //{
    //    private ILogger _logger = null!;
    //    private string BlockName = "BenchmarkBlock";

    //    [GlobalSetup]
    //    public void Setup()
    //    {
    //        var serviceCollection = new ServiceCollection();
    //        ConfigureServices(serviceCollection);

    //        var serviceProvider = serviceCollection.BuildServiceProvider();

    //        _logger = serviceProvider.GetRequiredService<ILogger<Benchmarks>>();

    //        // Access and use all the fields and properties to 'prime' them - probably unnecessary, but also doesn't hurt
    //        _logger.LogTrace("BlockName is: {BlockName}", BlockName);

    //        var beginBlockInformation = BeginBlockInformation;
    //        _logger.LogTrace("BeginBlockInformation type name: {BeginBlockInformationTypeName}", beginBlockInformation.GetType().Name);
    //        var endBlockInformation = EndBlockInformation;
    //        _logger.LogTrace("EndBlockInformation type name: {BeginBlockInformationTypeName}", endBlockInformation.GetType().Name);
    //        var beginBlockInformationAction = BeginBlockInformationAction;
    //        _logger.LogTrace("BeginBlockInformationAction type name: {BeginBlockInformationTypeName}", beginBlockInformationAction.GetType().Name);
    //        var endBlockInformationAction = EndBlockInformationAction;
    //        _logger.LogTrace("EndBlockInformationAction type name: {BeginBlockInformationTypeName}", endBlockInformationAction.GetType().Name);

    //    }
    //    static void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddLogging();
    //        services.AddTransient<Benchmarks>();
    //        //ILoggerBlockLoggingExtensions.Configure(defaultLogLevel: LogLevel.Information);
    //    }


    //    [Benchmark]
    //    public void RawLogEntryExitCallerMemberName()
    //    {
    //        RawLogEntryExitCallerMemberName();
    //    }

    //    public void RawLogEntryExitCallerMemberName([CallerMemberName] string methodName = "")
    //    {
    //        _logger.LogInformation("Log entered {methodName}", methodName);
    //        _logger.LogInformation("Log existed {methodName}", methodName);
    //    }

    //    [Benchmark]
    //    public void RawLogEntryExit()
    //    {
    //        _logger.LogInformation("Log entered {methodName}", BlockName);
    //        _logger.LogInformation("Log existed {methodName}", BlockName);
    //    }

    //    internal static readonly Action<ILogger, string, Exception?> BeginBlockInformation =
    //        LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "BeginBlock"), @"BeginBlock {process}");
    //    internal static readonly Action<ILogger, string, Exception?> EndBlockInformation =
    //        LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "EndBlock"), @"EndBlock {process}");

    //    [Benchmark]
    //    public void StaticDefinedMessageCallerMemberName()
    //    {
    //        StaticDefinedMessageCallerMemberName();
    //    }

    //    public void StaticDefinedMessageCallerMemberName([CallerMemberName] string methodName = "")
    //    {
    //        BeginBlockInformation(_logger, methodName, null);
    //        EndBlockInformation(_logger, methodName, null);
    //    }

    //    [Benchmark]
    //    public void StaticDefinedMessage()
    //    {
    //        BeginBlockInformation(_logger, BlockName, null);
    //        EndBlockInformation(_logger, BlockName, null);
    //    }


    //    internal static Action<ILogger, string, Exception?> BeginBlockInformationAction { get; set; } = BeginBlockInformation;
    //    internal static Action<ILogger, string, Exception?> EndBlockInformationAction { get; set; } = BeginBlockInformation;

    //    [Benchmark]
    //    public void StaticPropertyDefinedMessageCallerMemberName()
    //    {
    //        StaticPropertyDefinedMessageCallerMemberName();
    //    }

    //    public void StaticPropertyDefinedMessageCallerMemberName([CallerMemberName] string methodName = "")
    //    {
    //        BeginBlockInformationAction(_logger, methodName, null);
    //        EndBlockInformationAction(_logger, methodName, null);
    //    }

    //    [Benchmark]
    //    public void StaticPropertyDefinedMessage()
    //    {
    //        BeginBlockInformationAction(_logger, BlockName, null);
    //        EndBlockInformationAction(_logger, BlockName, null);
    //    }

    //    //[Benchmark]
    //    //public void ManualBeginBlockExtensionMethodsCallerMemberName([CallerMemberName] string methodName = "")
    //    //{
    //    //    _logger.BeginBlock(methodName);
    //    //    _logger.EndBlock(methodName);
    //    //}

    //    //[Benchmark]
    //    //public void ManualBeginBlockExtensionMethods()
    //    //{
    //    //    _logger.BeginBlock(BlockName);
    //    //    _logger.EndBlock(BlockName);
    //    //}


    //    //[Benchmark]
    //    //public void AutoBeginBlockExtensionMethods_x_N()
    //    //{
    //    //    for (int i = 0; i < LoopCount; i++)
    //    //    {
    //    //        AutoBeginBlockExtensionMethodsItem();
    //    //    }
    //    //}

    //    //public void AutoBeginBlockExtensionMethodsItem()
    //    //{
    //    //    using var log = _logger.BeginAutoBlock();
    //    //}

    //    //[Benchmark]
    //    //public void AutoBeginBlockExtensionMethods()
    //    //{
    //    //    using var log = _logger.BeginAutoBlock();
    //    //}
    //}
}