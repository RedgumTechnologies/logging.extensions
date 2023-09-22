﻿using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Redgum.Logging.Extensions.Justifications.Async
{
    // // * Summary *
    // 
    // BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2965/22H2/2022Update)
    // Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
    // .NET SDK=7.0.203
    //   [Host]     : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
    //   DefaultJob : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
    // 
    // 
    // |                                                             Method |     Mean |    Error |   StdDev |
    // |------------------------------------------------------------------- |---------:|---------:|---------:|
    // |            AutoBlockReturnsRefStructResultWithZeroParametersAction | 23.77 ns | 0.265 ns | 0.235 ns |
    // |               AutoBlockReturnsStructResultWithZeroParametersAction | 23.73 ns | 0.321 ns | 0.300 ns |
    // |  AutoBlockReturnsStructResultWithZeroParametersActionAsIDisposable | 29.08 ns | 0.277 ns | 0.259 ns |
    // |                AutoBlockReturnsClassResultWithZeroParametersAction | 28.68 ns | 0.154 ns | 0.121 ns |
    // |   AutoBlockReturnsClassResultWithZeroParametersActionAsIDisposable | 28.55 ns | 0.265 ns | 0.207 ns |
    // |           AutoBlockReturnsRefStructResultWithThreeParametersAction | 16.64 ns | 0.161 ns | 0.142 ns |
    // |              AutoBlockReturnsStructResultWithThreeParametersAction | 18.21 ns | 0.272 ns | 0.241 ns |
    // | AutoBlockReturnsStructResultWithThreeParametersActionAsIDisposable | 29.59 ns | 0.301 ns | 0.267 ns |
    // |               AutoBlockReturnsClassResultWithThreeParametersAction | 22.35 ns | 0.300 ns | 0.266 ns |
    // |  AutoBlockReturnsClassResultWithThreeParametersActionAsIDisposable | 23.17 ns | 0.195 ns | 0.173 ns |
    // 
    // // * Hints *
    // Outliers
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsRefStructResultWithZeroParametersAction: Default            -> 1 outlier  was  removed (27.06 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsClassResultWithZeroParametersAction: Default                -> 3 outliers were removed, 4 outliers were detected (29.83 ns, 31.55 ns..33.15 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsClassResultWithZeroParametersActionAsIDisposable: Default   -> 3 outliers were removed (31.75 ns..32.82 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsRefStructResultWithThreeParametersAction: Default           -> 1 outlier  was  removed (18.75 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsStructResultWithThreeParametersAction: Default              -> 1 outlier  was  removed (20.90 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsStructResultWithThreeParametersActionAsIDisposable: Default -> 1 outlier  was  removed (32.06 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsClassResultWithThreeParametersAction: Default               -> 1 outlier  was  removed (24.69 ns)
    //   ExtensionMethodsBenchmarks.AutoBlockReturnsClassResultWithThreeParametersActionAsIDisposable: Default  -> 1 outlier  was  removed, 2 outliers were detected (24.22 ns, 25.29 ns)
    // 
    // // * Legends *
    //   Mean   : Arithmetic mean of all measurements
    //   Error  : Half of 99.9% confidence interval
    //   StdDev : Standard deviation of all measurements
    //   1 ns   : 1 Nanosecond (0.000000001 sec)
    //
    // The Ref Struct is the quickest (on X64)
    // the ref struct can't be returned as 'real' IDisposable
    // there's not much in it, we're talking about 1.57ns difference
    // that's 0.00000000157 seconds

    public class ExtensionMethodsAsyncBenchmarks
    {
        private ILogger _logger = null!;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = serviceProvider.GetRequiredService<ILogger<ExtensionMethodsAsyncBenchmarks>>();

            // Access and use all the fields and properties to 'prime' them - probably unnecessary, but also doesn't hurt

            var beginBlockMessageAction = ILoggerAutoBlockLoggingExtensions.BeginBlockMessageAction;
            _logger.LogTrace("BeginBlockInformation type name: {BeginBlockInformationTypeName}", beginBlockMessageAction.GetType().Name);
            var endBlockMessageAction = ILoggerAutoBlockLoggingExtensions.EndBlockMessageAction;
            _logger.LogTrace("EndBlockInformation type name: {BeginBlockInformationTypeName}", endBlockMessageAction.GetType().Name);
        }
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
        }

        //// Ref Structs can't be used inside an async method, 
        //// Which pretty much puts them out of the running in terms of being useful for our logging use case
        //[Benchmark]
        //public async Task AutoBlockReturnsRefStructResultWithZeroParametersActionAsync()
        //{
        //    using var log = _logger.AutoBlockReturnsRefStructResultWithZeroParametersAction();
        //    await Task.CompletedTask;
        //}

        //[Benchmark]
        //public async Task AutoBlockReturnsRefStructResultWithZeroParametersActionAsIDisposableAsync()
        //{
        //    using var log = _logger.AutoBlockReturnsRefStructResultWithZeroParametersActionAsIDisposable();
        //    await Task.CompletedTask;
        //}

        [Benchmark]
        public async Task AutoBlockReturnsStructResultWithZeroParametersActionAsync()
        {
            using var log = _logger.AutoBlockReturnsStructResultWithZeroParametersAction();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsStructResultWithZeroParametersActionAsIDisposableAsync()
        {
            using var log = _logger.AutoBlockReturnsStructResultWithZeroParametersActionAsIDisposable();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsClassResultWithZeroParametersActionAsync()
        {
            using var log = _logger.AutoBlockReturnsClassResultWithZeroParametersAction();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsClassResultWithZeroParametersActionAsIDisposableAsync()
        {
            using var log = _logger.AutoBlockReturnsClassResultWithZeroParametersActionAsIDisposable();
            await Task.CompletedTask;
        }


        //// Ref Structs can't be used inside an async method, 
        //// Which pretty much puts them out of the running in terms of being useful for our logging use case
        //[Benchmark]
        //public async Task AutoBlockReturnsRefStructResultWithThreeParametersActionAsync()
        //{
        //    using var log = _logger.AutoBlockReturnsRefStructResultWithThreeParametersAction();
        //    await Task.CompletedTask;
        //}

        //[Benchmark]
        //public async Task AutoBlockReturnsRefStructResultWithThreeParametersActionAsIDisposableAsync()
        //{
        //    using var log = _logger.AutoBlockReturnsRefStructResultWithThreeParametersActionAsIDisposable();
        //    await Task.CompletedTask;
        //}

        [Benchmark]
        public async Task AutoBlockReturnsStructResultWithThreeParametersActionAsync()
        {
            using var log = _logger.AutoBlockReturnsStructResultWithThreeParametersAction();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsStructResultWithThreeParametersActionAsIDisposableAsync()
        {
            using var log = _logger.AutoBlockReturnsStructResultWithThreeParametersActionAsIDisposable();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsClassResultWithThreeParametersActionAsync()
        {
            using var log = _logger.AutoBlockReturnsClassResultWithThreeParametersAction();
            await Task.CompletedTask;
        }

        [Benchmark]
        public async Task AutoBlockReturnsClassResultWithThreeParametersActionAsIDisposableAsync()
        {
            using var log = _logger.AutoBlockReturnsClassResultWithThreeParametersActionAsIDisposable();
            await Task.CompletedTask;
        }
    }

    public static class ILoggerAutoBlockLoggingExtensions
    {
        internal static Action<ILogger, string, Exception?> BeginBlockMessageAction { get; set; } =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "BeginBlock"), @"BeginBlock {process}");
        internal static Action<ILogger, string, Exception?> EndBlockMessageAction { get; set; } =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "EndBlock"), @"EndBlock {process}");


        //// Ref Structs can't be used inside an async method, 
        //// Which pretty much puts them out of the running in terms of being useful for our logging use case
        //public static RefStructResultWithZeroParametersAction AutoBlockReturnsRefStructResultWithZeroParametersAction(
        //    this ILogger logger,
        //    [CallerMemberName] string process = ""
        //    )
        //{
        //    BeginBlockMessageAction(logger, process, null);
        //    return new RefStructResultWithZeroParametersAction(() => EndBlockMessageAction(logger, process, null));
        //}

        //// Ref Structs can't implement interfaces so technically they can't be cast to an IDisposable even though they behave exactly like they've implemented IDisposable
        //public static IDisposable AutoBlockReturnsRefStructResultWithZeroParametersActionAsIDisposable(
        //    this ILogger logger,
        //    [CallerMemberName] string process = ""
        //    )
        //{
        //    BeginBlockMessageAction(logger, process, null);
        //    return new RefStructResultWithZeroParametersAction(() => EndBlockMessageAction(logger, process, null));
        //}

        public static StructResultWithZeroParameterAction AutoBlockReturnsStructResultWithZeroParametersAction(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new StructResultWithZeroParameterAction(() => EndBlockMessageAction(logger, process, null));
        }

        public static IDisposable AutoBlockReturnsStructResultWithZeroParametersActionAsIDisposable(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new StructResultWithZeroParameterAction(() => EndBlockMessageAction(logger, process, null));
        }

        public static ClassResultWithZeroParametersAction AutoBlockReturnsClassResultWithZeroParametersAction(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new ClassResultWithZeroParametersAction(() => EndBlockMessageAction(logger, process, null));
        }

        public static IDisposable AutoBlockReturnsClassResultWithZeroParametersActionAsIDisposable(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new ClassResultWithZeroParametersAction(() => EndBlockMessageAction(logger, process, null));
        }


        public static RefStructResultWithThreeParametersAction AutoBlockReturnsRefStructResultWithThreeParametersAction(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new RefStructResultWithThreeParametersAction(EndBlockMessageAction, logger, process, null);
        }

        //// Ref Structs can't implement interfaces so technically they can't be cast to an IDisposable even though they behave exactly like they've implemented IDisposable
        //public static IDisposable AutoBlockReturnsRefStructResultWithThreeParametersActionAsIDisposable(
        //    this ILogger logger,
        //    [CallerMemberName] string process = ""
        //    )
        //{
        //    BeginBlockMessageAction(logger, process, null);
        //    return new RefStructResultWithThreeParametersAction(EndBlockMessageAction, logger, process, null);
        //}

        public static StructResultWithThreeParameterAction AutoBlockReturnsStructResultWithThreeParametersAction(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new StructResultWithThreeParameterAction(EndBlockMessageAction, logger, process, null);
        }

        public static IDisposable AutoBlockReturnsStructResultWithThreeParametersActionAsIDisposable(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new StructResultWithThreeParameterAction(EndBlockMessageAction, logger, process, null);
        }

        public static ClassResultWithThreeParametersAction AutoBlockReturnsClassResultWithThreeParametersAction(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new ClassResultWithThreeParametersAction(EndBlockMessageAction, logger, process, null);
        }

        public static IDisposable AutoBlockReturnsClassResultWithThreeParametersActionAsIDisposable(
            this ILogger logger,
            [CallerMemberName] string process = ""
            )
        {
            BeginBlockMessageAction(logger, process, null);
            return new ClassResultWithThreeParametersAction(EndBlockMessageAction, logger, process, null);
        }
    }

    //public ref struct RefStructResultWithZeroParametersAction
    //{
    //    private readonly Action DisposeAction;

    //    internal RefStructResultWithZeroParametersAction(Action disposeAction)
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

    public struct StructResultWithZeroParameterAction : IDisposable
    {
        private readonly Action DisposeAction;

        internal StructResultWithZeroParameterAction(Action disposeAction)
        {
            disposedValue = false;
            DisposeAction = disposeAction;
        }

        private bool disposedValue;
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeAction();
                disposedValue = true;
            }
        }
    }

    public class ClassResultWithZeroParametersAction : IDisposable
    {
        private readonly Action DisposeAction;

        public ClassResultWithZeroParametersAction(Action disposeAction)
        {
            DisposeAction = disposeAction;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeAction();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public ref struct RefStructResultWithThreeParametersAction
    {
        private readonly Action<ILogger, string, Exception?> DisposeAction;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly Exception? Exception1;

        internal RefStructResultWithThreeParametersAction(
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
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeAction(Logger, String1, Exception1);
                disposedValue = true;
            }
        }
    }

    public struct StructResultWithThreeParameterAction : IDisposable
    {
        private readonly Action<ILogger, string, Exception?> DisposeAction;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly Exception? Exception1;

        internal StructResultWithThreeParameterAction(
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
        public void Dispose()
        {
            if (!disposedValue)
            {
                DisposeAction(Logger, String1, Exception1);
                disposedValue = true;
            }
        }
    }

    public class ClassResultWithThreeParametersAction : IDisposable
    {
        private readonly Action<ILogger, string, Exception?> DisposeAction;
        private readonly ILogger Logger;
        private readonly string String1;
        private readonly Exception? Exception1;

        internal ClassResultWithThreeParametersAction(
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeAction(Logger, String1, Exception1);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}