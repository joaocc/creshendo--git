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
using System.Collections.Generic;
using System.Reflection;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule.Util
{
    /// <author>  Peter Lin
    /// 
    /// The class will generate the trigger facts for a single rule. The purpose of this is to make
    /// it easier to test a rule. Since a rule knows what conditions it needs, it makes sense to
    /// generate the trigger facts instead of doing it manually.
    /// 
    /// </author>
    public class GenerateFacts
    {
        public GenerateFacts() 
        {
        }

        public static IList<Object> generateFacts(IRule rule, Rete.Rete engine)
        {
            List<Object> facts = new List<Object>();
            if (rule != null)
            {
                ICondition[] conditions = rule.Conditions;
                for (int idx = 0; idx < conditions.Length; idx++)
                {
                    ICondition c = conditions[idx];
                    if (c is ObjectCondition)
                    {
                        ObjectCondition oc = (ObjectCondition) c;
                        Deftemplate tpl = (Deftemplate) engine.findTemplate(oc.TemplateName);
                        if (tpl.ClassName != null)
                        {
                            Object data = generateJavaFacts(oc, tpl, engine);
                            facts.Add(data);
                        }
                        else
                        {
                            IFact data = generateDeffact(oc, tpl, engine);
                            facts.Add(data);
                        }
                    }
                    else if (c is TestCondition)
                    {
                    }
                }
            }
            return facts;
        }

        /// <summary> The method uses Defclass, Class, Deftemplate and Rete to create a new
        /// instance of the java object. Once the instance is created, the method
        /// uses Defclass to look up the write method and calls it with the
        /// appropriate value.
        /// </summary>
        /// <param name="">cond
        /// </param>
        /// <param name="">templ
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object generateJavaFacts(ObjectCondition cond, Deftemplate templ, Rete.Rete engine)
        {
            //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            try
            {
                //UPGRADE_TODO: Format of parameters of method 'java.lang.Class.forName' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
                Type theclz = Type.GetType(templ.ClassName);
                Defclass dfc = engine.findDefclass(theclz);
                Object data = CreateNewInstance(theclz);
                IConstraint[] cnstr = cond.Constraints;
                for (int idx = 0; idx < cnstr.Length; idx++)
                {
                    IConstraint cn = cnstr[idx];
                    if (cn is LiteralConstraint)
                    {
                        MethodInfo meth = dfc.getWriteMethod(cn.Name);
                        meth.Invoke(data, (Object[]) new Object[] {cn.Value});
                    }
                }
                // for now the method doesn't inspect the bindings
                // later on it needs to be added

                return data;
            }

            catch (UnauthorizedAccessException e)
            {
                return null;
            }
            catch (ArgumentException e)
            {
                return null;
            }
            catch (TargetInvocationException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Object CreateNewInstance(Type classType)
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

        /// <summary> 
        /// </summary>
        /// <param name="">cond
        /// </param>
        /// <param name="">templ
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IFact generateDeffact(ObjectCondition cond, Deftemplate templ, Rete.Rete engine)
        {
            List<object> list = new List<Object>();
            IConstraint[] cnstr = cond.Constraints;
            for (int idx = 0; idx < cnstr.Length; idx++)
            {
                IConstraint cn = cnstr[idx];
                if (cn is LiteralConstraint)
                {
                    Slot s = new Slot(cn.Name, cn.Value);
                    list.Add(s);
                }
                else if (cn is PredicateConstraint)
                {
                    PredicateConstraint pc = (PredicateConstraint) cn;
                    Object val = generatePredicateValue(pc);
                    Slot s = new Slot(cn.Name, val);
                    list.Add(s);
                }
                else if (cn is BoundConstraint)
                {
                    // for now we do the simple thing and just set
                    // any bound slots to 1
                    Slot s = new Slot(cn.Name, 1);
                    list.Add(s);
                }
            }
            IFact f = templ.createFact(list, engine.nextFactId());
            return f;
        }

        public static String parseModuleName(IRule rule, Rete.Rete engine)
        {
            if (rule.Name.IndexOf("::") > 0)
            {
                String text = rule.Name;
                String[] sp = text.Split("::".ToCharArray());
                return sp[0].ToUpper();
            }
            return null;
        }

        public static Object generatePredicateValue(PredicateConstraint pc)
        {
            String fname = pc.FunctionName;
            Object value_Renamed = null;
            IParameter p = null;
            // first find the literal value
            IList prms = pc.Parameters;
            for (int idx = 0; idx < prms.Count; idx++)
            {
                if (prms[idx] is ValueParam)
                {
                    p = (IParameter) pc.Parameters[1];
                }
            }
            if (fname.Equals(">") || fname.Equals(">="))
            {
                value_Renamed = Decimal.Add(p.BigDecimalValue, new Decimal(1));
            }
            else if (fname.Equals("<") || fname.Equals("<="))
            {
                value_Renamed = Decimal.Subtract(p.BigDecimalValue, new Decimal(1));
            }
            return value_Renamed;
        }
    }
}