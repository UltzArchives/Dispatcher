using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ultz.Dispatcher.Unsafe
{
    public static unsafe class UnsafeDispatcherExtensions
    {
        public static void* Invoke(this Dispatcher dispatcher, Func<UnsafeDispatch> func)
        {
            var task = new Task<UnsafeDispatch>(func);
            dispatcher.Thread.Queue.Add(task);
            SpinWait.SpinUntil(() => task.IsCompleted);
            return (void*)task.Result;
        }
        public static T* Invoke<T>(this Dispatcher dispatcher, Func<UnsafeDispatch<T>> func) where T:unmanaged
        {
            var task = new Task<UnsafeDispatch<T>>(func);
            dispatcher.Thread.Queue.Add(task);
            SpinWait.SpinUntil(() => task.IsCompleted);
            return (T*)task.Result;
        }
    }
}
