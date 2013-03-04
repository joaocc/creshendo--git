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
using Creshendo.Util.Rete.Strategies;

namespace Creshendo.Util.Rete
{
    //using ListIterator = java.util.ListIterator;
    /// <author>  Peter Lin
    /// *
    /// LinkedActivationWrapper is a container for LinkedActivation. It provdes
    /// the logic for modifying a Creshendo.rete.util.LinkedList created from LinkedActivation.
    /// Null values are not permitted, and are silently ignored. Generally speaking,
    /// it doesn't make sense to Add a null activation to the agenda.
    /// 
    /// </author>
    public class LinkedActivationList : AbstractActivationList
    {
        private int count = 0;

        private LinkedActivation first = null;

        private LinkedActivation last = null;

        public LinkedActivationList()
        {
            stratey = new DepthStrategy(); 
        }

        public override bool AscendingOrder
        {
            get { return true; }
        }

        public virtual bool Empty
        {
            get { return count == 0; }
        }

        public override IActivation nextActivation()
        {
            if (lazy)
            {
                if (count == 0)
                {
                    return null;
                }
                else
                {
                    LinkedActivation left = last;
                    LinkedActivation right = last.Previous;
                    while (right != null)
                    {
                        if (stratey.compare(left, right) < 1)
                        {
                            left = right;
                        }
                        right = right.Previous;
                    }
                    if (left == first)
                    {
                        first = left.Next;
                    }
                    else if (left == last)
                    {
                        last = left.Previous;
                    }
                    left.remove();
                    count--;
                    return left;
                }
            }
            else
            {
                if (count > 1)
                {
                    LinkedActivation r = last;
                    last = r.Previous;
                    count--;
                    r.remove();
                    return r;
                }
                else if (count == 1)
                {
                    LinkedActivation r = last;
                    last = null;
                    first = null;
                    count--;
                    return r;
                }
                else
                {
                    return null;
                }
            }
        }

        public override void addActivation(IActivation act)
        {
            if (act is LinkedActivation)
            {
                LinkedActivation newact = (LinkedActivation) act;
                if (lazy)
                {
                    if (count == 0)
                    {
                        first = newact;
                        last = newact;
                    }
                    else
                    {
                        last.Next = newact;
                        last = newact;
                    }
                    count++;
                }
                else
                {
                    if (count > 0)
                    {
                        quickSort(newact);
                    }
                    else if (count == 0)
                    {
                        first = newact;
                        last = newact;
                    }
                    count++;
                }
            }
        }

        /// <summary>
        /// the sort method uses binary search to find the correct insertion
        /// point for the new activation. It's much faster than the brute
        /// force method.
        /// </summary>
        /// <param name="newact">The newact.</param>
        public virtual void quickSort(LinkedActivation newact)
        {
            if (stratey.compare(newact, last) >= 0)
            {
                // the new activation has a higher salience than the last, which means
                // it should become the bottom activation
                last.Next = newact;
                last = newact;
            }
            else if (stratey.compare(newact, first) < 0)
            {
                // the new activation has a salience lower than the first, which means
                // it should become the top activation
                newact.Next = first;
                first = newact;
            }
            else
            {
                // this means the new activation goes in the middle some where
                int counter = count/2;
                LinkedActivation cur = goUp(counter, last);
                bool added = false;
                while (!added)
                {
                    if (counter <= 1)
                    {
                        // Add the activation
                        if (stratey.compare(newact, cur) < 0)
                        {
                            // if the new activation is lower sailence than the current,
                            // we Add it before the current (aka above)
                            newact.Previous = cur.Previous;
                            newact.Next = cur;
                        }
                        else
                        {
                            // the new activation is higher salience than the current
                            // therefore we Add it after (aka below)
                            newact.Next = cur.Next;
                            newact.Previous = cur;
                        }
                        added = true;
                    }
                    else if (stratey.compare(newact, cur) >= 0)
                    {
                        // the new activation is of greater salience down half again
                        counter = counter/2;
                        cur = goDown(counter, cur);
                    }
                    else
                    {
                        // the new activation is of lower salience, up half again
                        counter = counter/2;
                        cur = goUp(counter, cur);
                    }
                }
            }
        }

        /// <summary>
        /// method will loop for the given count and return the item before it.
        /// for example:
        /// 1
        /// 2
        /// 3
        /// 4
        /// 5
        /// 6
        /// If I pass a count of 2 and item #6. it will return #4.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        protected internal virtual LinkedActivation goUp(int count, LinkedActivation start)
        {
            LinkedActivation rt = start;
            for (int idx = 0; idx < count; idx++)
            {
                rt = rt.Previous;
            }
            return rt;
        }

        /// <summary>
        /// method will loop for the given count and return the item after it.
        /// for example:
        /// 1
        /// 2
        /// 3
        /// 4
        /// 5
        /// 6
        /// If I pass a count of 2 and item #1. it will return #3.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        protected internal virtual LinkedActivation goDown(int count, LinkedActivation start)
        {
            LinkedActivation rt = start;
            for (int idx = 0; idx < count; idx++)
            {
                rt = rt.Next;
            }
            return rt;
        }

        /// <summary> removeActivation will check to see if the activation is
        /// the first or last before removing it.
        /// </summary>
        public override IActivation removeActivation(IActivation act)
        {
            if (act is LinkedActivation)
            {
                LinkedActivation lact = (LinkedActivation) act;
                if (first == lact)
                {
                    first = lact.Next;
                }
                if (last == lact)
                {
                    last = lact.Previous;
                }
                count--;
                lact.remove();
            }
            return act;
        }


        public override int size()
        {
            return count;
        }


        /// <summary> the current implementation iterates over the LinkedActivations
        /// from the start until it finds a match. If it doesn't find a
        /// match, the method returns false.
        /// </summary>
        public virtual bool contains(Object o)
        {
            bool contain = false;
            LinkedActivation act = first;
            while (act != null)
            {
                if (o == act)
                {
                    contain = true;
                    break;
                }
                else
                {
                    act = act.Next;
                }
            }
            return contain;
        }

        /// <summary> Iterate over the Creshendo.rete.util.LinkedList and null the references to previous
        /// and Current in the LinkedActivation
        /// </summary>
        public override void clear()
        {
            while (first != null)
            {
                LinkedActivation la = first;
                first = la.Next;
                la.remove();
            }
            last = null;
            count = 0;
        }

        public virtual Object set(int index, Object activation)
        {
            if (index < count && activation != null)
            {
                LinkedActivation act = first;
                for (int idx = 0; idx <= count; idx++)
                {
                    act = act.Next;
                }
                // now we are at the index point
                LinkedActivation pre = act.Previous;
                LinkedActivation nxt = act.Next;
                pre.Next = (LinkedActivation) activation;
                nxt.Previous = (LinkedActivation) activation;
                act.remove();
            }
            return activation;
        }

        /// <summary> Current implemenation will return the index of the activation, if
        /// it is in the Creshendo.rete.util.LinkedList. If activation isn't in the list, the method
        /// returns -1.
        /// </summary>
        public virtual int indexOf(Object activation)
        {
            int index = - 1;
            LinkedActivation la = first;
            LinkedActivation match = null;
            while (la != null)
            {
                index++;
                if (la == activation)
                {
                    match = la;
                    break;
                }
                else
                {
                    la = la.Next;
                }
            }
            if (match != null)
            {
                return index;
            }
            else
            {
                return - 1;
            }
        }

        /// <summary> method will clone the list and make a copy of the activations
        /// </summary>
        public override IActivationList cloneActivationList()
        {
            LinkedActivationList la = new LinkedActivationList();
            la.count = count;
            la.first = first.cloneActivation();
            la.lazy = lazy;
            la.stratey = stratey;
            LinkedActivation current = first;
            LinkedActivation newcurr = la.first;
            while (current != null)
            {
                newcurr.Next = current.Next.cloneActivation();
                current = current.Next;
                newcurr = newcurr.Next;
            }
            return la;
        }
    }
}