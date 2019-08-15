using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ultz.Dispatcher.Unsafe
{
    public static unsafe class UnsafeDispatcherExtensions
    {
        public static void* Invoke(this Dispatcher dispatcher, Func<UnsafeDispatch> func)
        {
            var task = new Dispatch{Arguments = new object[0], Delegate = func};
            dispatcher.Thread.Queue.Add(task);
            task.ResetEvent.WaitOne();
            return (UnsafeDispatch)task.Result;
        }
        public static T* Invoke<T>(this Dispatcher dispatcher, Func<UnsafeDispatch<T>> func) where T:unmanaged
        {
            var task = new Dispatch{Arguments = new object[0], Delegate = func};
            dispatcher.Thread.Queue.Add(task);
            task.ResetEvent.WaitOne();
            return (UnsafeDispatch<T>)task.Result;
        }
    }
}
