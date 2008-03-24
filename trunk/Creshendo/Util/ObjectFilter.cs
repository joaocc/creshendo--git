using System;

namespace Creshendo.Util
{
    public class ObjectFilter
    {
        public static BeanFilter lookupFilter(Type type)
        {
            return new BeanFilter();
        }
    }
}