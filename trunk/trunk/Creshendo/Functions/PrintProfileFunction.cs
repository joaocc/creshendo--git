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
using Creshendo.Util;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// PrintProfileFunction will print out the profile information.
    /// 
    /// </author>
    [Serializable]
    public class PrintProfileFunction : IFunction
    {
        public const String PRINT_PROFILE = "print-profile";

        /// <summary> 
        /// </summary>
        public PrintProfileFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return PRINT_PROFILE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            engine.writeMessage("fire ET=" + ProfileStats.fireTime + " ms" + Constants.LINEBREAK, "t");
            engine.writeMessage("assert ET=" + ProfileStats.assertTime + " ms" + Constants.LINEBREAK, "t");
            engine.writeMessage("retract ET=" + ProfileStats.retractTime + " ms" + Constants.LINEBREAK, "t");
            engine.writeMessage("Add Activation ET=" + ProfileStats.addActivation + " ms" + Constants.LINEBREAK, "t");
            engine.writeMessage("Remove Activation ET=" + ProfileStats.rmActivation + " ms" + Constants.LINEBREAK, "t");
            engine.writeMessage("Activation added=" + ProfileStats.addcount + Constants.LINEBREAK, "t");
            engine.writeMessage("Activation removed=" + ProfileStats.rmcount + Constants.LINEBREAK, "t");
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(print-profile)";
        }

        #endregion
    }
}