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
using Creshendo.Functions;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// Binding2 is used for bindings that are are numeric comparison like
    /// 
    /// 
    /// </author>
    public class Binding2 : Binding
    {
        protected internal IFunction function = null;
        protected internal int operator_Renamed;
        protected internal IParameter[] params_Renamed = null;
        protected internal String rightVariable = null;

        /// <summary> 
        /// </summary>
        public Binding2(int operator_Renamed) 
        {
            InitBlock();
            this.operator_Renamed = operator_Renamed;
        }

        public virtual int Operator
        {
            get { return operator_Renamed; }
        }

        public virtual IFunction Function
        {
            get { return function; }

            set { function = value; }
        }

        public virtual bool Predicate
        {
            get { return isPredJoin; }

            set { isPredJoin = value; }
        }

        public virtual IParameter[] Params
        {
            get { return params_Renamed; }

            set { params_Renamed = value; }
        }

        public virtual String RightVariable
        {
            get { return rightVariable; }

            set { rightVariable = value; }
        }

        private void InitBlock()
        {
            operator_Renamed = Constants.EQUAL;
        }

        //UPGRADE_NOTE: The initialization of  'operator_Renamed' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'


        public virtual bool evaluate(IFact[] left, IFact right, Rete engine)
        {
            if (function != null)
            {
                IReturnVector rv = function.executeFunction(engine, params_Renamed);
                return rv.firstReturnValue().BooleanValue;
            }
            else if (left.Length > leftrow)
            {
                if (left[leftrow] == right)
                {
                    return false;
                }
                return Evaluate.evaluate(operator_Renamed, left[leftrow].getSlotValue(leftIndex), right.getSlotValue(rightIndex));
            }
            else
            {
                return false;
            }
        }

        public override String toBindString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + leftrow + ")(");
            buf.Append(leftIndex);
            if (function != null)
            {
                buf.Append(") " + function.Name + " (0)(");
            }
            else
            {
                buf.Append(") " + ConversionUtils.getPPOperator(operator_Renamed) + " (0)(");
            }
            buf.Append(rightIndex);
            buf.Append(") ?" + rightVariable);
            return buf.ToString();
        }

        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("?" + varName + " (" + leftrow + ")(");
            buf.Append(leftIndex);
            if (function != null)
            {
                buf.Append(") " + function.Name + " (0)(");
            }
            else
            {
                buf.Append(") " + ConversionUtils.getPPOperator(operator_Renamed) + " (0)(");
            }
            buf.Append(rightIndex);
            buf.Append(") ?" + rightVariable);
            return buf.ToString();
        }
    }
}