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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// PrintFucntion is pretty simple. It can any number of parameters and
    /// print it.
    /// 
    /// </author>
    [Serializable]
    public class PrintFunction : IFunction
    {
        public const String PRINTOUT = "printout";

        /// <summary> 
        /// </summary>
        public PrintFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.Function#getName()
			*/

            get { return PRINTOUT; }
        }

        /// <summary> The implementation returns an array of Count 1 with Parameter.class
        /// as the only entry. Any function that can take an unlimited number
        /// of Parameters should return new Class[] {Parameter.class}.
        /// If a function doesn't take any parameters, the method should return
        /// null instead.
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        /// <summary> The implementation will call Rete.writeMessage(). This means that
        /// if multiple output streams are set, the message will be printed to
        /// all of them.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            // print out some stuff
            if (params_Renamed.Length > 0)
            {
                String output = params_Renamed[0].StringValue;
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        Object v = engine.getBinding(bp.VariableName);
                        if (v.GetType().IsArray)
                        {
                            Object[] ary = (Object[]) v;
                            writeArray(ary, engine, output, false);
                        }
                        else
                        {
                            engine.writeMessage(v.ToString(), output);
                        }
                    }
                    else if (params_Renamed[idx].Value != null && params_Renamed[idx].Value.Equals(Constants.CRLF))
                    {
                        engine.writeMessage(Constants.LINEBREAK, output);
                    }
                    else
                    {
                        Object val = params_Renamed[idx].Value;
                        if (val is String)
                        {
                            engine.writeMessage((String) val, output);
                        }
                        else if (val.GetType().IsArray)
                        {
                            Object[] ary = (Object[]) val;
                            writeArray(ary, engine, output, true);
                        }
                        else
                        {
                            engine.writeMessage(val.ToString(), output);
                        }
                    }
                }
            }
            // there's nothing to return, so just return a new DefaultReturnVector
            return new DefaultReturnVector();
        }


        /// <summary> Note: need to handle crlf correctly, for now leave it as is.
        /// </summary>
        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(print ");
                buf.Append(params_Renamed[0].StringValue);
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else
                    {
                        buf.Append(" \"" + params_Renamed[idx].StringValue + "\"");
                    }
                }
                buf.Append(" )");
                return buf.ToString();
            }
            else
            {
                return "(print)";
            }
        }

        #endregion

        public virtual void writeArray(Object[] arry, Rete engine, String output, bool linebreak)
        {
            for (int idz = 0; idz < arry.Length; idz++)
            {
                Object val = arry[idz];
                if (val is IFact)
                {
                    IFact f = (IFact) val;
                    engine.writeMessage(f.toFactString() + " ", output);
                }
                else
                {
                    engine.writeMessage(arry[idz].ToString() + " ", output);
                }
                if (linebreak)
                {
                    engine.writeMessage(Constants.LINEBREAK, output);
                }
            }
        }
    }
}