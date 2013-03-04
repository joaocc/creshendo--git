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
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    public class DeffactSort : IComparer
    {
        public static DeffactSort Comparator;

        static DeffactSort()
        {
            Comparator = new DeffactSort();
        }

        public DeffactSort() 
        {
        }

        #region IComparer Members

        public virtual int Compare(Object left, Object right)
        {
            if (((Deffact) left).FactId > ((Deffact) right).FactId)
            {
                return 1;
            }
            else if (((Deffact) left).FactId == ((Deffact) right).FactId)
            {
                return 0;
            }
            else
            {
                return - 1;
            }
        }

        #endregion
    }
}