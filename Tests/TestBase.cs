using Pocket;
using System;
using Xunit.Abstractions;

namespace BoardGameGeek.Dungeon
{
    public abstract class TestBase : IDisposable
    {
        protected TestBase(ITestOutputHelper logger)
        {
            Disposables = new CompositeDisposable
            {
                LogEvents.Subscribe(entry =>
                {
                    var (message, _) = entry.Evaluate();
                    logger.WriteLine($"{entry.TimestampUtc.ToLocalTime():HH:mm:ss} {message}");
                })
            };
        }

        public void Dispose() => Disposables.Dispose();

        internal CompositeDisposable Disposables { get; }
    }
}
