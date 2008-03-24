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
namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// BaseAlpha2 is an abstract class for AlphaNodes that compare literal or bound
    /// constraints. It isn't used for LIANode, ObjectTypeNode.
    /// 
    /// </author>
    public abstract class BaseAlpha2 : BaseAlpha
    {
        /// <param name="">id
        /// 
        /// </param>
        public BaseAlpha2(int id) : base(id)
        {
        }

        /// <summary> set the operator type for the node
        /// </summary>
        /// <param name="">opr
        /// 
        /// </param>
        public new abstract int Operator { get; set; }

        /// <summary> set the slot for the node
        /// </summary>
        /// <param name="">sl
        /// 
        /// </param>
        public abstract Slot Slot { set; }
    }
}