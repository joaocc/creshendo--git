/*
* Copyright 2006 Josef Hahn
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
namespace org.jamocha.rete.functions
{
	using System;
	
	
	using BaseNode = org.jamocha.rete.BaseNode;
	using Constants = org.jamocha.rete.Constants;
	using DefaultReturnVector = org.jamocha.rete.DefaultReturnVector;
	using Function = org.jamocha.rete.Function;
	using Parameter = org.jamocha.rete.Parameter;
	using Rete = org.jamocha.rete.Rete;
	using ReturnVector = org.jamocha.rete.ReturnVector;
	using RootNode = org.jamocha.rete.RootNode;
	using ViewGraphNode = org.jamocha.rete.visualisation.ViewGraphNode;
	using Visualiser = org.jamocha.rete.visualisation.Visualiser;
	/// <author>  Josef Alexander Hahn
	/// 
	/// Opens a visualisation window for the rete net
	/// 
	/// </author>
	[Serializable]
	public class ViewFunction : Function
	{
		virtual public int ReturnType
		{
			get
			{
				return Constants.RETURN_VOID_TYPE;
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return VIEW;
			}
			
		}
		virtual public System.Type[] Parameter
		{
			get
			{
				return new System.Type[0];
			}
			
		}
		
		public const System.String VIEW = "view";
		
		/// <summary> 
		/// </summary>
		public ViewFunction():base()
		{
		}
		
		
		internal virtual void  traverse(int indent, BaseNode b)
		{
			for (int i = 0; i < indent; i++)
				System.Console.Out.Write(" ");
			//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			System.Console.Out.WriteLine("+" + b.ToString() + " id=" + b.NodeId);
			for (int i = 0; i < b.SuccessorNodes.Length; i++)
				traverse(indent + 2, (BaseNode) b.SuccessorNodes[i]);
		}
		
		
		public virtual ReturnVector executeFunction(Rete engine, Parameter[] params_Renamed)
		{
			RootNode root = engine.RootNode;
			
			/*Collection firstLevel=root.getObjectTypeNodes().values();
			for (Iterator iter = firstLevel.iterator(); iter.hasNext();) {
			BaseNode b=(BaseNode)iter.next();
			traverse(0,b);
			}*/
			
			
			ViewGraphNode t = ViewGraphNode.buildFromRete(root);
			Visualiser visualiser = new Visualiser(engine);
			visualiser.show();
			return new DefaultReturnVector();
		}
		
		
		
		public virtual System.String toPPString(Parameter[] params_Renamed, int indents)
		{
			return "(view)";
		}
	}
}