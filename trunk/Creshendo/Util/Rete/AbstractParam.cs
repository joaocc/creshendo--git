/*
* Copyright 2002-2007 Peter Lin Licensed under the Apache License, Version 2.0
* (the "License"); you may not use this file except in compliance with the
* License. You may obtain a copy of the License at
* http://jamocha.sourceforge.net/ Unless required by applicable law or
* agreed to in writing, software distributed under the License is distributed
* on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
* express or implied. See the License for the specific language governing
* permissions and limitations under the License.
*/
using System;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin AbstractParam provides the common implementation of
    /// Parameter interface.
    /// 
    /// </author>
    [Serializable]
    public abstract class AbstractParam : IParameter
    {
        //protected internal bool objBinding = false;

        #region Parameter Members

        /// <summary> Get the value type
        /// </summary>
        public abstract int ValueType { get; set; }

        public abstract object Value { get; set; }


        /// <summary> Get the value of the given slot
        /// </summary>
        //public abstract System.Object getValue{Get;}
        /// <summary> subclasses have to implement the method
        /// </summary>
        public abstract bool ObjectBinding {get; }
        

        /// <summary> the implementation will check if the value is a String. if it is, it
        /// casts the object to a String, otherwise it calls the object's toString()
        /// method.
        /// </summary>
        public virtual String StringValue
        {
            get
            {
                Object value_Renamed = Value;
                if (value_Renamed != null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return Value.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary> 
        /// </summary>
        public virtual bool BooleanValue
        {
            get
            {
                if (Value != null && !(Value is Boolean))
                {
                    Boolean b = StringValue.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                    return b;
                }
                else
                {
                    return ((Boolean) Value);
                }
            }
        }

        public virtual int IntValue
        {
            get
            {
                //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                if (Value != null && !(Value is int))
                {
                    throw new FormatException("Value is not a number");
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.intValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return ((int) Value);
                }
            }
        }

        public virtual short ShortValue
        {
            get
            {
                //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                if (Value != null && !(Value is short))
                {
                    throw new FormatException("Value is not a number");
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.shortValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return ((short) Value);
                }
            }
        }

        public virtual long LongValue
        {
            get
            {
                //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                if (Value != null && !(Value is long))
                {
                    throw new FormatException("Value is not a number");
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.longValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return ((long) Value);
                }
            }
        }

        public virtual float FloatValue
        {
            get
            {
                //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                if (Value != null && !(Value is float))
                {
                    throw new FormatException("Value is not a number");
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.floatValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return ((float) Value);
                }
            }
        }

        public virtual double DoubleValue
        {
            get
            {
                //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                if (Value != null && !(Value is double))
                {
                    throw new FormatException("Value is not a number");
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.doubleValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return ((double) Value);
                }
            }
        }

        public virtual Decimal BigIntegerValue
        {
            get
            {
                if (Value != null && (Value is Decimal))
                {
                    return (Decimal) Value;
                }
                else
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return Decimal.Parse(Value.ToString());
                }
            }
        }

        public virtual Decimal BigDecimalValue
        {
            get
            {
                if (Value != null && (Value is Decimal))
                {
                    return (Decimal) Value;
                }
                else
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return Decimal.Parse(Value.ToString());
                }
            }
        }


        /// <summary> reset sets the Fact handle to null
        /// </summary>
        public abstract void reset();

        public abstract object getValue(Rete engine, int valueType);

        #endregion

        // --- methods for getting the value --- //
    }
}