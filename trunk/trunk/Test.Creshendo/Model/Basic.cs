using System;
using Creshendo.Util;

namespace Test.Creshendo.Model
{
    public class Basic
    {
        private int _that;
        private string _this;


        public Basic()
        {
        }

        public Basic(string th, int ta)
        {
            _this = th;
            _that = ta;
        }

        public string This
        {
            get { return _this; }
            set { _this = value; }
        }

        public int That
        {
            get { return _that; }
            set { _that = value; }
        }

        public event PropertyChangedHandler PropertyChanged;


        protected void OnPropertyChanged(String propName, Object oldValue, Object newValue)
        {
            PropertyChangedHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedHandlerEventArgs(propName, oldValue, newValue));
            }
        }
    }
}