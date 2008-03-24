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
    /// *
    /// 
    /// </author>
    [Serializable]
    public class SubStringFunction : IFunction
    {
        public const String SUBSTRING = "sub-string";

        /// <summary> 
        /// </summary>
        public SubStringFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return SUBSTRING; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            String sub = null;
            if (params_Renamed != null && params_Renamed.Length == 3)
            {
                int begin = params_Renamed[0].IntValue;
                int end = params_Renamed[1].IntValue;
                String txt = params_Renamed[2].StringValue;
                sub = txt.Substring(begin, (end) - (begin));
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, sub);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(sub-string [begin] [end] [string])";
        }

        #endregion
    }
}