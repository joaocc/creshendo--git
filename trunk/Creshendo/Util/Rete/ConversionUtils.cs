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
using System.Text;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// ConversioUtils has a methods for autoboxing primitive types
    /// with the Object equivalent.
    /// 
    /// </author>
    public class ConversionUtils
    {
        private static GenericHashMap<string, string> OPR_MAP;


        private static GenericHashMap<string, string> STROPR_MAP;

        static ConversionUtils()
        {
            {
                OPR_MAP = new GenericHashMap<string, string>();
                OPR_MAP.Put(Constants.ADD.ToString(), Constants.ADD_STRING);
                OPR_MAP.Put(Constants.SUBTRACT.ToString(), Constants.SUBTRACT_STRING);
                OPR_MAP.Put(Constants.MULTIPLY.ToString(), Constants.MULTIPLY_STRING);
                OPR_MAP.Put(Constants.DIVIDE.ToString(), Constants.DIVIDE_STRING);
                OPR_MAP.Put(Constants.LESS.ToString(), Constants.LESS_STRING);
                OPR_MAP.Put(Constants.LESSEQUAL.ToString(), Constants.LESSEQUAL_STRING);
                OPR_MAP.Put(Constants.GREATER.ToString(), Constants.GREATER_STRING);
                OPR_MAP.Put(Constants.GREATEREQUAL.ToString(), Constants.GREATEREQUAL_STRING);
                OPR_MAP.Put(Constants.EQUAL.ToString(), Constants.EQUAL_STRING);
                OPR_MAP.Put(Constants.NOTEQUAL.ToString(), Constants.NOTEQUAL_STRING);
            }
            {
                STROPR_MAP = new GenericHashMap<string, string>();
                OPR_MAP.Put(Constants.ADD_STRING, Constants.ADD_SYMBOL);
                OPR_MAP.Put(Constants.SUBTRACT_STRING, Constants.SUBTRACT_SYMBOL);
                OPR_MAP.Put(Constants.MULTIPLY_STRING, Constants.MULTIPLY_SYMBOL);
                OPR_MAP.Put(Constants.DIVIDE_STRING, Constants.DIVIDE_SYMBOL);
                OPR_MAP.Put(Constants.LESS_STRING, Constants.LESS_SYMBOL);
                OPR_MAP.Put(Constants.LESSEQUAL_STRING, Constants.LESSEQUAL_SYMBOL);
                OPR_MAP.Put(Constants.GREATER_STRING, Constants.GREATER_SYMBOL);
                OPR_MAP.Put(Constants.GREATEREQUAL_STRING, Constants.GREATEREQUAL_SYMBOL);
                OPR_MAP.Put(Constants.EQUAL_STRING, Constants.EQUAL_SYMBOL);
                OPR_MAP.Put(Constants.NOTEQUAL_STRING, Constants.NOTEQUAL_SYMBOL);
            }
        }


        /// <summary> Convert a int primitive to an Integer object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(int val)
        {
            return val;
        }

        /// <summary> Convert a short primitive to a Short object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(short val)
        {
            return val;
        }

        /// <summary> Convert a float primitive to Float object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(float val)
        {
            return val;
        }

        /// <summary> convert a primitive long to Long object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(long val)
        {
            return val;
        }

        /// <summary> convert a primitive double to a Double object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(double val)
        {
            return val;
        }

        /// <summary> convert a primitive byte to Byte object
        /// </summary>
        /// <param name="">val
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object convert(sbyte val)
        {
            return val;
        }

        public static Object convert(int type, Object val)
        {
            if (type == Constants.INT_PRIM_TYPE || type == Constants.INTEGER_OBJECT)
            {
                if (val is Decimal)
                {
                    return Decimal.ToInt32(((Decimal) val));
                }
            }
            else if (type == Constants.SHORT_PRIM_TYPE || type == Constants.SHORT_OBJECT)
            {
                if (val is Decimal)
                {
                    //UPGRADE_ISSUE: Method 'java.lang.Number.shortValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                    return Convert.ToInt16(val);
                }
            }
            else if (type == Constants.FLOAT_PRIM_TYPE || type == Constants.FLOAT_OBJECT)
            {
                if (val is Decimal)
                {
                    return Decimal.ToSingle(((Decimal) val));
                }
            }
            else if (type == Constants.LONG_PRIM_TYPE || type == Constants.LONG_OBJECT)
            {
                if (val is Decimal)
                {
                    return Decimal.ToInt64(((Decimal) val));
                }
            }
            else if (type == Constants.DOUBLE_PRIM_TYPE || type == Constants.DOUBLE_OBJECT)
            {
                if (val is Decimal)
                {
                    return Decimal.ToDouble(((Decimal) val));
                }
            }
            return val;
        }

        /// <summary> Return the string form of the operator
        /// </summary>
        /// <param name="">opr
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static String getPPOperator(int opr)
        {
            return (String) OPR_MAP.Get(opr.ToString());
        }

        /// <summary> find the matching fact in the array
        /// </summary>
        /// <param name="">temp
        /// </param>
        /// <param name="">facts
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IFact findFact(Deftemplate temp, IFact[] facts)
        {
            IFact ft = null;
            for (int idx = 0; idx < facts.Length; idx++)
            {
                if (facts[idx].Deftemplate == temp)
                {
                    ft = facts[idx];
                }
            }
            return ft;
        }

        /// <summary> Method will merge the two arrays by Add the facts from
        /// the right to the end
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IFact[] mergeFacts(IFact[] left, IFact[] right)
        {
            IFact[] merged = new IFact[left.Length + right.Length];
            Array.Copy(left, 0, merged, 0, left.Length);
            Array.Copy(right, 0, merged, left.Length, right.Length);
            return merged;
        }

        /// <summary> The method will merge a single right fact with the left
        /// fact array.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IFact[] mergeFacts(IFact[] left, IFact right)
        {
            IFact[] merged = new IFact[left.Length + 1];
            Array.Copy(left, 0, merged, 0, left.Length);
            merged[left.Length] = right;
            return merged;
        }

        /// <summary> Add a new object to an object array
        /// </summary>
        /// <param name="">list
        /// </param>
        /// <param name="">nobj
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static BaseNode[] add(BaseNode[] list, BaseNode nobj)
        {
            BaseNode[] newlist = new BaseNode[list.Length + 1];
            Array.Copy(list, 0, newlist, 0, list.Length);
            newlist[list.Length] = nobj;
            return newlist;
        }

        /// <summary> Remove an object from an object array
        /// </summary>
        /// <param name="">list
        /// </param>
        /// <param name="">nobj
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static BaseNode[] remove(BaseNode[] list, Object nobj)
        {
            BaseNode[] newlist = new BaseNode[list.Length - 1];
            int pos = 0;
            for (int idx = 0; idx < list.Length; idx++)
            {
                if (list[idx] != nobj)
                {
                    newlist[pos] = list[idx];
                    pos++;
                }
            }
            return newlist;
        }

        /// <summary> Return the int mapped type for the field
        /// </summary>
        /// <param name="">clzz
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static int getTypeCode(Type clzz)
        {
            if (clzz.IsArray)
            {
                return Constants.ARRAY_TYPE;
            }
            else if (clzz.IsPrimitive)
            {
                if (clzz == typeof (int))
                {
                    return Constants.INT_PRIM_TYPE;
                }
                else if (clzz == typeof (short))
                {
                    return Constants.SHORT_PRIM_TYPE;
                }
                else if (clzz == typeof (long))
                {
                    return Constants.LONG_PRIM_TYPE;
                }
                else if (clzz == typeof (float))
                {
                    return Constants.FLOAT_PRIM_TYPE;
                }
                else if (clzz == typeof (sbyte))
                {
                    return Constants.BYTE_PRIM_TYPE;
                }
                else if (clzz == typeof (double))
                {
                    return Constants.DOUBLE_PRIM_TYPE;
                }
                else if (clzz == typeof (bool))
                {
                    return Constants.BOOLEAN_PRIM_TYPE;
                }
                else if (clzz == typeof (char))
                {
                    return Constants.CHAR_PRIM_TYPE;
                }
                else
                {
                    return Constants.OBJECT_TYPE;
                }
            }
            else if (clzz == typeof (String))
            {
                return Constants.STRING_TYPE;
            }
            else
            {
                return Constants.OBJECT_TYPE;
            }
        }

        /// <summary> Convienance method for converting the int type code
        /// to the string form
        /// </summary>
        /// <param name="">intType
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static String getTypeName(int intType)
        {
            if (intType == Constants.INT_PRIM_TYPE)
            {
                return "INTEGER";
            }
            else if (intType == Constants.SHORT_PRIM_TYPE)
            {
                return "SHORT";
            }
            else if (intType == Constants.LONG_PRIM_TYPE)
            {
                return "LONG";
            }
            else if (intType == Constants.FLOAT_PRIM_TYPE)
            {
                return "FLOAT";
            }
            else if (intType == Constants.DOUBLE_PRIM_TYPE)
            {
                return "DOUBLE";
            }
            else if (intType == Constants.BYTE_PRIM_TYPE)
            {
                return "BYTE";
            }
            else if (intType == Constants.BOOLEAN_PRIM_TYPE)
            {
                return "BOOLEAN";
            }
            else if (intType == Constants.CHAR_PRIM_TYPE)
            {
                return "CHAR";
            }
            else if (intType == Constants.STRING_TYPE)
            {
                return "STRING";
            }
            else if (intType == Constants.ARRAY_TYPE)
            {
                return typeof (Object[]).Name;
            }
            else
            {
                return typeof (Object).FullName;
            }
        }

        public static int getOperatorCode(String strSymbol)
        {
            if (strSymbol.Equals(Constants.EQUAL_SYMBOL))
            {
                return Constants.EQUAL;
            }
            else if (strSymbol.Equals(Constants.NOTEQUAL_SYMBOL))
            {
                return Constants.NOTEQUAL;
            }
            else if (strSymbol.Equals(Constants.ADD_SYMBOL))
            {
                return Constants.ADD;
            }
            else if (strSymbol.Equals(Constants.SUBTRACT_SYMBOL))
            {
                return Constants.SUBTRACT;
            }
            else if (strSymbol.Equals(Constants.MULTIPLY_SYMBOL))
            {
                return Constants.MULTIPLY;
            }
            else if (strSymbol.Equals(Constants.DIVIDE_SYMBOL))
            {
                return Constants.DIVIDE;
            }
            else if (strSymbol.Equals(Constants.GREATER_SYMBOL))
            {
                return Constants.GREATER;
            }
            else if (strSymbol.Equals(Constants.GREATEREQUAL_SYMBOL))
            {
                return Constants.GREATEREQUAL;
            }
            else if (strSymbol.Equals(Constants.LESS_SYMBOL))
            {
                return Constants.LESS;
            }
            else if (strSymbol.Equals(Constants.LESSEQUAL_SYMBOL))
            {
                return Constants.LESSEQUAL;
            }
            else
            {
                return Constants.USERDEFINED;
            }
        }

        public static int getOppositeOperatorCode(int op)
        {
            int rvop = Constants.EQUAL;
            switch (op)
            {
                case Constants.EQUAL:
                    rvop = Constants.NOTEQUAL;
                    break;

                case Constants.NOTEQUAL:
                    rvop = Constants.EQUAL;
                    break;

                case Constants.GREATER:
                    rvop = Constants.LESS;
                    break;

                case Constants.LESS:
                    rvop = Constants.GREATER;
                    break;

                case Constants.GREATEREQUAL:
                    rvop = Constants.LESSEQUAL;
                    break;

                case Constants.LESSEQUAL:
                    rvop = Constants.GREATEREQUAL;
                    break;
            }
            return rvop;
        }

        /// <summary> If the operate is equal, not equal, greater, less than,
        /// greater or equal, less than or equal.
        /// </summary>
        /// <param name="">strSymbol
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool isPredicateOperatorCode(String strSymbol)
        {
            if (strSymbol.Equals(Constants.EQUAL_SYMBOL))
            {
                return true;
            }
            else if (strSymbol.Equals(Constants.NOTEQUAL_SYMBOL))
            {
                return true;
            }
            else if (strSymbol.Equals(Constants.GREATER_SYMBOL))
            {
                return true;
            }
            else if (strSymbol.Equals(Constants.GREATEREQUAL_SYMBOL))
            {
                return true;
            }
            else if (strSymbol.Equals(Constants.LESS_SYMBOL))
            {
                return true;
            }
            else if (strSymbol.Equals(Constants.LESSEQUAL_SYMBOL))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static String formatSlot(Object s)
        {
            if (s != null)
            {
                if (s is Boolean)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return s.ToString().ToUpper();
                }
                else if (s is String)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return "\"" + s.ToString() + "\"";
                }
                else if (s.GetType() != null && s.GetType().IsArray)
                {
                    StringBuilder buf = new StringBuilder();
                    Object[] ary = (Object[]) s;
                    for (int idx = 0; idx < ary.Length; idx++)
                    {
                        if (idx > 0)
                        {
                            buf.Append(" ");
                        }
                        buf.Append(formatSlot(ary[idx]));
                    }
                    return buf.ToString();
                }
                else
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    return s.ToString();
                }
            }
            else
            {
                return Constants.NIL_SYMBOL;
            }
        }

        [STAThread]
        public static void Main(String[] args)
        {
            /// <summary>String[] left = {"one","two","three"};
            /// String[] right = {"four","five"};
            /// String[] m = (String[])mergeFacts(left,right);
            /// for (int idx=0; idx < m.length; idx++){
            /// System.out.println(m[idx]);
            /// }
            /// *
            /// </summary>
        }
    }
}