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
    /// TODO To change the template for this generated type comment go to
    /// 
    /// </author>
    [Serializable]
    public class ResetFactsFunction : IFunction
    {
        public const String RESET_FACTS = "reset-facts";

        /// <summary> 
        /// </summary>
        public ResetFactsFunction() 
        {
        }

        #region Function Members

        /// <summary> the function does not return anything
        /// </summary>
        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return RESET_FACTS; }
        }

        /// <summary> reset does not take any parameters
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        /// <summary> current implementation will call Rete.resetAll. This means it
        /// will reset all objects and deffacts.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            engine.resetFacts();
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(reset)";
        }

        #endregion
    }
}