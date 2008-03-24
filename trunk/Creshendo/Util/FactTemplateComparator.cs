using System;
using System.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    public class FactTemplateComparator : IComparer
    {
        public FactTemplateComparator() 
        {
        }

        #region IComparer Members

        public virtual int Compare(Object left, Object right)
        {
            IFact lf = (IFact) left;
            IFact rf = (IFact) right;
            return lf.Deftemplate.Name.CompareTo(rf.Deftemplate.Name);
        }

        #endregion
    }
}