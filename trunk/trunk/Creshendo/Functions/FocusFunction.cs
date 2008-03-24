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
    /// <author>  Sebastian Reinartz
    /// *
    /// 
    /// </author>
    [Serializable]
    public class FocusFunction : IFunction
    {
        public const String FOCUS = "focus";

        public FocusFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return FOCUS; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            String focus = null;
            if (params_Renamed.Length == 0)
            {
                focus = engine.CurrentFocus.ModuleName;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, focus);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(set-focus)";
        }

        #endregion
    }
}