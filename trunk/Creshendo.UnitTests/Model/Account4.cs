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

namespace Creshendo.UnitTests.Model
{
    public class Account4
    {
        protected String accountId = null;
        protected String accountType = null;
        protected double cash;
        protected String countryCode = null;
        protected String first = null;
        protected String last = null;

        protected ArrayList listeners = new ArrayList();
        protected String middle = null;
        protected String status = null;
        protected String title = null;
        protected String username = null;

        public string Title
        {
            set
            {
                if (!value.Equals(title))
                {
                    String old = title;
                    title = value;
                    OnPropertyChanged("title", old, title);
                }
            }
            get { return title; }
        }

        public string First
        {
            set
            {
                if (!value.Equals(first))
                {
                    String old = first;
                    first = value;
                    OnPropertyChanged("first", old, first);
                }
            }
            get { return first; }
        }

        public string Last
        {
            set
            {
                if (!value.Equals(last))
                {
                    String old = last;
                    last = value;
                    OnPropertyChanged("last", old, last);
                }
            }
            get { return last; }
        }

        public string Middle
        {
            set
            {
                if (!value.Equals(middle))
                {
                    String old = middle;
                    middle = value;
                    OnPropertyChanged("middle", old, middle);
                }
            }
            get { return middle; }
        }

        public string Status
        {
            set
            {
                if (!value.Equals(status))
                {
                    String old = status;
                    status = value;
                    OnPropertyChanged("status", old, status);
                }
            }
            get { return status; }
        }

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

        public string AccountType
        {
            set
            {
                if (!value.Equals(accountType))
                {
                    String old = accountType;
                    accountType = value;
                    OnPropertyChanged("accountType", old, accountType);
                }
            }
            get { return accountType; }
        }

        public string Username
        {
            set
            {
                if (!value.Equals(username))
                {
                    String old = username;
                    username = value;
                    OnPropertyChanged("username", old, username);
                }
            }
            get { return username; }
        }

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

        public double Cash
        {
            set
            {
                if (value != cash)
                {
                    Double old = Convert.ToDouble(cash);
                    cash = value;
                    OnPropertyChanged("cash", old, Convert.ToDouble(cash));
                }
            }
            get { return cash; }
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