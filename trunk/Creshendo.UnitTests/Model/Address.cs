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
    public class Address
    {
        protected String accountId = null;
        protected String city = null;
        protected ArrayList listeners = new ArrayList();
        protected String state = null;
        protected String street1 = null;
        protected String street2 = null;
        protected String zip = null;


        public string Street1
        {
            set
            {
                if (!value.Equals(street1))
                {
                    String old = street1;
                    street1 = value;
                    OnPropertyChanged("street1", old, street1);
                }
            }
            get { return street1; }
        }

        public string Street2
        {
            set
            {
                if (!value.Equals(street2))
                {
                    String old = street2;
                    street2 = value;
                    OnPropertyChanged("street2", old, street2);
                }
            }
            get { return street2; }
        }

        public string City
        {
            set
            {
                if (!value.Equals(city))
                {
                    String old = city;
                    city = value;
                    OnPropertyChanged("city", old, city);
                }
            }
            get { return city; }
        }

        public string State
        {
            set
            {
                if (!value.Equals(state))
                {
                    String old = state;
                    state = value;
                    OnPropertyChanged("state", old, state);
                }
            }
            get { return state; }
        }

        public string Zip
        {
            set
            {
                if (!value.Equals(zip))
                {
                    String old = zip;
                    zip = value;
                    OnPropertyChanged("zip", old, zip);
                }
            }
            get { return zip; }
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