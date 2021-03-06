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
    /// InitialFact is used for rules without conditions and cases where a rule
    /// starts with exist or not.
    /// 
    /// </author>
    public class InitialFact : Deftemplate
    {
        /// <summary> 
        /// </summary>
        public InitialFact() : base(Constants.INITIAL_FACT)
        {
            slots = new Slot[0];
        }
    }
}