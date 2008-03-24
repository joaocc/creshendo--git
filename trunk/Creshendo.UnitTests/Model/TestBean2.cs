using System;
using System.Collections;
using Creshendo.Util;

namespace Creshendo.UnitTests.Model
{
    public class TestBean2
    {
        protected String attr1 = null;
        protected int attr2;
        protected short attr3;
        protected long attr4;
        protected float attr5;
        protected double attr6;

        protected ArrayList listeners = new ArrayList();

        public TestBean2()
        {
        }

        public string Attr1
        {
            set { attr1 = value; }
            get { return attr1; }
        }

        public int Attr2
        {
            set { attr2 = value; }
            get { return attr2; }
        }

        public short Attr3
        {
            set { attr3 = value; }
            get { return attr3; }
        }

        public long Attr4
        {
            set { attr4 = value; }
            get { return attr4; }
        }

        public float Attr5
        {
            set { attr5 = value; }
            get { return attr5; }
        }

        public double Attr6
        {
            set { attr6 = value; }
            get { return attr6; }
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