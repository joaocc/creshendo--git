using System;
using System.Collections;

namespace Creshendo.UnitTests.Model
{
    public class GroupTransaction : Security
    {
        protected String[] accountIds = null;
        protected double buyPrice;
        protected new ArrayList listeners = new ArrayList();
        protected String purchaseDate = null;
        protected double shares;
        protected double total;
        protected String transactionId = null;

        public string[] AccountIds
        {
            set
            {
                if (value != accountIds)
                {
                    String[] old = accountIds;
                    accountIds = value;
                    OnPropertyChanged("accountIds", old, accountIds);
                }
            }
            get { return accountIds; }
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
    }
}