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
namespace Creshendo.Util.Rete
{
    /// <summary> IFLIANode is a special Left-Input Adapater node which has alpha memory.
    /// LIANode has no memory to make the engine more efficient, but the left
    /// input adapter for the InitialFact needs the memory to make sure rules
    /// that start with NOTCE work properly. If we don't, the user has to 
    /// execute (reset) function, so the rule will fire correctly.
    /// </summary>
    /// <author>  woolfel
    /// *
    /// 
    /// </author>
    public class IFLIANode : LIANode
    {
        public IFLIANode(int id) : base(id)
        {
        }

        /// <summary> the implementation just propogates the assert down the network
        /// </summary>
        public override void assertFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
            alpha.addPartialMatch(fact);
            propogateAssert(fact, engine, mem);
        }

        /// <summary> Retract simply propogates it down the network
        /// </summary>
        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
            if (alpha.removePartialMatch(fact) != null)
            {
                propogateRetract(fact, engine, mem);
            }
        }
    }
}