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

namespace Creshendo.UnitTests.Model
{
    public class TestBean
    {
        protected String attr1 = null;
        protected int attr2;
        protected short attr3;
        protected long attr4;
        protected float attr5;
        protected double attr6;


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
    }
}