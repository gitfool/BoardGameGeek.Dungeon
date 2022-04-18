using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BoardGameGeek.Dungeon
{
    public abstract class TestBase : IDisposable
    {
        protected TestBase(ITestOutputHelper testOutput)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(testOutput)
                .CreateLogger();

            LoggerFactory = new SerilogLoggerFactory(logger);
            Logger = LoggerFactory.CreateLogger(GetType());
            //Logger.LogTrace($">> {GetType().Name}");
        }

        ~TestBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Logger.LogTrace($"<< {GetType().Name}");
        }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
    }
}
