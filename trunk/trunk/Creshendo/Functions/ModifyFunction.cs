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
using System.Text;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// ModifyFunction is equivalent to CLIPS modify function.
    /// 
    /// </author>
    [Serializable]
    public class ModifyFunction : IFunction
    {
        public const String MODIFY = "modify";

        protected internal IFact[] triggerFacts = null;

        /// <summary> 
        /// </summary>
        public ModifyFunction() 
        {
        }

        public virtual IFact[] TriggerFacts
        {
            set { triggerFacts = value; }
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.Function#getName()
			*/

            get { return MODIFY; }
        }

        /// <summary> The current implementation expects 3 parameters in the following
        /// sequence:<br/>
        /// BoundParam
        /// SlotParam[]
        /// <br/>
        /// Example: (modify ?boundVariable (slotName value)* )
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam), typeof (SlotParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool exec = false;
            if (engine != null && params_Renamed != null && params_Renamed.Length >= 2 && params_Renamed[0].ObjectBinding)
            {
                BoundParam bp = (BoundParam) params_Renamed[0];
                Deffact fact = (Deffact) bp.Fact;
                try
                {
                    // first retract the fact
                    engine.retractFact(fact);
                    // now modify the fact
                    SlotParam[] sp = new SlotParam[params_Renamed.Length - 1];
                    for (int idx = 0; idx < sp.Length; idx++)
                    {
                        IParameter p = params_Renamed[idx + 1];
                        if (p is SlotParam)
                        {
                            sp[idx] = (SlotParam) p;
                        }
                    }
                    fact.updateSlots(engine, convertToSlots(sp, fact.Deftemplate));
                    if (fact.hasBinding())
                    {
                        fact.resolveValues(engine, triggerFacts);
                        fact = fact.cloneFact();
                    }
                    // now assert the fact using the same fact-id
                    engine.assertFact(fact);
                    exec = true;
                }
                catch (RetractException e)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    engine.writeMessage(e.Message);
                }
                catch (AssertException e)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                    engine.writeMessage(e.Message);
                }
            }

            DefaultReturnVector rv = new DefaultReturnVector();
            DefaultReturnValue rval = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, exec);
            rv.addReturnValue(rval);
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(modify ");
                buf.Append("?" + ((BoundParam) params_Renamed[0]).VariableName + " ");
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    // the parameter should be a deffact
                    SlotParam sp = (SlotParam) params_Renamed[idx];
                    Slot s = sp.SlotValue;
                    if (s.Value is BoundParam)
                    {
                        buf.Append("(" + s.Name + " ?" + ((BoundParam) s.Value).VariableName + ")");
                    }
                    else
                    {
                        buf.Append("(" + s.Name + " " + s.Value + ")");
                    }
                }
                buf.Append(" )");
                return buf.ToString();
            }
            else
            {
                return "(modify [binding] [deffact])\n" + "Function description:\n" + "\tAllows the user to modify template facts on the fact-list.";
            }
        }

        #endregion

        /// <summary> convert the SlotParam to Slot objects
        /// </summary>
        /// <param name="">params
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual Slot[] convertToSlots(IParameter[] params_Renamed, Deftemplate templ)
        {
            Slot[] slts = new Slot[params_Renamed.Length];
            for (int idx = 0; idx < params_Renamed.Length; idx++)
            {
                slts[idx] = ((SlotParam) params_Renamed[idx]).SlotValue;
                int col = templ.getColumnIndex(slts[idx].Name);
                if (col != - 1)
                {
                    slts[idx].Id = col;
                }
            }
            return slts;
        }
    }
}