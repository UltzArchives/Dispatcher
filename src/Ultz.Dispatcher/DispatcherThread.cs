using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ultz.Dispatcher
{
    public class DispatcherThread : IDisposable
    {
        private CancellationTokenSource _cts;
        public DispatcherThread()
        {
            _cts = new CancellationTokenSource();
            Queue = new BlockingCollection<Task>();
            Executor = Task.Run(Run);
        }

        public BlockingCollection<Task> Queue { get; }
        public Task Executor { get; }

        private Task Run()
        {
            foreach (var task in Queue.GetConsumingEnumerable(_cts.Token))
            {
                task.RunSynchronously();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts.Cancel();
            Executor.Wait();
            Queue?.Dispose();
            Executor?.Dispose();
        }
    }
}
