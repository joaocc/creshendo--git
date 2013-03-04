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
using Creshendo.Util;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <summary> DeffunctionFunction is used for functions that are declared in the
    /// shell. It is different than a function written in java.
    /// Deffunction run interpreted and are mapped to existing
    /// functions.
    /// 
    /// </summary>
    /// <author>  Peter Lin
    /// 
    /// </author>
    public class DeffunctionFunction : IFunction
    {
        protected internal Type[] functionParams = null;
        protected internal IList functions = null;
        protected internal String name = null;
        protected internal IParameter[] parameters = null;
        protected internal String ppString = null;
        protected internal int returnType;

        /// <summary> 
        /// </summary>
        public DeffunctionFunction()
        {
        }

        public virtual String PPString
        {
            set { ppString = value; }
        }

        public virtual IList Function
        {
            get { return functions; }

            set { functions = value; }
        }

        public virtual IParameter[] Parameters
        {
            get { return parameters; }

            set { parameters = value; }
        }

        #region Function Members

        public virtual String Name
        {
            get { return name; }

            set { name = value; }
        }

        public virtual Type[] Parameter
        {
            get { return functionParams; }
        }

        public virtual int ReturnType
        {
            get { return returnType; }
        }

        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            bool add = false;
            if (engine.findFunction(name) == null)
            {
                // first we Get the actual function from the shell function
                IFunction[] functions = new IFunction[this.functions.Count];
                IParameter[][] parameters = new IParameter[this.functions.Count][];
                for (int i = 0; i < functions.Length; ++i)
                {
                    ShellFunction sf = (ShellFunction) this.functions[i];
                    functions[i] = engine.findFunction(sf.Name);
                    parameters[i] = sf.Parameters;
                }
                InterpretedFunction intrfunc = new InterpretedFunction(name, this.parameters, functions, parameters);
                intrfunc.configureFunction(engine);
                engine.declareFunction(intrfunc);
                add = true;
            }

            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, add);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return ppString;
        }

        #endregion

        public void setParameters(IParameter[] parameters1)
        {
            parameters = parameters1;
        }

        public void setFunction(IList infunc)
        {
            functions = infunc;
        }
    }
}