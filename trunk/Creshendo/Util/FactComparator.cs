using System;
using System.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    public class FactComparator : IComparer
    {
        public FactComparator() 
        {
        }

        #region IComparer Members

        public virtual int Compare(Object left, Object right)
        {
            IFact lf = (IFact) left;
            IFact rf = (IFact) right;
            if (lf.FactId > rf.FactId)
            {
                return 1;
            }
            else if (lf.FactId == rf.FactId)
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