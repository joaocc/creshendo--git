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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// AbstractTemporalNode is the base class for all temporal joins. For performance
    /// reasons, we will have several subclasses.
    /// 
    /// </author>
    public abstract class AbstractTemporalNode : BaseJoin
    {
        /// <summary> The relative elapsed time for the left side of the join.
        /// The default value is 0 to indicate it has no time window
        /// </summary>
        protected internal int leftElapsedTime = 0;

        /// <summary> the relative elapsed time for the right side of the join
        /// The default value is 0 to indicate it has no time window
        /// </summary>
        protected internal int rightElapsedTime = 0;

        public AbstractTemporalNode(int id) : base(id)
        {
        }

        protected internal virtual long RightTime
        {
            get
            {
                long time;
                long ts = (DateTime.Now.Ticks - 621355968000000000)/10000;
                if (rightElapsedTime > 0)
                {
                    time = ts - rightElapsedTime;
                }
                else
                {
                    time = 9223372036854775807L;
                }
                return time;
            }
        }

        protected internal virtual long LeftTime
        {
            get
            {
                long time;
                if (leftElapsedTime > 0)
                {
                    time = (DateTime.Now.Ticks - 621355968000000000)/10000 - leftElapsedTime;
                }
                else
                {
                    time = 9223372036854775807L;
                }
                return time;
            }
        }

        public virtual int LeftElapsedTime
        {
            get { return leftElapsedTime; }

            set { leftElapsedTime = value; }
        }

        public virtual int RightElapsedTime
        {
            get { return rightElapsedTime; }

            set { rightElapsedTime = value; }
        }

        /// <summary> assertLeft takes an array of facts. Since the Current join may be joining
        /// against one or more objects, we need to pass all previously matched
        /// facts.
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract override void assertLeft(Index linx, Rete engine, IWorkingMemory mem);

        /// <summary> Assert from the right side is always going to be from an Alpha node.
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract override void assertRight(IFact rfact, Rete engine, IWorkingMemory mem);

        /// <summary> Retracting from the left requires that we propogate the
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract override void retractLeft(Index linx, Rete engine, IWorkingMemory mem);

        /// <summary> Retract from the right works in the following order. 1. Remove the fact
        /// from the right memory 2. check which left memory matched 3. propogate the
        /// retract
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem);

        /// <summary> evaluate will first compare the timestamp of the last fact in the fact
        /// array of the left and make sure the fact is still fresh. if it is not
        /// fresh, the method returns false.
        /// </summary>
        /// <param name="">leftlist
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact[] leftlist, IFact right, long time)
        {
            bool eval = true;
            // first we compare the timestamp of the last fact in the
            // fact array. the last fact should be the fact with a
            // relative time window
            if (leftlist[leftlist.Length - 1].timeStamp() > time)
            {
                // we iterate over the binds and evaluate the facts
                for (int idx = 0; idx < binds.Length; idx++)
                {
                    // we got the binding
                    Binding bnd = binds[idx];
                    eval = bnd.evaluate(leftlist, right);
                    if (!eval)
                    {
                        break;
                    }
                }
                return eval;
            }
            else
            {
                return false;
            }
        }


        /// <summary> Basic implementation will return string format of the betaNode
        /// </summary>
        public abstract override String ToString();

        /// <summary> returns the node named + node id and the bindings in a string format
        /// </summary>
        public abstract override String toPPString();
    }
}