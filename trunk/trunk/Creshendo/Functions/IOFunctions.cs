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
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// IO Functions will initialize the IO related functions like printout,
    /// batch, etc.
    /// 
    /// </author>
    [Serializable]
    public class IOFunctions : IFunctionGroup
    {
        private List<Object> funcs;

        /// <summary> 
        /// </summary>
        public IOFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return "IO functions"; }
        }


        public virtual void loadFunctions(Rete engine)
        {
            BatchFunction b = new BatchFunction();
            engine.declareFunction(b);
            funcs.Add(b);
            LoadFactsFunction load = new LoadFactsFunction();
            engine.declareFunction(load);
            funcs.Add(load);
            PrintFunction pf = new PrintFunction();
            engine.declareFunction(pf);
            funcs.Add(pf);
        }

        public virtual IList listFunctions()
        {
            return funcs;
        }

        #endregion

        private void InitBlock()
        {
            funcs = new List<Object>();
        }
    }
}