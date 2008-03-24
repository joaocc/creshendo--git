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
    /// ProfileFunction is used to turn on profiling. It provides basic
    /// profiling of assert, retract, Add activation, Remove activation
    /// and fire.
    /// 
    /// </author>
    [Serializable]
    public class UnProfileFunction : IFunction
    {
        public const String PROFILE = "unprofile";

        /// <summary> 
        /// </summary>
        public UnProfileFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return PROFILE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx].StringValue.Equals("all"))
                    {
                        engine.Profile = ProfileType.PROFILE_ALL;
                    }
                    else if (params_Renamed[idx].StringValue.Equals("assert-fact"))
                    {
                        engine.Profile = ProfileType.PROFILE_ASSERT;
                    }
                    else if (params_Renamed[idx].StringValue.Equals("Add-activation"))
                    {
                        engine.Profile = ProfileType.PROFILE_ADD_ACTIVATION;
                    }
                    else if (params_Renamed[idx].StringValue.Equals("fire"))
                    {
                        engine.Profile = ProfileType.PROFILE_FIRE;
                    }
                    else if (params_Renamed[idx].StringValue.Equals("retract-fact"))
                    {
                        engine.Profile = ProfileType.PROFILE_RETRACT;
                    }
                    else if (params_Renamed[idx].StringValue.Equals("Remove-activation"))
                    {
                        engine.Profile = ProfileType.PROFILE_RM_ACTIVATION;
                    }
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(unprofile assert|all|retract|fire|Add-activation|Remove-activation)";
        }

        #endregion
    }
}