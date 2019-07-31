namespace Ultz.Dispatcher.Unsafe
{
    public struct UnsafeDispatch
    {
        public unsafe void* Value { get; set; }

        public static unsafe implicit operator void*(UnsafeDispatch dispatch)
        {
            return dispatch.Value;
        }

        public static unsafe implicit operator UnsafeDispatch(void* value)
        {
            return new UnsafeDispatch {Value = value};
        }
    }
}
