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
    /// ClearFunction will call Rete.Clear()
    /// 
    /// </author>
    [Serializable]
    public class ClearFunction : IFunction
    {
        public const String CLEAR = "Clear";

        /// <summary> 
        /// </summary>
        public ClearFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return CLEAR; }
        }

        /// <summary> The function does not take any parameters
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length == 1)
            {
                if (params_Renamed[0].StringValue.Equals("objects"))
                {
                    engine.clearObjects();
                }
                else if (params_Renamed[0].StringValue.Equals("deffacts"))
                {
                    engine.clearFacts();
                }
            }
            else
            {
                engine.clearAll();
            }
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, true);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (indents > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < indents; idx++)
                {
                    buf.Append(" ");
                }
                buf.Append("(Clear)");
                return buf.ToString();
            }
            else
            {
                return "(Clear [objects | deffacts])\n" + "Function description:\n" + "\tRemoves all the facts from memory and resets the fact index\n" + "\tif no argument is provided.\n" + "\tThe argument \"objects\" removes all the facts and\n" + "\tthe argument \"deffacts\" clears all the defined facts.\n";
            }
        }

        #endregion
    }
}