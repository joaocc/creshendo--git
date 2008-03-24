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
    /// WatchFunction allows users to watch different engine process, like
    /// activations, facts and rules.
    /// 
    /// </author>
    [Serializable]
    public class UnWatchFunction : IFunction
    {
        protected internal const String UNWATCH = "unwatch";

        /// <summary> 
        /// </summary>
        public UnWatchFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return UNWATCH; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            if (params_Renamed != null)
            {
                // the params are not null, now check the parameter count
                if (params_Renamed.Length > 0)
                {
                    for (int idx = 0; idx < params_Renamed.Length; idx++)
                    {
                        String cmd = params_Renamed[idx].StringValue;
                        setWatch(engine, cmd);
                    }
                }
                else
                {
                    // we do nothing, maybe we should return a message
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(unwatch)";
        }

        #endregion

        protected internal virtual void setWatch(Rete engine, String cmd)
        {
            if (cmd.Equals("all"))
            {
                engine.UnWatch = WatchType.WATCH_ALL;
            }
            else if (cmd.Equals("facts"))
            {
                engine.UnWatch = WatchType.WATCH_FACTS;
            }
            else if (cmd.Equals("activations"))
            {
                engine.UnWatch = WatchType.WATCH_ACTIVATIONS;
            }
            else if (cmd.Equals("rules"))
            {
                engine.UnWatch = WatchType.WATCH_RULES;
            }
        }
    }
}