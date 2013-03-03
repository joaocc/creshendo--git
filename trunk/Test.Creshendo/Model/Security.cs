using System;
using System.Collections;
using Creshendo.Util;

namespace Test.Creshendo.Model
{
    public class Security
    {
        protected String countryCode = null;
        protected double currentPrice;
        protected int cusip;
        protected String exchange = null;
        protected int industryGroupID;
        protected int industryID;
        protected String issuer = null;
        protected double lastPrice;
        protected ArrayList listeners = new ArrayList();
        protected int sectorID;
        protected String securityType = null;
        protected int subIndustryID;

        public string CountryCode
        {
            set
            {
                if (!value.Equals(countryCode))
                {
                    String old = countryCode;
                    countryCode = value;
                    OnPropertyChanged("countryCode", old, countryCode);
                }
            }
            get { return countryCode; }
        }

        public double CurrentPrice
        {
            set
            {
                if (value != currentPrice)
                {
                    Double old = Convert.ToDouble(currentPrice);
                    currentPrice = value;
                    OnPropertyChanged("currentPrice", old, Convert.ToDouble(currentPrice));
                }
            }
            get { return currentPrice; }
        }

        public int Cusip
        {
            set
            {
                if (value != cusip)
                {
                    Int32 old = Convert.ToInt32(cusip);
                    cusip = value;
                    OnPropertyChanged("cusip", old, Convert.ToInt32(cusip));
                }
            }
            get { return cusip; }
        }

        public string Exchange
        {
            set
            {
                if (!value.Equals(exchange))
                {
                    String old = exchange;
                    exchange = value;
                    OnPropertyChanged("exchange", old, exchange);
                }
            }
            get { return exchange; }
        }

        public int IndustryGroupID
        {
            set
            {
                if (value != industryGroupID)
                {
                    int old = industryGroupID;
                    industryGroupID = value;
                    OnPropertyChanged("industryGroupID",
                                      Convert.ToInt32(old), Convert.ToInt32(industryGroupID));
                }
            }
            get { return industryGroupID; }
        }

        public int IndustryID
        {
            set
            {
                if (value != industryID)
                {
                    int old = industryID;
                    industryID = value;
                    OnPropertyChanged("industryID",
                                      Convert.ToInt32(old), Convert.ToInt32(industryID));
                }
            }
            get { return industryID; }
        }

        public string Issuer
        {
            set
            {
                if (!value.Equals(issuer))
                {
                    String old = issuer;
                    issuer = value;
                    OnPropertyChanged("issuer", old, issuer);
                }
            }
            get { return issuer; }
        }

        public double LastPrice
        {
            set
            {
                if (value != lastPrice)
                {
                    Double old = Convert.ToDouble(lastPrice);
                    lastPrice = value;
                    OnPropertyChanged("lastPrice", old, Convert.ToDouble(lastPrice));
                }
            }
            get { return lastPrice; }
        }

        public int SectorID
        {
            set
            {
                if (value != sectorID)
                {
                    int old = sectorID;
                    sectorID = value;
                    OnPropertyChanged("sectorID",
                                      Convert.ToInt32(old), Convert.ToInt32(sectorID));
                }
            }
            get { return sectorID; }
        }

        public string SecurityType
        {
            set
            {
                if (!value.Equals(securityType))
                {
                    String old = securityType;
                    securityType = value;
                    OnPropertyChanged("securityType", old, securityType);
                }
            }
            get { return securityType; }
        }

        public int SubIndustryID
        {
            set
            {
                if (value != subIndustryID)
                {
                    int old = subIndustryID;
                    subIndustryID = value;
                    OnPropertyChanged("subIndustryID", Convert.ToInt32(old), Convert.ToInt32(subIndustryID));
                }
            }
            get { return subIndustryID; }
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