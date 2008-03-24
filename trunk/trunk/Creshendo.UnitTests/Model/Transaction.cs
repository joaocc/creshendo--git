using System;
using System.Collections;
using Creshendo.Util;

namespace Creshendo.UnitTests.Model
{
    public class Transaction : Security
    {
        protected String accountId = null;
        protected double buyPrice;
        protected new ArrayList listeners = new ArrayList();
        protected String purchaseDate = null;
        protected double shares;
        protected double total;
        protected String transactionId = null;


        public string AccountId
        {
            set
            {
                if (!value.Equals(accountId))
                {
                    String old = accountId;
                    accountId = value;
                    OnPropertyChanged("accountId", old, accountId);
                }
            }
            get { return accountId; }
        }

        public double BuyPrice
        {
            set
            {
                if (value != buyPrice)
                {
                    Double old = Convert.ToDouble(buyPrice);
                    buyPrice = value;
                    OnPropertyChanged("buyPrice", old, Convert.ToDouble(buyPrice));
                }
            }
            get { return buyPrice; }
        }

        public string PurchaseDate
        {
            set
            {
                if (!value.Equals(purchaseDate))
                {
                    String old = purchaseDate;
                    purchaseDate = value;
                    OnPropertyChanged("purchaseDate", old, purchaseDate);
                }
            }
            get { return purchaseDate; }
        }

        public double Shares
        {
            set
            {
                if (value != shares)
                {
                    Double old = Convert.ToDouble(shares);
                    shares = value;
                    OnPropertyChanged("shares", old, Convert.ToDouble(shares));
                }
            }
            get { return shares; }
        }

        public double Total
        {
            set
            {
                if (value != total)
                {
                    Double old = Convert.ToDouble(total);
                    total = value;
                    OnPropertyChanged("total", old, Convert.ToDouble(total));
                }
            }
            get { return total; }
        }

        public string TransactionId
        {
            set
            {
                if (!value.Equals(transactionId))
                {
                    String old = transactionId;
                    transactionId = value;
                    OnPropertyChanged("transactionId", old, transactionId);
                }
            }
            get { return transactionId; }
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