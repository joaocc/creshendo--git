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
 


import PropertyChangeListener;
import PropertyChangeEvent;
import java.util.ArrayList;
*/
/**
 * @author Peter Lin
 *
 * Alternate version of TestBean that does implement add/remove
 * PropertyChangeListener. This version implements notify method
 * to notify the listeners.
 */
using System;
using System.Collections;
using Creshendo.Util;

namespace Creshendo.UnitTests.Model
{
    public class TestBean3
    {
        protected String attr1 = null;
        protected int attr2;
        protected short attr3;
        protected long attr4;
        protected float attr5;
        protected double attr6;

        protected ArrayList listeners = new ArrayList();


        public string Name
        {
            set { attr1 = value; }
            get { return attr1; }
        }

        public int Count
        {
            set { attr2 = value; }
            get { return attr2; }
        }

        public short Short
        {
            set { attr3 = value; }
            get { return attr3; }
        }

        public long Long
        {
            set { attr4 = value; }
            get { return attr4; }
        }

        public float Float
        {
            set { attr5 = value; }
            get { return attr5; }
        }

        public double Double
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