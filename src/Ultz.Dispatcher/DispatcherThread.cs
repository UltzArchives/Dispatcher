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
            Queue = new BlockingCollection<Dispatch>();
            Executor = new Thread(Run) {IsBackground = true};
            Executor.Start();
        }

        public BlockingCollection<Dispatch> Queue { get; }
        public Thread Executor { get; }

        private void Run()
        {
            foreach (var task in Queue.GetConsumingEnumerable(_cts.Token))
            {
                task.Result = task.Delegate.DynamicInvoke(task.Arguments);
                task.ResetEvent.Set();
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            Executor.Join();
            Queue?.Dispose();
        }
    }
}
