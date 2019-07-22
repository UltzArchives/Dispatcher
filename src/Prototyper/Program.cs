using System;
using System.Threading;
using Ultz.Dispatcher;

namespace Prototyper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " is the entrypoint thread.");
            Console.WriteLine();
            var dispatcher = new Dispatcher();
            dispatcher.Invoke
            (
                () =>
                {
                    Console.Write(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("> This is Bowie to Bowie.");
                }
            );
            dispatcher.Invoke
            (
                () =>
                {
                    Console.Write(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("> Do you hear me out there man?");
                }
            );
            Console.Write(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("> This is Bowie back to Bowie.");
            Console.Write(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("> I can hear you loud and clear man.");
            dispatcher.InvokeAsync
            (
                () =>
                {
                    Console.Write(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("> Oh yeah man!");
                }
            );
            Console.Write(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("> Oh yeah man!");
            Thread.Sleep(1000);
        }
    }
}
