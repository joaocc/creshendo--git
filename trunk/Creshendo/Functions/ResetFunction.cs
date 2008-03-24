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
    /// Reset will reset all deffacts and objects. To reset just the objects,
    /// call reset-objects
    /// 
    /// </author>
    [Serializable]
    public class ResetFunction : IFunction
    {
        public const String RESET = "reset";

        /// <summary> 
        /// </summary>
        public ResetFunction() 
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
            get { return RESET; }
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
            engine.resetAll();
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(reset)";
        }

        #endregion
    }
}