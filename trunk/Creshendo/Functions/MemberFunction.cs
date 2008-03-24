using System;
using System.Reflection;
using System.Security;
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Christian Ebert
    /// 
    /// Calls a method of a specified object.
    /// 
    /// </author>
    [Serializable]
    public class MemberFunction : IFunction
    {
        public const String MEMBER = "member";

        public MemberFunction(ClassnameResolver classnameResolver) 
        {
        }

        #region Function Members

        public virtual String Name
        {
            get { return MEMBER; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }

        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Object o = null;
            Object ro = null;
            String methodname = null;
            Type[] argsclass = null;
            Object[] args = null;
            if (params_Renamed != null)
            {
                if (params_Renamed[0] is ValueParam)
                {
                    ValueParam n = (ValueParam) params_Renamed[0];
                    o = n.Value;
                }
                else if (params_Renamed[0] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[0];
                    o = engine.getBinding(bp.VariableName);
                }
                else if (params_Renamed[0] is FunctionParam2)
                {
                    FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                    n.Engine = engine;
                    n.lookUpFunction();
                    IReturnVector rval = (IReturnVector) n.Value;
                    o = rval.firstReturnValue().Value;
                }
                if (params_Renamed[1] is ValueParam)
                {
                    ValueParam n = (ValueParam) params_Renamed[1];
                    methodname = n.StringValue;
                }
                else if (params_Renamed[1] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[1];
                    methodname = (String) engine.getBinding(bp.VariableName);
                }
                else if (params_Renamed[1] is FunctionParam2)
                {
                    FunctionParam2 n = (FunctionParam2) params_Renamed[1];
                    n.Engine = engine;
                    n.lookUpFunction();
                    IReturnVector rval = (IReturnVector) n.Value;
                    methodname = rval.firstReturnValue().StringValue;
                }
                if (params_Renamed.Length > 2)
                {
                    argsclass = new Type[params_Renamed.Length - 1];
                    args = new Object[params_Renamed.Length - 1];
                }
                for (int idx = 2; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[idx];
                        argsclass[idx - 1] = n.Value.GetType();
                        args[idx - 1] = n.Value;
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        argsclass[idx - 1] = engine.getBinding(bp.VariableName).GetType();
                        args[idx - 1] = engine.getBinding(bp.VariableName);
                    }
                    else if (params_Renamed[idx] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[idx];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        argsclass[idx - 1] = rval.firstReturnValue().Value.GetType();
                        args[idx - 1] = rval.firstReturnValue().Value;
                    }
                }
                try
                {
                    Type classDefinition = o.GetType();
                    MethodInfo method = classDefinition.GetMethod(methodname, (Type[]) argsclass);
                    ro = method.Invoke(o, (Object[]) args);
                }
                catch (UnauthorizedAccessException e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
                catch (SecurityException e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
                catch (MethodAccessException e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
                catch (ArgumentException e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
                catch (TargetInvocationException e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.OBJECT_TYPE, ro);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(member");
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
                return "(member (<literal> | <binding> | <value>)+)\n";
            }
        }

        #endregion
    }
}