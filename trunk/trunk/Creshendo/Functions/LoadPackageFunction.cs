using System;
using System.Diagnostics;
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
    public class LoadPackageFunction : IFunction
    {
        public const String FUNCTION_NAME = "load-package";

        private ClassnameResolver classnameResolver;

        public LoadPackageFunction(ClassnameResolver classnameResolver) 
        {
            this.classnameResolver = classnameResolver;
        }

        #region Function Members

        public virtual String Name
        {
            get { return FUNCTION_NAME; }
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
            if (params_Renamed != null && params_Renamed.Length == 1)
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
                try
                {
                    Type classDefinition = classnameResolver.resolveClass(classname);
                    o = CreateNewInstance(classDefinition);
                    if (o is IFunctionGroup)
                    {
                        engine.declareFunctionGroup((IFunctionGroup) o);
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
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Trace.WriteLine(e.Message);
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.OBJECT_TYPE, o);
            ret.addReturnValue(rv);
            return ret;
        }

        public Object CreateNewInstance(Type classType)
        {
            ConstructorInfo[] constructors = classType.GetConstructors();

            if (constructors.Length == 0)
                return null;

            ParameterInfo[] firstConstructor = constructors[0].GetParameters();
            int countParams = firstConstructor.Length;

            Type[] constructor = new Type[countParams];
            for (int i = 0; i < countParams; i++)
                constructor[i] = firstConstructor[i].ParameterType;

            return classType.GetConstructor(constructor).Invoke(new Object[] { });
        }

        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(load-package");
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
                return "(load-package <classname>)\n";
            }
        }

        #endregion
    }
}