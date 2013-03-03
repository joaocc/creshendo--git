/*
 * Copyright 2002-2006 Peter Lin
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://ruleml-dev.sourceforge.net/
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */
using System;
using System.Collections;
using Creshendo.Util;

//using System.ComponentModel;

/*

    *
     * @author Peter Lin
     *
     * A simple test bean that represents a generic account. It could be
     * a bank account, shopping card account, or any type of membership
     * account with a nationwide company.
     */

namespace Test.Creshendo.Model
{
    public class Account : IAccount
    {
        protected String accountId = null;
        protected String accountType = null;
        protected String areaCode = null;
        protected String exchange = null;
        protected String ext = null;
        protected String first = null;
        protected String last = null;

        protected ArrayList listeners = new ArrayList();
        protected String middle = null;
        protected String number = null;
        protected String officeCode = null;
        protected String regionCode = null;
        protected String status = null;
        protected String title = null;
        protected String username = null;

        #region IAccount Members

        public string Title
        {
            get { return title; }
            set
            {
                if (!value.Equals(title))
                {
                    String old = title;
                    title = value;
                    OnPropertyChanged("title", old, title);
                }
            }
        }

        public string First
        {
            get { return first; }
            set
            {
                if (!value.Equals(first))
                {
                    String old = first;
                    first = value;
                    OnPropertyChanged("first", old, first);
                }
            }
        }

        public string Last
        {
            get { return last; }
            set
            {
                if (!value.Equals(last))
                {
                    String old = last;
                    last = value;
                    OnPropertyChanged("last", old, last);
                }
            }
        }

        public string Middle
        {
            get { return middle; }
            set
            {
                if (!value.Equals(middle))
                {
                    String old = middle;
                    middle = value;
                    OnPropertyChanged("middle", old, middle);
                }
            }
        }

        public string OfficeCode
        {
            get { return officeCode; }
            set
            {
                if (!value.Equals(officeCode))
                {
                    String old = officeCode;
                    officeCode = value;
                    OnPropertyChanged("officeCode", old, officeCode);
                }
            }
        }

        public string RegionCode
        {
            get { return regionCode; }
            set
            {
                if (!value.Equals(regionCode))
                {
                    String old = regionCode;
                    regionCode = value;
                    OnPropertyChanged("regionCode", old, regionCode);
                }
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (!value.Equals(status))
                {
                    String old = status;
                    status = value;
                    OnPropertyChanged("status", old, status);
                }
            }
        }

        public string AccountId
        {
            get { return accountId; }
            set
            {
                if (!value.Equals(accountId))
                {
                    String old = accountId;
                    accountId = value;
                    OnPropertyChanged("accountId", old, accountId);
                }
            }
        }

        public string AccountType
        {
            get { return accountType; }
            set
            {
                if (!value.Equals(accountType))
                {
                    String old = accountType;
                    accountType = value;
                    OnPropertyChanged("accountType", old, accountType);
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (!value.Equals(username))
                {
                    String old = username;
                    username = value;
                    OnPropertyChanged("username", old, username);
                }
            }
        }

        public string AreaCode
        {
            get { return areaCode; }
            set
            {
                if (!value.Equals(areaCode))
                {
                    String old = areaCode;
                    areaCode = value;
                    OnPropertyChanged("areaCode", old, areaCode);
                }
            }
        }

        public string Exchange
        {
            get { return exchange; }
            set
            {
                if (!value.Equals(exchange))
                {
                    String old = exchange;
                    exchange = value;
                    OnPropertyChanged("exchange", old, exchange);
                }
            }
        }

        public string Number
        {
            get { return number; }
            set
            {
                if (!value.Equals(number))
                {
                    String old = number;
                    number = value;
                    OnPropertyChanged("number", old, number);
                }
            }
        }

        public string Ext
        {
            get { return ext; }
            set
            {
                if (!value.Equals(ext))
                {
                    String old = ext;
                    ext = value;
                    OnPropertyChanged("ext", old, ext);
                }
            }
        }

        #endregion

        public event PropertyChangedHandler PropertyChanged;


        protected void OnPropertyChanged(String propName, Object oldValue, Object newValue)
        {
            PropertyChangedHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedHandlerEventArgs(propName, oldValue, newValue));
            }
        }

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            return hash;
        }
    }
}