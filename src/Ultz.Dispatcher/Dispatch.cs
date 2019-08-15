using System;
using System.Threading;

namespace Ultz.Dispatcher
{
    public class Dispatch
    {
        public Delegate Delegate { get; set; }
        public object[] Arguments { get; set; }
        public object Result { get; set; }
        public AutoResetEvent ResetEvent { get; } = new AutoResetEvent(false);
    }
}