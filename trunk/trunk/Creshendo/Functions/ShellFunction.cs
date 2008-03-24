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
    /// The purpose of Shell function is to make it easy to parse text in the shell
    /// and execute the real function. ShellFunction expects the parser to pass
    /// the name of the real function and parameter values.
    /// 
    /// </author>
    [Serializable]
    public class ShellFunction : IFunction
    {
        private IFunction actualFunction = null;
        public String funcName = null;
        private IParameter[] params_Renamed = null;

        /// <summary> 
        /// </summary>
        public ShellFunction()
        {
        }

        public virtual IParameter[] Parameters
        {
            get { return params_Renamed; }

            set { params_Renamed = value; }
        }

        public virtual IFunction Function
        {
            get { return actualFunction; }

            set { actualFunction = value; }
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return actualFunction.ReturnType; }
        }

        /// <summary>
        /// The name of the function to call
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public virtual String Name
        {
            get { return funcName; }

            set { funcName = value; }
        }

        public virtual Type[] Parameter
        {
            get { return actualFunction.Parameter; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            lookUpFunction(engine);
            if (this.params_Renamed != null && actualFunction != null)
            {
                return actualFunction.executeFunction(engine, this.params_Renamed);
            }
            else
            {
                DefaultReturnVector rv = new DefaultReturnVector();
                DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                rv.addReturnValue(rval);
                return rv;
            }
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            StringBuilder buf = new StringBuilder();
            return buf.ToString();
        }

        #endregion

        public virtual void lookUpFunction(Rete engine)
        {
            actualFunction = engine.findFunction(funcName);
        }

        public void setParameters(IParameter[] pms)
        {
            params_Renamed = pms;
        }
    }
}