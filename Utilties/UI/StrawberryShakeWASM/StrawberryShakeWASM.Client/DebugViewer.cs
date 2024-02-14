namespace StrawberryShakeWASM.Client
{
    public class DebugViewer<T> where T : class
    {
        private T _value;

        public T? Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                }
            }
        }
    }
}
