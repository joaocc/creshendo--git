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
using Creshendo.Functions;



namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// TestNode extends BaseJoin. TestNode is used to evaluate functions.
    /// It may use values or bindings as parameters for the functions. The
    /// left input is where the facts would enter. The right input is a
    /// dummy input, since no facts actually enter.
    /// 
    /// </author>
    public class NTestNode : BaseJoin
    {
        /// <summary> TestNode can only have 1 top level function
        /// </summary>
        protected internal Function func = null;

        /// <summary> the parameters to pass to the function
        /// </summary>
        protected internal Parameter[] params_Renamed = null;

        /// <summary> by default the string is null, until the first time
        /// toPPString is called.
        /// </summary>
        private String ppstring = null;

        /// <param name="">id
        /// 
        /// </param>
        public NTestNode(int id, Function func, Parameter[] parameters) : base(id)
        {
            this.func = func;
            params_Renamed = parameters;
        }

        /// <summary> for TestNode, setbindings does not apply
        /// </summary>
        public override Binding[] Bindings
        {
            set { }
        }

        protected internal virtual Fact[] Parameters
        {
            set
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        ((BoundParam) params_Renamed[idx]).Facts = value;
                    }
                    else if (params_Renamed[idx] is FunctionParam)
                    {
                        ((FunctionParam) params_Renamed[idx]).Facts = value;
                    }
                }
            }
        }

        public virtual void lookUpFunction(Rete engine)
        {
            if (func is ShellFunction)
            {
                ShellFunction sf = (ShellFunction) func;
                sf.lookUpFunction(engine);
                if (sf.Function != null)
                {
                    func = sf.Function;
                }
            }
        }

        /// <summary> Assert will first pass the facts to the parameters. Once the
        /// parameters are set, it should call execute to get the result.
        /// </summary>
        public override void assertLeft(Index linx, Rete engine, WorkingMemory mem)
        {
            Map leftmem = (Map) mem.getBetaLeftMemory(this);
            if (!leftmem.containsKey(linx))
            {
                Parameters = linx.Facts;
                ReturnVector rv = func.executeFunction(engine, params_Renamed);
                if (!rv.firstReturnValue().BooleanValue)
                {
                    BetaMemory bmem = new BetaMemoryImpl(linx);
                    leftmem.put(bmem.Index, bmem);
                }
                // only propogate if left memories count is zero
                if (leftmem.size() == 0)
                {
                    propogateAssert(linx, engine, mem);
                }
            }
        }

        /// <summary> Since the assertRight is a dummy, it doesn't do anything.
        /// </summary>
        public override void assertRight(Fact rfact, Rete engine, WorkingMemory mem)
        {
        }

        /// <summary> 
        /// </summary>
        public override void retractLeft(Index linx, Rete engine, WorkingMemory mem)
        {
            Map leftmem = (Map) mem.getBetaLeftMemory(this);
            int prev = leftmem.size();
            if (leftmem.containsKey(linx))
            {
                // the memory contains the key, so we retract and propogate
                leftmem.remove(linx);
            }
            if (prev != 0 && leftmem.size() == 0)
            {
                propogateRetract(linx, engine, mem);
            }
        }


        /// <summary> retract right is a dummy, so it does nothing.
        /// </summary>
        public override void retractRight(Fact rfact, Rete engine, WorkingMemory mem)
        {
        }

        /// <summary> clear the memory
        /// </summary>
        public override void clear(WorkingMemory mem)
        {
            ((Map) mem.getBetaLeftMemory(this)).clear();
        }


        /// <summary> Still need to implement the method to return string
        /// format of the node
        /// </summary>
        public override String ToString()
        {
            return "(test (" + func.Name + ") )";
        }

        /// <summary> 
        /// </summary>
        public override String toPPString()
        {
            if (ppstring == null)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("TestNode-" + nodeID + "> (test (" + func.Name);
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else if (params_Renamed[idx] is ValueParam)
                    {
                        ValueParam vp = (ValueParam) params_Renamed[idx];
                        buf.Append(" " + vp.StringValue);
                    }
                }
                buf.Append(") )");
                ppstring = buf.ToString();
            }
            return ppstring;
        }
    }
}