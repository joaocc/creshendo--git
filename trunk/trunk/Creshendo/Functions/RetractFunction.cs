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
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Functions
{
    [Serializable]
    public class RetractFunction : IFunction
    {
        public const String RETRACT = "retract";

        public RetractFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return RETRACT; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector rv = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length >= 1)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        Deffact fact = (Deffact) bp.Fact;
                        try
                        {
                            if (bp.ObjectBinding)
                            {
                                engine.retractObject(fact.ObjectInstance);
                            }
                            else
                            {
                                engine.retractFact(fact);
                            }
                            DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, true);
                            rv.addReturnValue(rval);
                        }
                        catch (RetractException e)
                        {
                            DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                            rv.addReturnValue(rval);
                        }
                    }
                    else if (params_Renamed[idx] is ValueParam)
                    {
                        Decimal bi = params_Renamed[idx].BigDecimalValue;
                        try
                        {
                            engine.retractById(Decimal.ToInt64(bi));
                            DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, true);
                            rv.addReturnValue(rval);
                        }
                        catch (RetractException e)
                        {
                            DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                            rv.addReturnValue(rval);
                        }
                    }
                }
            }
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(retract [?binding|fact-id])\n" + "Function description:\n" + "\tAllows the user to Remove facts from the fact-list.";
        }

        #endregion
    }
}