/*
* Copyright 2006 Nikolaus Koemm
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
	/// <author>  Nikolaus Koemm
	/// 
	/// If its ony argument is odd, Oddp returns true.
	/// 
	/// </author>
	[Serializable]
	public class Oddp : Function, 
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
				return ODDP;
			}
			
		}
		virtual public System.Type[] Parameter
		{
			get
			{
				return new System.Type[]{typeof(ValueParam[])};
			}
			
		}
		
		public const System.String ODDP = "oddp";
		
		/// <summary> 
		/// </summary>
		public Oddp():base()
		{
		}
		
		
		public virtual ReturnVector executeFunction(Rete engine, Parameter[] params_Renamed)
		{
			System.Decimal bdval = new System.Decimal(0);
			bool eval = false;
			if (params_Renamed.Length == 1)
			{
				bdval = (System.Decimal) params_Renamed[0].getValue(engine, Constants.BIG_DECIMAL);
				double bdh = System.Decimal.ToDouble(bdval);
				if (bdh % 2 == 1)
				{
					eval = true;
				}
			}
			DefaultReturnVector ret = new DefaultReturnVector();
			DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, eval);
			ret.addReturnValue(rv);
			return ret;
		}
		
		
		
		public virtual System.String toPPString(Parameter[] params_Renamed, int indents)
		{
			if (params_Renamed != null && params_Renamed.Length >= 0)
			{
				System.Text.StringBuilder buf = new System.Text.StringBuilder();
				buf.Append("(oddp");
				int idx = 0;
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
				buf.Append(")");
				return buf.ToString();
			}
			else
			{
				return "(oddp <expression>)\n" + "Function description:\n" + "\tReturns true, if its only argument is odd.";
			}
		}
	}
}