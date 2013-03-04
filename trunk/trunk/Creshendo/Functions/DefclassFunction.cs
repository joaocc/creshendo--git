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
    /// 
    /// 
    /// </author>
    [Serializable]
    public class DefclassFunction : IFunction
    {
        public const String DEFCLASS = "defclass";

        public DefclassFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return DEFCLASS; }
        }

        /// <summary> defclass function expects 3 parameters. (defclass classname,
        /// templatename, parenttemplate) parent template name is optional.
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool def = true;
            if (params_Renamed.Length >= 0)
            {
                String clazz = params_Renamed[0].StringValue;
                String template = null;
                if (params_Renamed[1] != null)
                {
                    template = params_Renamed[1].StringValue;
                }
                String parent = null;
                if (params_Renamed.Length == 3)
                {
                    parent = params_Renamed[2].StringValue;
                }
                try
                {
                    engine.declareObject(clazz, template, parent);
                }
                catch (Exception e)
                {
                    def = false;
                }
            }
            else
            {
                def = false;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, def);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(defclass");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    buf.Append(" " + params_Renamed[idx].StringValue);
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(defclass [new classname] [template] [parent template])";
            }
        }

        #endregion
    }
}