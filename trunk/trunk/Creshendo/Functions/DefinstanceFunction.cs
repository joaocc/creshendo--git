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
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// Definstance will assert an object instance using Rete.assert(Object).
    /// 
    /// </author>
    [Serializable]
    public class DefinstanceFunction : IFunction
    {
        public const String DEFINSTANCE = "definstance";

        public DefinstanceFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return DEFINSTANCE; }
        }

        /// <summary> The function expects a single BoundParam that is an object binding
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            String asrt = "";
            if (params_Renamed.Length >= 1 && params_Renamed[0].Value != null)
            {
                Object obj = params_Renamed[0].Value;
                String template = "";
                if (params_Renamed.Length == 2 && params_Renamed[1].StringValue != null)
                {
                    template = params_Renamed[1].StringValue;
                }
                try
                {
                    engine.assertObject(obj, template, false, true);
                    asrt = "true";
                }
                catch (AssertException e)
                {
                    // we should log this and output an error
                    asrt = "false";
                }
            }
            else
            {
                asrt = "false";
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, asrt);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                return buf.ToString();
            }
            else
            {
                return "(definstance )";
            }
        }

        #endregion
    }
}