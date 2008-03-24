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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    /// Peter Lin
    /// *
    /// Slot2 is used for conditions that evaluate against multiple values.
    /// For example: (attr2 "me" | "you" | ~"her" | ~"she")
    /// 
    /// Rather than evaluate a long sequence of equal/not equal sequentially,
    /// we use two lists: equal and notequal. Slot2 is used exclusive for
    /// a sequence of "or" value comparisons.
    /// 
    /// 
    public class Slot2 : Slot
    {
        //UPGRADE_NOTE: The initialization of  'equalsList' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IList equalsList;
        //UPGRADE_NOTE: The initialization of  'notEqualList' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IList notEqualList;

        public Slot2()
        {
            InitBlock();
        }

        public Slot2(String name)
        {
            InitBlock();
            Name = name;
        }

        /// <summary> the method doesn't apply to slot2
        /// </summary>
        /// <summary> method will check to see if the object is a collection. if it is,
        /// it will iterate over the collection and Add each one to the right
        /// list.
        /// </summary>
        /// <param name="">val
        /// 
        /// </param>
        public override Object Value
        {
            get { return null; }

            set
            {
                if (value is IList)
                {
                    IEnumerator itr = ((IList)value).GetEnumerator();
                    while (itr.MoveNext())
                    {
                        MultiValue mv = (MultiValue) itr.Current;
                        if (mv.Negated)
                        {
                            notEqualList.Add(mv.Value);
                        }
                        else
                        {
                            equalsList.Add(mv.Value);
                        }
                    }
                }
            }
        }

        /// <summary> Get the list of values the slot should equal to
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the values the slot should equal to 
        /// </summary>
        /// <param name="">val
        /// 
        /// </param>
        public virtual IList EqualList
        {
            get { return equalsList; }

            set { equalsList = value; }
        }

        /// <summary> Get the list of values the slot should not equal to
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the list of values the slot should not equal to
        /// </summary>
        /// <param name="">val
        /// 
        /// </param>
        public virtual IList NotEqualList
        {
            get { return notEqualList; }

            set { notEqualList = value; }
        }

        private void InitBlock()
        {
            equalsList = new List<Object>();
            notEqualList = new List<Object>();
        }


        public virtual String toString(String andOr)
        {
            StringBuilder buf = new StringBuilder();
            if (equalsList.Count > 0)
            {
                IEnumerator itr = equalsList.GetEnumerator();
                buf.Append(itr.Current.ToString());
                while (itr.MoveNext())
                {
                    buf.Append(andOr + itr.Current.ToString());
                }
            }
            if (notEqualList.Count > 0)
            {
                IEnumerator itr = notEqualList.GetEnumerator();
                buf.Append(itr.Current.ToString());
                while (itr.MoveNext())
                {
                    buf.Append(andOr + itr.Current.ToString());
                }
            }
            return buf.ToString();
        }

        //UPGRADE_TODO: The equivalent of method 'java.lang.Object.clone' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
        /// <summary> A convienance method to clone slots
        /// </summary>
        public Object Clone()
        {
            Slot2 newslot = new Slot2();
            newslot.Id = Id;
            newslot.Name = Name;
            newslot.EqualList = (EqualList);
            newslot.NotEqualList = (NotEqualList);
            newslot.ValueType = ValueType;
            return newslot;
        }
    }
}