using System;
using System.Reflection;

namespace Creshendo.Util
{
    public class BeanInfo
    {
        private Type _type;

        public BeanInfo(Type type)
        {
            _type = type;
        }

        public PropertyInfo[] getPropertyDescriptors()
        {
            return _type.GetProperties();
        }

        public MethodInfo[] getMethodDescriptors()
        {
            return _type.GetMethods();
        }
    }
}