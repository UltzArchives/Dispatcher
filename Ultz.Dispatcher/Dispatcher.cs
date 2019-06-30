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
            var task = new Task<object>(() => @delegate.DynamicInvoke(args));
            Thread.Queue.Add(task);
            SpinWait.SpinUntil(() => task.IsCompleted);
            return task.Result;
        }

        public void Invoke(Action @delegate)
        {
            var task = new Task(@delegate);
            Thread.Queue.Add(task);
            SpinWait.SpinUntil(() => task.IsCompleted);
        }

        public T Invoke<T>(Func<T> @delegate)
        {
            var task = new Task<T>(@delegate);
            Thread.Queue.Add(task);
            SpinWait.SpinUntil(() => task.IsCompleted);
            return task.Result;
        }

        public async Task<object> InvokeAsync(Delegate @delegate, params object[] args)
        {
            return await Task.Run
            (
                () =>
                {
                    var task = new Task<object>(() => { return @delegate.DynamicInvoke(args); });
                    Thread.Queue.Add(task);
                    SpinWait.SpinUntil(() => task.IsCompleted);
                    return task;
                }
            );
        }

        public async Task InvokeAsync(Action @delegate)
        {
            await Task.Run
            (
                () =>
                {
                    var task = new Task(@delegate);
                    Thread.Queue.Add(task);
                    SpinWait.SpinUntil(() => task.IsCompleted);
                }
            );
        }

        public async Task<T> InvokeAsync<T>(Func<T> @delegate)
        {
            return await Task.Run
            (
                () =>
                {
                    var task = new Task<T>(@delegate);
                    Thread.Queue.Add(task);
                    SpinWait.SpinUntil(() => task.IsCompleted);
                    return task.Result;
                }
            );
        }
        
        public DispatcherThread Thread { get; }

        public void Dispose()
        {
            Thread?.Dispose();
        }
    }
}
