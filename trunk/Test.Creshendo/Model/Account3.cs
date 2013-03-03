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

namespace Test.Creshendo.Model
{
    public class Account3 : Account2
    {
        protected String city = null;
        protected String country = null;
        protected String state = null;
        protected String stree2 = null;
        protected String street1 = null;


        public string Street1
        {
            get { return street1; }
            set
            {
                if (!value.Equals(street1))
                {
                    String old = street1;
                    street1 = value;
                    OnPropertyChanged("street1", old, street1);
                }
            }
        }

        public string Street2
        {
            get { return stree2; }
            set
            {
                if (!value.Equals(stree2))
                {
                    String old = stree2;
                    stree2 = value;
                    OnPropertyChanged("stree2", old, stree2);
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (!value.Equals(city))
                {
                    String old = city;
                    city = value;
                    OnPropertyChanged("city", old, city);
                }
            }
        }

        public string State
        {
            get { return state; }
            set
            {
                if (!value.Equals(state))
                {
                    String old = state;
                    state = value;
                    OnPropertyChanged("state", old, state);
                }
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                if (!value.Equals(country))
                {
                    String old = country;
                    country = value;
                    OnPropertyChanged("country", old, country);
                }
            }
        }
    }
}