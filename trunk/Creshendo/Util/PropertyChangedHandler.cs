using System;

namespace Creshendo.Util
{
    public delegate void PropertyChangedHandler(object sender, PropertyChangedHandlerEventArgs e);


    public class PropertyChangedHandlerEventArgs : EventArgs
    {
        private readonly object _newValue;
        private readonly object _oldValue;
        private readonly string _propertyName;

        public PropertyChangedHandlerEventArgs(string propertyName, object oldValue, object newValue)
        {
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public object NewValue
        {
            get { return _newValue; }
        }
    }
}