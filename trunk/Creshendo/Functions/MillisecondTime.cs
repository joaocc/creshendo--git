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
    /// EchoFunction is used to echo variable bindings in the shell.
    /// 
    /// </author>
    [Serializable]
    public class MillisecondTime : IFunction
    {
        public const String MSTIME = "ms-time";

        /// <summary> 
        /// </summary>
        public MillisecondTime()
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.LONG_OBJECT; }
        }

        public virtual String Name
        {
            get { return MSTIME; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (String[])}; }
        }


        /// <summary> The method expects an array of ShellBoundParam. The method will use
        /// StringBuffer to resolve the binding and print out 1 binding per
        /// line.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            //Decimal time = new Decimal((DateTime.Now.Ticks - 621355968000000000)/10000);
            Decimal time = new Decimal(DateTime.Now.Ticks);
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BIG_DECIMAL, time);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(ms-time)";
        }

        #endregion
    }
}