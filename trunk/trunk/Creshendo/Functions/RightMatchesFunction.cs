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
    /// The purpose of RightMatches is to print out the facts in the right
    /// side of BetaNodes. It isn't the same as matches function. Unlike
    /// matches, RightMatches prints out all the facts on the right side
    /// and doesn't show which facts it matches on the left.
    /// 
    /// </author>
    [Serializable]
    public class RightMatchesFunction : IFunction
    {
        public const String RIGHT_MATCHES = "right-matches";

        /// <summary> 
        /// </summary>
        public RightMatchesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return RIGHT_MATCHES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            engine.WorkingMemory.printWorkingMemoryBetaRight();
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(right-matches)\n" + "Function description:\n" + "\tPrints out the facts in the right side of BetaNodes,\n" + "\tand does not show which facts it matches on the left.";
        }

        #endregion
    }
}