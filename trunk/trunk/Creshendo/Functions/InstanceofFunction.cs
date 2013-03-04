using System;
using System.Text;
using Creshendo.Util;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    [Serializable]
    public class InstanceofFunction : IFunction
    {
        public const String INSTANCEOF = "instanceof";

        private ClassnameResolver classnameResolver;

        public InstanceofFunction(ClassnameResolver classnameResolver) 
        {
            this.classnameResolver = classnameResolver;
        }

        #region Function Members

        public virtual String Name
        {
            get { return INSTANCEOF; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam), typeof (BoundParam)}; }
        }

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool eval = false;
            if (params_Renamed.Length == 2)
            {
                Object param1 = null;
                if (params_Renamed[0] is BoundParam && params_Renamed[1] is BoundParam)
                {
                    param1 = ((BoundParam) params_Renamed[0]).ObjectRef;
                    try
                    {
                        Type clazz = classnameResolver.resolveClass(((BoundParam) params_Renamed[1]).StringValue);
                        eval = clazz.IsInstanceOfType(param1);
                    }
                    catch (Exception e)
                    {
                        throw new RuntimeException(e);
                    }
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, eval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(instanceof");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else if (params_Renamed[idx] is ValueParam)
                    {
                        buf.Append(" " + params_Renamed[idx].StringValue);
                    }
                    else
                    {
                        buf.Append(" " + params_Renamed[idx].StringValue);
                    }
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(instanceof <Java-object> <class-name>)\n";
            }
        }

        #endregion
    }
}