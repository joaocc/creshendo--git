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
namespace org.jamocha.rete.functions.math
{
	using System;
	using BoundParam = org.jamocha.rete.BoundParam;
	using Constants = org.jamocha.rete.Constants;
	using DefaultReturnValue = org.jamocha.rete.DefaultReturnValue;
	using DefaultReturnVector = org.jamocha.rete.DefaultReturnVector;
	using Function = org.jamocha.rete.Function;
	using FunctionParam2 = org.jamocha.rete.FunctionParam2;
	using Parameter = org.jamocha.rete.Parameter;
	using Rete = org.jamocha.rete.Rete;
	using ReturnVector = org.jamocha.rete.ReturnVector;
	using ValueParam = org.jamocha.rete.ValueParam;
	/// <author>  Peter Lin
	/// *
	/// TODO To change the template for this generated type comment go to
	/// Window - Preferences - Java - Code Style - Code Templates
	/// 
	/// </author>
	[Serializable]
	public class Multiply : Function, 
	{
		virtual public int ReturnType
		{
			get
			{
				return Constants.BIG_DECIMAL;
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return MULTIPLY;
			}
			
		}
		virtual public System.Type[] Parameter
		{
			get
			{
				return new System.Type[]{typeof(ValueParam[])};
			}
			
		}
		
		public const System.String MULTIPLY = "multiply";
		
		/// <summary> 
		/// </summary>
		public Multiply():base()
		{
		}
		
		
		public virtual ReturnVector executeFunction(Rete engine, Parameter[] params_Renamed)
		{
			System.Decimal bdval = new System.Decimal(0);
			if (params_Renamed != null)
			{
				if (params_Renamed[0] is ValueParam)
				{
					bdval = params_Renamed[0].BigDecimalValue;
				}
				else if (params_Renamed[0] is BoundParam)
				{
					BoundParam bp = (BoundParam) params_Renamed[0];
					bdval = (System.Decimal) engine.getBinding(bp.VariableName);
				}
				else if (params_Renamed[0] is FunctionParam2)
				{
					FunctionParam2 n = (FunctionParam2) params_Renamed[0];
					n.Engine = engine;
					n.lookUpFunction();
					ReturnVector rval = (ReturnVector) n.Value;
					bdval = rval.firstReturnValue().BigDecimalValue;
				}
				for (int idx = 1; idx < params_Renamed.Length; idx++)
				{
					if (params_Renamed[idx] is ValueParam)
					{
						ValueParam n = (ValueParam) params_Renamed[idx];
						System.Decimal bd = n.BigDecimalValue;
						bdval = System.Decimal.Multiply(bdval, bd);
					}
					else if (params_Renamed[idx] is FunctionParam2)
					{
						FunctionParam2 n = (FunctionParam2) params_Renamed[idx];
						n.Engine = engine;
						n.lookUpFunction();
						ReturnVector rval = (ReturnVector) n.Value;
						System.Decimal bd = rval.firstReturnValue().BigDecimalValue;
						if (idx == 0)
						{
							bdval = bd;
						}
						else
						{
							bdval = System.Decimal.Multiply(bdval, bd);
						}
					}
				}
			}
			DefaultReturnVector ret = new DefaultReturnVector();
			DefaultReturnValue rv = new DefaultReturnValue(Constants.BIG_DECIMAL, bdval);
			ret.addReturnValue(rv);
			return ret;
		}
		
		
		
		public virtual System.String toPPString(Parameter[] params_Renamed, int indents)
		{
			if (params_Renamed != null && params_Renamed.Length > 0)
			{
				System.Text.StringBuilder buf = new System.Text.StringBuilder();
				buf.Append("(*");
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
				return "(* (<literal> | <binding>)+)\n" + "Function description:\n" + "\tCalculates the product of its arguments.";
			}
		}
	}
}