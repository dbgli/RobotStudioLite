using System.ComponentModel;

namespace DBGware.RobotStudioLite.Utilities
{
    public class CancellablePropertyChangingEventArgs<T> : PropertyChangingEventArgs
    {
        public bool IsCancelled { get; set; }
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public CancellablePropertyChangingEventArgs(string propertyName, T oldValue, T newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}