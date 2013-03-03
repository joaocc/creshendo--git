using System;
using System.Collections;
using Creshendo.Util;

namespace Test.Creshendo.Model
{
    public class SubTransaction : Security
    {
        protected new ArrayList listeners = new ArrayList();
        protected String[] transactionSet = null;


        public string[] TransactionSet
        {
            set
            {
                if (value != transactionSet)
                {
                    String[] old = transactionSet;
                    transactionSet = value;
                    OnPropertyChanged("transactionSet", old, transactionSet);
                }
            }
            get { return transactionSet; }
        }

        public new event PropertyChangedHandler PropertyChanged;

        protected new void OnPropertyChanged(String propName, Object oldValue, Object newValue)
        {
            PropertyChangedHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedHandlerEventArgs(propName, oldValue, newValue));
            }
        }
    }
}