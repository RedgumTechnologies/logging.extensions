using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>(args: args);
            Console.WriteLine(summary);
        }
    }

    public class Benchmarks
    {
        private ILogger _logger = null!;
        private string BlockNameField = "BenchmarkBlock";
        private int LoopCount = 100;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = serviceProvider.GetRequiredService<ILogger<Benchmarks>>();

            LoopCount = 100;

            // Access and use all the fields and properties to 'prime' them - probably unnecessary, but also doesn't hurt
            BlockNameField = $"{BlockNameField}";
            _logger.LogTrace("BlockName is: {BlockName}", BlockNameField);

            var beginBlockInformation = BeginBlockInformation;
            _logger.LogTrace("BeginBlockInformation type name: {BeginBlockInformationTypeName}", beginBlockInformation.GetType().Name);
            var endBlockInformation = EndBlockInformation;
            _logger.LogTrace("EndBlockInformation type name: {BeginBlockInformationTypeName}", endBlockInformation.GetType().Name);
            var beginBlockInformationAction = BeginBlockInformationAction;
            _logger.LogTrace("BeginBlockInformationAction type name: {BeginBlockInformationTypeName}", beginBlockInformationAction.GetType().Name);
            var endBlockInformationAction = EndBlockInformationAction;
            _logger.LogTrace("EndBlockInformationAction type name: {BeginBlockInformationTypeName}", endBlockInformationAction.GetType().Name);

        }
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddTransient<Benchmarks>();
            BlockLoggingExtensions.Configure(defaultLogLevel: LogLevel.Information);
        }


        [Benchmark]
        public void RawLogEntryExit_x_N()
        {
            for (var i = 0; i < LoopCount; i++)
            {
                RawLogEntryExitItem();
            }
        }
        public void RawLogEntryExitItem([CallerMemberName] string methodName = "")
        {
            _logger.LogInformation("Log entered {methodName}", methodName);
            _logger.LogInformation("Log existed {methodName}", methodName);
        }

        [Benchmark]
        public void RawLogEntryExit()
        {
            _logger.LogInformation("Log entered {methodName}", BlockNameField);
            _logger.LogInformation("Log existed {methodName}", BlockNameField);
        }

        internal static readonly Action<ILogger, string, Exception?> BeginBlockInformation =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "BeginBlock"), @"BeginBlock {process}");
        internal static readonly Action<ILogger, string, Exception?> EndBlockInformation =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "EndBlock"), @"EndBlock {process}");

        [Benchmark]
        public void StaticDefinedMessage_x_N()
        {
            for (var i = 0; i < LoopCount; i++)
            {
                StaticDefinedMessageItem();
            }
        }

        public void StaticDefinedMessageItem([CallerMemberName] string methodName = "")
        {
            BeginBlockInformation(_logger, methodName, null);
            EndBlockInformation(_logger, methodName, null);
        }

        [Benchmark]
        public void StaticDefinedMessage()
        {
            BeginBlockInformation(_logger, BlockNameField, null);
            EndBlockInformation(_logger, BlockNameField, null);
        }


        internal static Action<ILogger, string, Exception?> BeginBlockInformationAction { get; set; } = BeginBlockInformation;
        internal static Action<ILogger, string, Exception?> EndBlockInformationAction { get; set; } = BeginBlockInformation;

        [Benchmark]
        public void StaticPropertyDefinedMessage_x_N()
        {
            for (var i = 0; i < LoopCount; i++)
            {
                StaticPropertyDefinedMessageItem();
            }
        }

        public void StaticPropertyDefinedMessageItem([CallerMemberName] string methodName = "")
        {
            BeginBlockInformationAction(_logger, methodName, null);
            EndBlockInformationAction(_logger, methodName, null);
        }

        [Benchmark]
        public void StaticPropertyDefinedMessage()
        {
            BeginBlockInformationAction(_logger, BlockNameField, null);
            EndBlockInformationAction(_logger, BlockNameField, null);
        }

        [Benchmark]
        public void ManualBeginBlockExtensionMethods_x_N()
        {
            for (var i = 0; i < LoopCount; i++)
            {
                ManualBeginBlockExtensionMethodsItem();
            }
        }

        public void ManualBeginBlockExtensionMethodsItem([CallerMemberName] string methodName = "")
        {
            _logger.BeginBlock(methodName);
            _logger.EndBlock(methodName);
        }

        [Benchmark]
        public void ManualBeginBlockExtensionMethods()
        {
            _logger.BeginBlock(BlockNameField);
            _logger.EndBlock(BlockNameField);
        }


        [Benchmark]
        public void AutoBeginBlockExtensionMethods_x_N()
        {
            for (var i = 0; i < LoopCount; i++)
            {
                AutoBeginBlockExtensionMethodsItem();
            }
        }
        
        public void AutoBeginBlockExtensionMethodsItem()
        {
            using var log = _logger.BeginAutoBlock();
        }

        [Benchmark]
        public void AutoBeginBlockExtensionMethods()
        {
            using var log = _logger.BeginAutoBlock();
        }
    }

    //public class BenchmarkOne
    //{
    //    private readonly ILogger _logger;

    //    public Benchmarks(ILogger<Benchmarks> logger)
    //    {
    //        _logger = logger;
    //    }

    //    [Benchmark]
    //    public void Method1()
    //    {
    //        using var block = _logger.BeginAutoBlock();
    //        _logger.LogInformation("Hello from inside Method 1");
    //        // block will be disposed when it falls out of scope at the end of the method 
    //        // which will automatically log an EndBlock
    //    }

    //    [Benchmark]
    //    public void Method2()
    //    {
    //        using var log = _logger.BeginAutoBlock();
    //        _logger.LogInformation("Hello from inside Method 2");
    //    }

    //    public void DoLog()
    //    {

    //    }
    //}
}