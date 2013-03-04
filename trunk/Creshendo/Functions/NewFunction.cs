using System;
using System.Reflection;
using System.Security;
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Christian Ebert
    /// 
    /// Creates a Java Object and returns it.
    /// 
    /// </author>
    [Serializable]
    public class NewFunction : IFunction
    {
        public const String NEW = "new";

        private ClassnameResolver classnameResolver;

        public NewFunction(ClassnameResolver classnameResolver) 
        {
            this.classnameResolver = classnameResolver;
        }

        #region Function Members

        public virtual String Name
        {
            get { return NEW; }
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
            String classname = null;
            Type[] argsclass = null;
            Object[] args = null;
            if (params_Renamed != null)
            {
                if (params_Renamed[0] is ValueParam)
                {
                    ValueParam n = (ValueParam) params_Renamed[0];
                    classname = n.StringValue;
                }
                else if (params_Renamed[0] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[0];
                    classname = (String) engine.getBinding(bp.VariableName);
                }
                else if (params_Renamed[0] is FunctionParam2)
                {
                    FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                    n.Engine = engine;
                    n.lookUpFunction();
                    IReturnVector rval = (IReturnVector) n.Value;
                    classname = rval.firstReturnValue().StringValue;
                }
                args = new Object[params_Renamed.Length - 1];
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[idx];
                        args[idx - 1] = n.Value;
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        args[idx - 1] = engine.getBinding(bp.VariableName);
                    }
                    else if (params_Renamed[idx] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[idx];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        args[idx - 1] = rval.firstReturnValue().Value;
                    }
                }
                try
                {
                    Type classDefinition = classnameResolver.resolveClass(classname);
                    ConstructorInfo foundConstructor = null;
                    for (int idx = 0; idx < classDefinition.GetConstructors().Length; idx++)
                    {
                        ConstructorInfo constructor = classDefinition.GetConstructors()[idx];
                        ParameterInfo[] parameterClasses = constructor.GetParameters();
                        if (parameterClasses.Length == args.Length)
                        {
                            bool match = true;
                            for (int i = 0; i < parameterClasses.Length; ++i)
                            {
                                match &= (parameterClasses[i].GetType().IsInstanceOfType(args[i]) || args[i] == null);
                            }
                            if (match)
                            {
                                foundConstructor = constructor;
                                break;
                            }
                        }
                    }
                    if (foundConstructor != null)
                    {
                        o = foundConstructor.Invoke(args);
                    }
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
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.OBJECT_TYPE, o);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(new");
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
                return "(new (<literal> | <binding> | <value>)+)\n";
            }
        }

        #endregion
    }
}