/*
* Copyright 2006 Nikolaus Koemm
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
    /// <author>  Nikolaus Koemm
    /// *
    /// 
    /// </author>
    [Serializable]
    public class Const : IFunction
    {
        public const String CONST = "const";

        /// <summary> 
        /// </summary>
        public Const() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return CONST; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            double eq = 0;
            if (params_Renamed[0] != null)
            {
                String val = params_Renamed[0].StringValue;
                if (val.CompareTo("pi") == 0)
                {
                    eq = System.Math.PI;
                }
                else if (val.CompareTo("e") == 0)
                {
                    eq = System.Math.E;
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BIG_DECIMAL, eq);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(const e|pi)\n" + "Function description:\n" + "\te  return the value of the Euler constant,\n" + "\tpi returns the value of Pi.";
        }

        #endregion
    }
}