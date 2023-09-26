using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Redgum.Extensions.Logging.Auto.ILoggerExtensions;
//using Redgum.Extensions.Logging.ILoggerExtensions;

using System;
using System.ComponentModel.DataAnnotations;

namespace Redgum.Logging.Extensions.Examples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Auto Logging Example");

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Normal Logging
            // the simplest way to do logging
            logger.LogInformation("A simple information message from within the example executable");

            // A slightly more complex way to do logging that gives you an EventId
            logger.LogInformation(eventId: new EventId(123, "The Event Name"), message: "A more complex way of logging an information message from within the example executable");

            // Another more complex way to do logging that gives you an EventId and a predefined Message
            // Defining the message is slow, but if you define it as a static property and re-use it, that slowness is made up for when it's reused
            var definedMessageAction = LoggerMessage.Define(LogLevel.Information, new EventId(1234, "The Event Name 2"), "Another more complex way of logging an information message from within the example executable");
            definedMessageAction(logger, null);


            var myClass = serviceProvider.GetRequiredService<MyExampleClass>();
            myClass.DefaultLevelDefaultMessage();
            myClass.CustomLevelDefaultMessage();
            myClass.CustomMessages();
            myClass.CustomMessagesUsingLoggerMessageAttribute();
            myClass.CustomBlockName();
            myClass.MultiBlock();
            myClass.LocalFunctionBlock();
            myClass.LocalLambdaExpressionBlock();


            myClass.DefaultLevelDefaultTypeMessage<int>();
            myClass.DefaultLevelDefaultTypeMessage<string>();
            myClass.DefaultLevelDefaultTypeMessage<MyExampleClass>();

            await myClass.AwaitedMessageAsync();
            await myClass.DefaultLevelDefaultMessageUnAwaitedAsync();
            await myClass.AwaitedTwiceMessageAsync();

            logger.LogInformation("Before starting {methodName} as a Task", nameof(myClass.AwaitedTwiceMessageAsync));
            var awaitedTwiceMessageTask = myClass.AwaitedTwiceMessageAsync();
            logger.LogInformation("After starting {methodName} as a Task", nameof(myClass.AwaitedTwiceMessageAsync));
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            logger.LogInformation("After awaiting a Task.Delay() after starting {methodName} as a Task", nameof(myClass.AwaitedTwiceMessageAsync));
            await awaitedTwiceMessageTask;
            logger.LogInformation("After awaiting {methodName} Task", nameof(myClass.AwaitedTwiceMessageAsync));

            var messageUnAwaited = myClass.MessageUnAwaitedAsync();
            var x = await messageUnAwaited;

            logger.LogInformation("Program.DoExample() is finished");


            await LetConsoleLoggingFinishAsync();
        }


        static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging(configure => configure
                    .AddConsole()
                )
                .AddTransient<MyExampleClass>();

            BlockLoggingExtensions.Configure(defaultLogLevel: LogLevel.Warning);
        }

        private static async Task LetConsoleLoggingFinishAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.01));
            await Task.Delay(TimeSpan.FromSeconds(0.01));
        }
    }

    public partial class MyExampleClass
    {
        private readonly ILogger _logger;

        public MyExampleClass(ILogger<MyExampleClass> logger)
        {
            _logger = logger;
        }

        internal void DefaultLevelDefaultMessage()
        {
            using var block = _logger.BeginAutoBlock();

            // do some normal logging to indicate that something happened inside this method
            // normally you would just proceed with regular processing
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(DefaultLevelDefaultMessage));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        internal void CustomLevelDefaultMessage()
        {
            // Start and End of Block will always be logged at whatever LogLevel is supplied
            // this is the slowest option because the correct log message has to be determined for the LogLevel
            using var block = _logger.BeginAutoBlock(LogLevel.Information);
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(CustomLevelDefaultMessage));
            // the block variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        // Defining message actions is relatively expensive so try to only instantiate once if possible
        // by making storage of the Action variable static we ensure that it's only instantiated once for the lifetime of the application rather than once per instance of the containing class
        private static readonly Action<ILogger, string, Exception?> CustomBeginBlockAction =
          LoggerMessage.Define<string>(LogLevel.Information, new EventId(123, "MyCustomBeginBlock"), @"Custom message that indicates this is the beginning of an execution block {blockName}");
        private static readonly Action<ILogger, string, Exception?> CustomEndBlockAction =
          LoggerMessage.Define<string>(LogLevel.Information, new EventId(124, "MyCustomEndBlock"), @"Custom message that indicates this is the ending of an execution block {blockName}");

        internal void CustomMessages()
        {
            using var log = _logger.BeginAutoBlock(beginBlockMessageAction: CustomBeginBlockAction, endBlockMessageAction: CustomEndBlockAction);
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(CustomMessages));
            // the block variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        [LoggerMessage(
            EventId = 123,
            Level = LogLevel.Error,
            Message = "Custom message defined via an attribute and compile-time source generation that indicates this is the beginning of an execution block '{blockName}'"
            )]
        private static partial void CustomBeginBlockMessageUsingAttribute(ILogger logger, string blockName);

        [LoggerMessage(
            EventId = 124,
            Level = LogLevel.Error,
            Message = "Custom message defined via an attribute and compile-time source generation that indicates this is the ending of an execution block '{blockName}'"
            )]
        private static partial void CustomEndBlockMessageUsingAttribute(ILogger logger, string blockName);

        internal void CustomMessagesUsingLoggerMessageAttribute()
        {
            // instead of Actions we can supply methods that matches the Begin and End Delegates
            using var log = _logger.BeginAutoBlock(
                beginBlockMessageDelegate: CustomBeginBlockMessageUsingAttribute,
                endBlockMessageDelegate: CustomEndBlockMessageUsingAttribute
                );
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(CustomMessages));
            // the block variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        internal void CustomBlockName()
        {
            using var block = _logger.BeginAutoBlock("My Custom Block Name Here");

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        internal void MultiBlock()
        {
            using var block1 = _logger.BeginAutoBlock();

            _logger.LogInformation("Manually logged message from outside and before the inner block of {methodName}", nameof(DefaultLevelDefaultMessage));

            using (var block2 = _logger.BeginAutoBlock($"{nameof(DefaultLevelDefaultMessage)} - Inner Block"))
            {

                _logger.LogInformation("Manually logged message from the inner block of {methodName}", nameof(DefaultLevelDefaultMessage));

                // the 'block2' variable will be disposed at the end of this using block
                // which will automatically log an EndBlock Message
            }

            _logger.LogInformation("Manually logged message from outside and after the inner block of {methodName}", nameof(DefaultLevelDefaultMessage));

            // the 'block1' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        public void LocalFunctionBlock()
        {
            using var log = _logger.BeginAutoBlock();
            var x = GetIntegerValue() + 1;

            int GetIntegerValue()
            {
                // this would pick up the name of the outer method, not the local function
                // so the logs will say "Beginning LocalFunctionBlock" twice
                //using var log2 = _logger.BeginAutoBlock();
                // but we can pass our own Block name here instead
                using var log2 = _logger.BeginAutoBlock(blockName: nameof(GetIntegerValue));
                return 1;
            }
        }

        public void LocalLambdaExpressionBlock()
        {
            using var log = _logger.BeginAutoBlock();

            var aLambdaExpression = () =>
            {
                using var log2 = _logger.BeginAutoBlock(blockName: $"{nameof(LocalLambdaExpressionBlock)}_aLambdaExpression");
                return 1;
            };

            var x = aLambdaExpression() + 1;
        }

        internal void DefaultLevelDefaultTypeMessage<T>()
        {
            using var block = _logger.BeginAutoBlock<T>();

            // do some normal logging to indicate that something happened inside this method
            // normally you would just proceed with regular processing
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(DefaultLevelDefaultTypeMessage));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }


        internal async Task AwaitedMessageAsync()
        {
            using var block = _logger.BeginAutoBlock();

            await Task.CompletedTask;
            // do some normal logging to indicate that something happened inside this method
            // normally you would just proceed with regular processing
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(AwaitedMessageAsync));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        internal async Task AwaitedTwiceMessageAsync()
        {
            using var block = _logger.BeginAutoBlock();

            _logger.LogInformation("Before first delay inside {methodName}", nameof(AwaitedTwiceMessageAsync));

            await Task.Delay(TimeSpan.FromSeconds(0.1));

            _logger.LogInformation("Before second delay inside {methodName}", nameof(AwaitedTwiceMessageAsync));

            await Task.Delay(TimeSpan.FromSeconds(0.1));

            _logger.LogInformation("After second delay inside {methodName}", nameof(AwaitedTwiceMessageAsync));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message
        }

        internal Task DefaultLevelDefaultMessageUnAwaitedAsync()
        {
            using var block = _logger.BeginAutoBlock();

            // do some normal logging to indicate that something happened inside this method
            // normally you would just proceed with regular processing
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(DefaultLevelDefaultMessageUnAwaitedAsync));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message

            return Task.CompletedTask;
        }

        internal Task<int> MessageUnAwaitedAsync()
        {
            using var block = _logger.BeginAutoBlock();

            // do some normal logging to indicate that something happened inside this method
            // normally you would just proceed with regular processing
            _logger.LogInformation("Manually logged message from inside {methodName}", nameof(MessageUnAwaitedAsync));

            // the 'block' variable will be disposed when it falls out of scope at the end of the method 
            // which will automatically log an EndBlock Message


            var aLambdaExpression = () =>
            {
                using var log2 = _logger.BeginAutoBlock(blockName: $"{nameof(MessageUnAwaitedAsync)}_aLambdaExpression");
                return 1;
            };

            return Task.FromResult(aLambdaExpression());
        }
    }
}
