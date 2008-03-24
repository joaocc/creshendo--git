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
    /// 
    /// </author>
    [Serializable]
    public class DefmoduleFunction : IFunction
    {
        public const String DEFMODULE = "defmodule";

        public DefmoduleFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return DEFMODULE; }
        }

        /// <summary> The expected parameter is a single ValueParam containing a deftemplate
        /// instance. The function gets the deftemplate using Parameter.getValue().
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool add = true;
            if (params_Renamed.Length == 1)
            {
                engine.addModule(params_Renamed[0].StringValue);
                engine.writeMessage("true", Constants.DEFAULT_OUTPUT);
            }
            else
            {
                add = false;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, add);
            ret.addReturnValue(rv);
            DefaultReturnValue rv2 = new DefaultReturnValue(Constants.STRING_TYPE, params_Renamed[0].StringValue);
            ret.addReturnValue(rv2);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null)
            {
                StringBuilder buf = new StringBuilder();
                return buf.ToString();
            }
            else
            {
                return "(defmodule name)";
            }
        }

        #endregion
    }
}