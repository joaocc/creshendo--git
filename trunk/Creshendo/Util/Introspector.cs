using System;

namespace Creshendo.Util
{
    public class Introspector
    {
        public static BeanInfo getBeanInfo(Type type)
        {
            return new BeanInfo(type);
        }
    }
}