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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// LazyAgenda is used to turn on/off lazy agenda. That means the
    /// activations are not sorted when added to the agenda. Instead,
    /// it's sorted when they are removed.
    /// 
    /// </author>
    [Serializable]
    public class LazyAgendaFunction : IFunction
    {
        public const String LAZY_AGENDA = "lazy-agenda";

        /// <summary> 
        /// </summary>
        public LazyAgendaFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return LAZY_AGENDA; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool exec = false;
            String mode = "normal";
            DefaultReturnVector rv = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length == 1)
            {
                exec = true;
                ValueParam vp = (ValueParam) params_Renamed[0];
                if (vp.StringValue.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    engine.CurrentFocus.Lazy = true;
                    mode = "lazy";
                }
                else if (vp.StringValue.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                {
                    engine.CurrentFocus.Lazy = false;
                }
            }
            DefaultReturnValue drv = new DefaultReturnValue(Constants.STRING_TYPE, mode);
            rv.addReturnValue(drv);
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(lazy-agenda [true|false])";
        }

        #endregion
    }
}