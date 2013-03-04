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
    public class LoadFunctionsFunction : IFunction
    {
        public const String LOAD_FUNCTION = "load-function";

        public LoadFunctionsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return LOAD_FUNCTION; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool load = false;
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    String func = params_Renamed[idx].StringValue;
                    try
                    {
                        engine.declareFunction(func);
                        load = true;
                    }
                    catch (Exception e)
                    {
                        load = false;
                    }
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, load);
            ret.addReturnValue(rv);
            return ret;
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rete.Function#toPPString(woolfel.engine.rete.Parameter[], int)
		*/

        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(load-function [classname])";
        }

        #endregion
    }
}