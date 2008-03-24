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
using System.Diagnostics;
using System.IO;
using System.Text;
using Creshendo.Util.Messagerouter;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
//using ParseException=Creshendo.Util.Parser.Clips.ParseException;

namespace Creshendo.Functions
{
    /// <author>  Sebastian Reinartz
    /// 
    /// Functional equivalent of (eval "(+ 1 3)") in CLIPS and JESS.
    /// 
    /// </author>
    [Serializable]
    public class EvalFunction : IFunction
    {
        public const String EVAL = "eval";

        /// <summary> 
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary> 
        /// </summary>
        public EvalFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        public virtual String Name
        {
            get { return EVAL; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            IReturnVector result = null;
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                String command = (String) params_Renamed[0].getValue(engine, Constants.STRING_TYPE);
                if (command != null)
                {
                    result = eval(engine, command);
                }
            }
            return result;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(eval");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else if (params_Renamed[idx] is ValueParam)
                    {
                        buf.Append(" \"" + params_Renamed[idx].StringValue + "\"");
                    }
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(eval <string expressions>)\n" + "Command description:\n" + "\tEvaluates the content of a string.";
            }
        }

        #endregion

        public virtual IReturnVector eval(Rete engine, String command)
        {
            IReturnVector result = null;
            try
            {
                CLIPSParser parser = new CLIPSParser(engine, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(command)));
                CLIPSInterpreter interpreter = new CLIPSInterpreter(engine);
                Object expr = null;
                while ((expr = parser.basicExpr()) != null)
                {
                    result = interpreter.executeCommand(expr);
                }
            }
            catch (ParseException e)
            {
                // we should report the error
                Trace.WriteLine(e.Message);
            }
            return result;
        }
    }
}