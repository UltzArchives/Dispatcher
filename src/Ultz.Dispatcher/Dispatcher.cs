using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ultz.Dispatcher
{
    public class Dispatcher : IDisposable
    {
        public Dispatcher() : this(new DispatcherThread())
        {
        }

        public Dispatcher(DispatcherThread underlyingThread)
        {
            Thread = underlyingThread;
        }

        public object Invoke(Delegate @delegate, params object[] args)
        {
            var task = new Dispatch {Arguments = args, Delegate = @delegate};
            Thread.Queue.Add(task);
            task.ResetEvent.WaitOne();
            return task.Result;
        }

        public void Invoke(Action @delegate)
        {
            var task = new Dispatch {Arguments = new object[0], Delegate = @delegate};
            task.ResetEvent.WaitOne();
        }

        public T Invoke<T>(Func<T> @delegate)
        {
            var task = new Dispatch {Arguments = new object[0], Delegate = @delegate};
            Thread.Queue.Add(task);
            task.ResetEvent.WaitOne();
            return (T) task.Result;
        }

        public async Task<object> InvokeAsync(Delegate @delegate, params object[] args)
        {
            return await Task.Run(() => Invoke(@delegate, args));
        }

        public async Task InvokeAsync(Action @delegate)
        {
            await Task.Run(() => Invoke(@delegate));
        }

        public async Task<T> InvokeAsync<T>(Func<T> @delegate)
        {
            return await Task.Run(() => Invoke(@delegate));
        }
        
        public DispatcherThread Thread { get; }

        public void Dispose()
        {
            Thread?.Dispose();
        }
    }
}
