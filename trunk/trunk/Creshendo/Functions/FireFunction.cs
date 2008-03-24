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
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// 
    /// </author>
    [Serializable]
    public class FireFunction : IFunction
    {
        public const String FIRE = "fire";

        /// <summary> 
        /// </summary>
        public FireFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.INTEGER_OBJECT; }
        }

        public virtual String Name
        {
            get { return FIRE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            int count = 0;
            if (params_Renamed != null && params_Renamed.Length == 1)
            {
                int fc = params_Renamed[0].IntValue;
                try
                {
                    count = engine.fire(fc);
                }
                catch (ExecuteException e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
            else
            {
                count = engine.fire();
            }
            // engine.writeMessage(String.valueOf(count) + Constants.LINEBREAK,"t");
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.INTEGER_OBJECT, count);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(fire)";
        }

        #endregion
    }
}