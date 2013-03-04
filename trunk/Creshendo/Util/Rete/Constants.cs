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
    /// 
    /// </author>
    public class Constants
    {
        public const int ACTION_ASSERT = 1000;
        public const int ACTION_MODIFY = 1002;
        public const int ACTION_RETRACT = 1001;

        /// --------- operators types ---------///
        public const int ADD = 1;

        /// --------- operators strings ---------///
        public const String ADD_STRING = "Add";

        /// --------- operators symbol ---------///
        public const String ADD_SYMBOL = "+";

        public const int ARRAY_TYPE = 10;
        public const int BACKWARD_CHAINING = 10001;
        public const int BIDIRECTIONAL_CHAINING = 10002;
        public const int BIG_DECIMAL = 22;
        public const int BIG_INTEGER = 21;
        public const int BOOLEAN_OBJECT = 20;

        public const int BOOLEAN_PRIM_TYPE = 7;
        public const int BYTE_OBJECT = 19;
        public const int BYTE_PRIM_TYPE = 6;
        public const int CHAR_PRIM_TYPE = 8;
        public const String CRLF = "crlf";
        public const int DIVIDE = 4;
        public const String DIVIDE_STRING = "divide";
        public const String DIVIDE_SYMBOL = "/";
        public const int DOUBLE_OBJECT = 18;
        public const int DOUBLE_PRIM_TYPE = 5;
        public const int EQUAL = 9;
        public const String EQUAL_STRING = "equal to";
        public const String EQUAL_SYMBOL = "=";
        public const int FACT_TYPE = 13;
        public const int FLOAT_OBJECT = 17;
        public const int FLOAT_PRIM_TYPE = 4;

        /// ----------- constants for chaining direction -------///
        public const int FORWARD_CHAINING = 10000;

        public const int GREATER = 5;
        public const String GREATER_STRING = "greater than";
        public const String GREATER_SYMBOL = ">";
        public const int GREATEREQUAL = 7;
        public const String GREATEREQUAL_STRING = "greater than or equal to";
        public const String GREATEREQUAL_SYMBOL = ">=";

        /// --------- primitive types ---------///
        public const int INT_PRIM_TYPE = 1;

        public const int INTEGER_OBJECT = 14;
        public const int LAZY_CHAINING = 10003;
        public const int LESS = 6;

        public const String LESS_STRING = "less than";
        public const String LESS_SYMBOL = "<";
        public const int LESSEQUAL = 8;
        public const String LESSEQUAL_STRING = "less than or equal to";
        public const String LESSEQUAL_SYMBOL = "<=";
        public const int LONG_OBJECT = 16;
        public const int LONG_PRIM_TYPE = 3;
        public const String MAIN_MODULE = "MAIN";
        public const int MULTIPLY = 3;
        public const String MULTIPLY_STRING = "multiply";
        public const String MULTIPLY_SYMBOL = "*";
        public const String NIL_SYMBOL = "nil";
        public const int NILL = 11;
        public const String NILL_STRING = "is null";
        public const int NOTEQUAL = 10;
        public const String NOTEQUAL_STRING = "not equal to";
        public const String NOTEQUAL_SYMBOL = "!=";
        public const int NOTNILL = 12;
        public const int NUMERIC_INCLUSIVE = 23;

        /// --------- non-primitive types ---------///
        public const int OBJECT_TYPE = 9;

        public const String PCS = "java.beans.PropertyChangeSupport";
        public const String PCS_ADD = "add_PropertyChanged";
        public const String PCS_REMOVE = "remove_PropertyChanged";
        public const String PROPERTYCHANGELISTENER = "Creshendo.Util.PropertyChangedHandler";
        public const int RETURN_VOID_TYPE = 12;
        public const int SHORT_OBJECT = 15;
        public const int SHORT_PRIM_TYPE = 2;

        /// --------- native types for the rule engine ---------///
        public const int SLOT_TYPE = 100;

        public const int STRING_TYPE = 11;
        public const int SUBTRACT = 2;
        public const String SUBTRACT_STRING = "subtract";
        public const String SUBTRACT_SYMBOL = "-";
        public const int USERDEFINED = 13;

        public static readonly String FILE_SEPARATOR;
        public static readonly String LINEBREAK;
        public static String DEFAULT_OUTPUT = "t";
        public static String INITIAL_FACT = "_initialFact";
        public static String SHELL_PROMPT = "Creshendo> ";
        public static String VERSION = "0.8";

        static Constants()
        {
            LINEBREAK = Environment.NewLine;
            FILE_SEPARATOR = "HUH";
        }
    }
}