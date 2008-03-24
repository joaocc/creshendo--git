/*
* Copyright 2002-2007 Peter Lin
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
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  pete
    /// *
    /// 
    /// </author>
    public class DeffunctionGroup : IFunctionGroup
    {
        //UPGRADE_NOTE: The initialization of  'funcs' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IList funcs;

        /// <summary> 
        /// </summary>
        public DeffunctionGroup()
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return typeof (DeffunctionGroup).FullName; }
        }


        /* (non-Javadoc)
		* @see org.jamocha.rete.FunctionGroup#listFunctions()
		*/

        public virtual IList listFunctions()
        {
            return funcs;
        }

        /// <summary> At engine initialization time, the function group doesn't
        /// have any functions.
        /// </summary>
        public virtual void loadFunctions(Rete engine)
        {
        }

        #endregion

        private void InitBlock()
        {
            funcs = new List<Object>();
        }

        /// <summary> Add a function to the group
        /// </summary>
        /// <param name="">f
        /// 
        /// </param>
        public virtual void addFunction(IFunction f)
        {
            funcs.Add(f);
        }
    }
}