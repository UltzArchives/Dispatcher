namespace Ultz.Dispatcher.Unsafe
{
    public class UnsafeDispatch<T> where T:unmanaged
    {
        public unsafe T* Value { get; set; }

        public static unsafe implicit operator T*(UnsafeDispatch<T> dispatch)
        {
            return dispatch.Value;
        }

        public static unsafe implicit operator UnsafeDispatch<T>(T* value)
        {
            return new UnsafeDispatch<T>{Value = value};
        }
    }
}
