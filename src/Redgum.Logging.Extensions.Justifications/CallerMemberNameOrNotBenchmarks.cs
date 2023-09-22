using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions.Justifications
{
    // |                               Method |     Mean |   Error |  StdDev |
    // |------------------------------------- |---------:|--------:|--------:|
    // |  RawLogEntryExitWithCallerMemberName | 112.8 ns | 0.80 ns | 0.75 ns |
    // |            RawLogEntryExitUsingField | 116.2 ns | 0.81 ns | 0.71 ns |
    // |    RawLogEntryExitUsingReadOnlyField | 114.5 ns | 1.21 ns | 1.13 ns |
    // |         RawLogEntryExitUsingProperty | 113.4 ns | 0.89 ns | 0.83 ns |
    // | RawLogEntryExitUsingReadOnlyProperty | 112.2 ns | 2.22 ns | 2.96 ns |
    //
    // The above numbers show that using [CallerMemberName] is not detrimental at all.
    // This makes sense because the value is substituted in at compile time.

    public class CallerMemberNameOrNotBenchmarks
    {
        private ILogger _logger = null!;
        private string BlockNameField = "BlockNameField";
        private readonly string ReadOnlyBlockNameField = "ReadOnlyBlockNameField";
        private string BlockNameProperty { get; set; } = "BlockNameProperty";
        private string ReadOnlyBlockNameProperty { get; } = "ReadOnlyBlockNameProperty";

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = serviceProvider.GetRequiredService<ILogger<CallerMemberNameOrNotBenchmarks>>();

            // Access and use all the fields and properties to 'prime' them - probably unnecessary, but also doesn't hurt
            BlockNameField = $"{BlockNameField}";
            _logger.LogTrace("BlockNameField is: {BlockName}", BlockNameField);
            _logger.LogTrace("ReadOnlyBlockNameField is: {BlockName}", ReadOnlyBlockNameField);
            _logger.LogTrace("BlockNameProperty is: {BlockName}", BlockNameProperty);
            _logger.LogTrace("ReadOnlyBlockNameProperty is: {BlockName}", ReadOnlyBlockNameProperty);
        }
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            //ILoggerBlockLoggingExtensions.Configure(defaultLogLevel: LogLevel.Information);
        }


        [Benchmark]
        public void RawLogEntryExitWithCallerMemberName()
        {
            RawLogEntryExitCallerMemberName();
        }

        public void RawLogEntryExitCallerMemberName([CallerMemberName] string methodName = "")
        {
            _logger.LogInformation("Log entered {methodName}", methodName);
            _logger.LogInformation("Log existed {methodName}", methodName);
        }

        [Benchmark]
        public void RawLogEntryExitUsingField()
        {
            _logger.LogInformation("Log entered {methodName}", BlockNameField);
            _logger.LogInformation("Log existed {methodName}", BlockNameField);
        }

        [Benchmark]
        public void RawLogEntryExitUsingReadOnlyField()
        {
            _logger.LogInformation("Log entered {methodName}", ReadOnlyBlockNameField);
            _logger.LogInformation("Log existed {methodName}", ReadOnlyBlockNameField);
        }

        [Benchmark]
        public void RawLogEntryExitUsingProperty()
        {
            _logger.LogInformation("Log entered {methodName}", BlockNameProperty);
            _logger.LogInformation("Log existed {methodName}", BlockNameProperty);
        }

        [Benchmark]
        public void RawLogEntryExitUsingReadOnlyProperty()
        {
            _logger.LogInformation("Log entered {methodName}", ReadOnlyBlockNameProperty);
            _logger.LogInformation("Log existed {methodName}", ReadOnlyBlockNameProperty);
        }
    }
}