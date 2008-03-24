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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// The purpose of Evaluate is similar to the Evaluatn in CLIPS. The class
    /// constains static methods for evaluating two values
    /// 
    /// </author>
    public class Evaluate
    {
        /// <summary> evaluate is responsible for evaluating two values. The left value
        /// is the value in the slot. The right value is the value of the object
        /// instance to match against.
        /// </summary>
        /// <param name="">operator
        /// </param>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluate(int operator_Renamed, Object left, Object right)
        {
            bool eval = false;
            switch (operator_Renamed)
            {
                case Constants.EQUAL:
                    eval = evaluateEqual(left, right);
                    break;

                case Constants.NOTEQUAL:
                    eval = evaluateNotEqual(left, right);
                    break;

                case Constants.LESS:
                    eval = evaluateLess(left, right);
                    break;

                case Constants.LESSEQUAL:
                    eval = evaluateLessEqual(left, right);
                    break;

                case Constants.GREATER:
                    eval = evaluateGreater(left, right);
                    break;

                case Constants.GREATEREQUAL:
                    eval = evaluateGreaterEqual(left, right);
                    break;

                case Constants.NILL:
                    eval = evaluateNull(left, right);
                    break;
            }
            return eval;
        }

        /// <summary> evaluate if two values are equal. If they are equal
        /// return true. otherwise return false.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateEqual(Object left, Object right)
        {
            if (left is String)
            {
                return evaluateStringEqual((String) left, right);
            }
            else if (left is Boolean)
            {
                return evaluateEqual((Boolean) left, right);
            }
            else if (left is Double)
            {
                return evaluateEqual((Double) left, right);
            }
            else if (left is Int32)
            {
                return evaluateEqual((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateEqual((Int16) left, right);
            }
            else if (left is Single)
            {
                return evaluateEqual((Single) left, right);
            }
            else if (left is Int64)
            {
                return evaluateEqual((Int64) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateEqual((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        /// <summary> evaluate if two values are equal when left is a string and right
        /// is some object.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateStringEqual(String left, Object right)
        {
            if (right is Boolean)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Convert.ToBoolean(left) == Convert.ToBoolean(right);
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary> evaluate Boolean values against each other. If the right is a String,
        /// the method will attempt to create a new Boolean object and evaluate.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateEqual(Boolean left, Object right)
        {
            if (right is Boolean)
            {
                return left.Equals(right);
            }
            else if (right is String)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Convert.ToBoolean(left) == Convert.ToBoolean(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Int32 left, Object right)
        {
            if (right is Double)
            {
                return (double) left == ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left == Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Int16 left, Object right)
        {
            if (right is Double)
            {
                return (double) left == ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left == Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Single left, Object right)
        {
            if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == ((Double) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left <= Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Int64 left, Object right)
        {
            if (right is Double)
            {
                return (double) left == ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left == Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Double left, Object right)
        {
            if (right is Double)
            {
                return left == ((Double) right);
            }
            else if (right is Int32)
            {
                return left == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left == Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateEqual(Decimal left, Object right)
        {
            if (right is Double)
            {
                return Decimal.ToDouble(left) == ((Double) right);
            }
            else if (right is Int32)
            {
                return Decimal.ToDouble(left) == (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return Decimal.ToDouble(left) == (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToDouble(left) == (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToDouble(left) == (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) == Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        /// <summary> evaluate if two values are not equal. If they are not
        /// equal, return true. Otherwise return false.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateNotEqual(Object left, Object right)
        {
            if (left is String)
            {
                return !left.Equals(right);
            }
            else if (left is Boolean)
            {
                return evaluateNotEqual((Boolean) left, right);
            }
            else if (left is Int32)
            {
                return evaluateNotEqual((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateNotEqual((Int16) left, right);
            }
            else if (left is Single)
            {
                return evaluateNotEqual((Single) left, right);
            }
            else if (left is Int64)
            {
                return evaluateNotEqual((Int64) left, right);
            }
            else if (left is Double)
            {
                return evaluateNotEqual((Double) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateNotEqual((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        /// <summary> evaluate Boolean values against each other. If the right is a String,
        /// the method will attempt to create a new Boolean object and evaluate.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateNotEqual(Boolean left, Object right)
        {
            if (right is Boolean)
            {
                return left.Equals(right);
            }
            else if (right is String)
            {
                Boolean b = ((String) right).Equals("true", StringComparison.InvariantCultureIgnoreCase);
                return left.Equals(b);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Int32 left, Object right)
        {
            if (right is Double)
            {
                return (double) left != ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Int16 left, Object right)
        {
            if (right is Double)
            {
                return (double) left != ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Single left, Object right)
        {
            if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != ((Double) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Int64 left, Object right)
        {
            if (right is Double)
            {
                return (double) left != ((Double) right);
            }
            else if (right is Int32)
            {
                return (double) left != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (double) left != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return (double) left != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return (double) left != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Double left, Object right)
        {
            if (right is Double)
            {
                return left != ((Double) right);
            }
            else if (right is Int32)
            {
                return left != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateNotEqual(Decimal left, Object right)
        {
            if (right is Double)
            {
                return Decimal.ToDouble(left) != ((Double) right);
            }
            else if (right is Int32)
            {
                return Decimal.ToDouble(left) != (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return Decimal.ToDouble(left) != (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToDouble(left) != (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToDouble(left) != (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) != Decimal.ToDouble(((Decimal) right));
            }
            else if (right is String)
            {
                return !left.ToString().Equals(right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLess(Object left, Object right)
        {
            if (left is Int32)
            {
                return evaluateLess((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateLess((Int16) left, right);
            }
            else if (left is Int64)
            {
                return evaluateLess((Int64) left, right);
            }
            else if (left is Single)
            {
                return evaluateLess((Single) left, right);
            }
            else if (left is Double)
            {
                return evaluateLess((Double) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateLess((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Object left, Object right)
        {
            if (left is Int32)
            {
                return evaluateLessEqual((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateLessEqual((Int16) left, right);
            }
            else if (left is Int64)
            {
                return evaluateLessEqual((Int64) left, right);
            }
            else if (left is Single)
            {
                return evaluateLessEqual((Single) left, right);
            }
            else if (left is Double)
            {
                return evaluateLessEqual((Double) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateLessEqual((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Object left, Object right)
        {
            if (left is Int32)
            {
                return evaluateGreater((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateGreater((Int16) left, right);
            }
            else if (left is Int64)
            {
                return evaluateGreater((Int64) left, right);
            }
            else if (left is Single)
            {
                return evaluateGreater((Single) left, right);
            }
            else if (left is Double)
            {
                return evaluateGreater((Double) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateGreater((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Object left, Object right)
        {
            if (left is Int32)
            {
                return evaluateGreaterEqual((Int32) left, right);
            }
            else if (left is Int16)
            {
                return evaluateGreaterEqual((Int16) left, right);
            }
            else if (left is Int64)
            {
                return evaluateGreaterEqual((Int64) left, right);
            }
            else if (left is Single)
            {
                return evaluateGreaterEqual((Single) left, right);
            }
            else if (left is Double)
            {
                return evaluateGreaterEqual((Double) left, right);
            }
            else if (left is Decimal)
            {
                return evaluateGreaterEqual((Decimal) left, right);
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Decimal left, Object right)
        {
            if (right is Int32)
            {
                return Decimal.ToInt32(left) >= ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_ISSUE: Method 'java.lang.Number.shortValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
                return Convert.ToInt16(left) >= (short) ((Int16) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToInt64(left) >= (long) ((Int64) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToSingle(left) >= (float) ((Single) right);
            }
            else if (right is Double)
            {
                return Decimal.ToDouble(left) >= ((Double) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        /// <summary> In the case of checking if an object's attribute is null,
        /// we only check the right.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool evaluateNull(Object left, Object right)
        {
            if (right == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// ------- Integer comparison methods ------- ///
        public static bool evaluateLess(Int32 left, Object right)
        {
            if (right is Int32)
            {
                return left < ((Int32) right);
            }
            else if (right is Int16)
            {
                return left < (int) ((Int16) right);
            }
            else if (right is Int64)
            {
                return (long) left < (long) ((Int64) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left < ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left < Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Int32 left, Object right)
        {
            if (right is Int32)
            {
                return left <= ((Int32) right);
            }
            else if (right is Int16)
            {
                return left <= (int) ((Int16) right);
            }
            else if (right is Int64)
            {
                return (long) left <= (long) ((Int64) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left <= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        //UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
        public static bool evaluateGreater(Int32 left, object right)
        {
            if (right is Int32)
            {
                return left > ((Int32) right);
            }
            else if (right is Int16)
            {
                return left > (int) ((Int16) right);
            }
            else if (right is Int64)
            {
                return (long) left > (long) ((Int64) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left > ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Int32 left, Object right)
        {
            if (right is Int32)
            {
                return left >= ((Int32) right);
            }
            else if (right is Int16)
            {
                return left >= (int) ((Int16) right);
            }
            else if (right is Int64)
            {
                return (long) left >= (long) ((Int64) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left >= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        /// ------- Short comparison methods ------- ///
        public static bool evaluateLess(Int16 left, Object right)
        {
            if (right is Int16)
            {
                return (short) left < (short) ((Int16) right);
            }
            else if (right is Int32)
            {
                return (int) left < ((Int32) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Single) right);
            }
            else if (right is Int64)
            {
                return (long) left < (long) ((Int64) right);
            }
            else if (right is Double)
            {
                return (double) left < ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left < Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Int16 left, Object right)
        {
            if (right is Int16)
            {
                return (short) left <= (short) ((Int16) right);
            }
            else if (right is Int32)
            {
                return (int) left <= ((Int32) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Single) right);
            }
            else if (right is Int64)
            {
                return (long) left <= (long) ((Int64) right);
            }
            else if (right is Double)
            {
                return (double) left <= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Int16 left, Object right)
        {
            if (right is Int16)
            {
                return (short) left > (short) ((Int16) right);
            }
            else if (right is Int32)
            {
                return (int) left > ((Int32) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Single) right);
            }
            else if (right is Int64)
            {
                return (long) left > (long) ((Int64) right);
            }
            else if (right is Double)
            {
                return (double) left > ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Int16 left, Object right)
        {
            if (right is Int16)
            {
                return (short) left >= (short) ((Int16) right);
            }
            else if (right is Int32)
            {
                return (int) left >= ((Int32) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Single) right);
            }
            else if (right is Int64)
            {
                return (long) left >= (long) ((Int64) right);
            }
            else if (right is Double)
            {
                return (double) left >= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        /// ------- Long comparison methods ------- ///
        public static bool evaluateLess(Int64 left, Object right)
        {
            if (right is Int64)
            {
                return (long) left < (long) ((Int64) right);
            }
            else if (right is Int32)
            {
                return (long) left < (long) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (long) left < (long) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left < ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left < Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Int64 left, Object right)
        {
            if (right is Int64)
            {
                return (long) left <= (long) ((Int64) right);
            }
            else if (right is Int32)
            {
                return (long) left <= (long) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (long) left <= (long) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left <= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Int64 left, Object right)
        {
            if (right is Int64)
            {
                return (long) left > (long) ((Int64) right);
            }
            else if (right is Int32)
            {
                return (long) left > (long) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (long) left > (long) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left > ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Int64 left, Object right)
        {
            if (right is Int64)
            {
                return (long) left >= (long) ((Int64) right);
            }
            else if (right is Int32)
            {
                return (long) left >= (long) ((Int32) right);
            }
            else if (right is Int16)
            {
                return (long) left >= (long) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Single) right);
            }
            else if (right is Double)
            {
                return (double) left >= ((Double) right);
            }
            else if (right is Decimal)
            {
                return (double) left >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        /// ------- Float comparison methods ------- ///
        public static bool evaluateLess(Single left, Object right)
        {
            if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Single) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Int16) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left < (float) ((Int64) right);
            }
            else if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left < ((Double) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left < Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Single left, Object right)
        {
            if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Single) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Int16) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left <= (float) ((Int64) right);
            }
            else if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left <= ((Double) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Single left, Object right)
        {
            if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Single) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Int16) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left > (float) ((Int64) right);
            }
            else if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left > ((Double) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Single left, Object right)
        {
            if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Single) right);
            }
            else if (right is Int32)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Int32) right);
            }
            else if (right is Int16)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Int16) right);
            }
            else if (right is Int64)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (float) left >= (float) ((Int64) right);
            }
            else if (right is Double)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left >= ((Double) right);
            }
            else if (right is Decimal)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return (double) left >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        /// ------- Double comparison methods ------- ///
        public static bool evaluateLess(Double left, Object right)
        {
            if (right is Double)
            {
                return left < ((Double) right);
            }
            else if (right is Int32)
            {
                return left < (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left < (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left < (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left < (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLess(Decimal left, Object right)
        {
            if (right is Double)
            {
                return Decimal.ToDouble(left) < ((Double) right);
            }
            else if (right is Int32)
            {
                return Decimal.ToDouble(left) < (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return Decimal.ToDouble(left) < (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToDouble(left) < (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToDouble(left) < (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) < Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Double left, Object right)
        {
            if (right is Double)
            {
                return left <= ((Double) right);
            }
            else if (right is Int32)
            {
                return left <= (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left <= (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left <= (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left <= (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateLessEqual(Decimal left, Object right)
        {
            if (right is Double)
            {
                return Decimal.ToDouble(left) <= ((Double) right);
            }
            else if (right is Int32)
            {
                return Decimal.ToDouble(left) <= (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return Decimal.ToDouble(left) <= (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToDouble(left) <= (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToDouble(left) <= (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) <= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Decimal left, Object right)
        {
            if (right is Double)
            {
                return Decimal.ToDouble(left) > ((Double) right);
            }
            else if (right is Int32)
            {
                return Decimal.ToDouble(left) > (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return Decimal.ToDouble(left) > (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Decimal.ToDouble(left) > (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return Decimal.ToDouble(left) > (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return Decimal.ToDouble(left) > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreater(Double left, Object right)
        {
            if (right is Double)
            {
                return left > ((Double) right);
            }
            else if (right is Int32)
            {
                return left > (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left > (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left > (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left > (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left > Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool evaluateGreaterEqual(Double left, Object right)
        {
            if (right is Double)
            {
                return left >= ((Double) right);
            }
            else if (right is Int32)
            {
                return left >= (double) ((Int32) right);
            }
            else if (right is Int16)
            {
                return left >= (double) ((Int16) right);
            }
            else if (right is Single)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return left >= (double) ((Single) right);
            }
            else if (right is Int64)
            {
                return left >= (double) ((Int64) right);
            }
            else if (right is Decimal)
            {
                return left >= Decimal.ToDouble(((Decimal) right));
            }
            else
            {
                return false;
            }
        }

        public static bool factsEqual(IFact[] left, IFact[] right)
        {
            if (left == right)
            {
                return true;
            }
            int length = left.Length;
            if (length != right.Length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                Object o1 = left[i];
                Object o2 = right[i];
                if (!(o1 == null ? o2 == null : o1 == o2))
                    return false;
            }
            return true;
        }
    }
}