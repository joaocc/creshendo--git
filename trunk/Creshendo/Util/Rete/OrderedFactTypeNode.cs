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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <summary> OrderedFactTypeNode is the object type node for ordered facts. The
    /// difference between the two is this type node is used only for
    /// ordered facts. Jamocha groups the ordered facts by the number of slots
    /// first. The second thing is it uses a HashMap for the successors, since
    /// the first slot in the ordered fact is a symbol. This means it is always
    /// an equality test, which means we shouldn't iterate over all successors.
    /// The class should just look it up and only propogate to the one node.
    /// </summary>
    /// <author>  Peter Lin
    /// 
    /// </author>
    [Serializable]
    public class OrderedFactTypeNode : BaseAlpha
    {
        /// <summary> The Class that defines object type
        /// </summary>
        private ITemplate deftemplate = null;

        /// <summary> HashMap entries for unique AlphaNodes
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'entries' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IGenericMap<object, object> entries;

        /// <param name="">id
        /// 
        /// </param>
        public OrderedFactTypeNode(int id, ITemplate tmpl) : base(id)
        {
            InitBlock();
            deftemplate = tmpl;
        }

        private void InitBlock()
        {
            entries = CollectionFactory.localMap();
        }

        public override void assertFact(IFact factInstance, Rete engine, IWorkingMemory mem)
        {
        }

        public override String hashString()
        {
            // TODO Auto-generated method stub
            return null;
        }

        public override void retractFact(IFact factInstance, Rete engine, IWorkingMemory mem)
        {
            // TODO Auto-generated method stub
        }

        public override String toPPString()
        {
            return "InputNode for Template(" + deftemplate.Name + ")";
        }

        public override String ToString()
        {
            return "OrderedFactTypeNode(" + deftemplate.Name + ")";
        }
    }
}