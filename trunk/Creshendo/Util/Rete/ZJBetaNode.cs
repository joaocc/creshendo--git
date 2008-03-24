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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// ZJBetaNode is different than other BetaNodes in that it
    /// has no bindings. We optimize the performance for those
    /// cases by skipping evaluation and just propogate
    /// 
    /// </author>
    public class ZJBetaNode : BaseJoin
    {
        /// <summary> The operator for the join by default is equal. The the join
        /// doesn't comparing values, the operator should be set to -1.
        /// </summary>
        protected internal int operator_Renamed;

        public ZJBetaNode(int id) : base(id)
        {
            InitBlock();
        }

        /// <summary>
        /// Set the bindings for this join
        /// </summary>
        /// <value></value>
        public override Binding[] Bindings
        {
            set { }
        }

        private void InitBlock()
        {
            operator_Renamed = Constants.EQUAL;
        }


        /// <summary> Clear will Clear the lists
        /// </summary>
        public override void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = mem.getBetaLeftMemory(this);
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            
            // first we iterate over the list for each fact
            // and Clear it.
            foreach(object item in leftmem.Keys)
            {
                ((IBetaMemory) leftmem.Get(item)).clear();
            }
            
            /*
            IEnumerator itr = leftmem.Keys.GetEnumerator();
            // first we iterate over the list for each fact
            // and Clear it.
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) leftmem.Get(itr.Current);
                bmem.clear();
            }
            */

            // now that we've cleared the list for each fact, we
            // can Clear the org.jamocha.rete.util.Map.
            leftmem.Clear();
            rightmem.Clear();
        }

        /// <summary>
        /// assertLeft takes an array of facts. Since the Current join may be
        /// joining against one or more objects, we need to pass all
        /// previously matched facts.
        /// </summary>
        /// <param name="linx">The linx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = mem.getBetaLeftMemory(this);

            leftmem.Put(linx, linx);

            //foreach(IFact rfcts in ((IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this)).Values)
            //{
            //    propogateAssert(linx.add(rfcts), engine, mem);
            //}


            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);

            foreach(IFact rfcts in rightmem.Values)
            {
                propogateAssert(linx.add(rfcts), engine, mem);
            }

            //IEnumerator itr = rightmem.Values.GetEnumerator();
            //while (itr.MoveNext())
            //{
            //    IFact rfcts = (IFact)itr.Current;
            //    // now we propogate
            //    propogateAssert(linx.add(rfcts), engine, mem);
            //}
        }

        /// <summary>
        /// Assert from the right side is always going to be from an Alpha node.
        /// </summary>
        /// <param name="rfact"></param>
        /// <param name="engine"></param>
        /// <param name="mem"></param>
        public override void assertRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);
            rightmem.Put(rfact, rfact);
            IGenericMap<Object, Object> leftmem = mem.getBetaLeftMemory(this);

            foreach (Index bmem in leftmem.Values)
            {
                propogateAssert(bmem.add(rfact), engine, mem);
            }


            //IEnumerator itr = leftmem.Values.GetEnumerator();
            //while (itr.MoveNext())
            //{
            //    Index bmem = (Index) itr.Current;
            //    // now we propogate
            //    propogateAssert(bmem.add(rfact), engine, mem);
            //}
        }

        /// <summary>
        /// Retracting from the left requires that we propogate the
        /// </summary>
        /// <param name="linx">The linx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void retractLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = mem.getBetaLeftMemory(this);
            leftmem.Remove(linx);
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);

            foreach(IFact itr in rightmem.Values)
            {
                propogateRetract(linx.add(itr), engine, mem);
            }

            //IEnumerator itr = rightmem.Values.GetEnumerator();
            //while (itr.MoveNext())
            //{
            //    propogateRetract(linx.add((IFact) itr.Current), engine, mem);
            //}
        }

        /// <summary>
        /// Retract from the right works in the following order.
        /// 1. Remove the fact from the right memory
        /// 2. check which left memory matched
        /// 3. propogate the retract
        /// </summary>
        /// <param name="rfact"></param>
        /// <param name="engine"></param>
        /// <param name="mem"></param>
        public override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);
            rightmem.Remove(rfact);
            IGenericMap<Object, Object> leftmem = mem.getBetaLeftMemory(this);

            foreach (Index bmem in leftmem.Values)
            {
                propogateRetract(bmem.add(rfact), engine, mem);
            }


            //IEnumerator itr = leftmem.Values.GetEnumerator();
            //while (itr.MoveNext())
            //{
            //    Index bmem = (Index) itr.Current;
            //    // now we propogate
            //    propogateRetract(bmem.add(rfact), engine, mem);
            //}
        }

        /// <summary> Basic implementation will return string format of the betaNode
        /// </summary>
        public override String ToString()
        {
            return "ZJBetaNode";
        }

        /// <summary> implementation just returns the node id and the text
        /// zero-bind join.
        /// </summary>
        public override String toPPString()
        {
            return "ZJBetaNode-" + nodeID + "> ";
        }
    }
}