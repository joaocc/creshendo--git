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
namespace org.jamocha.rete.visualisation
{
	using System;
	
	
	
	using BaseNode = org.jamocha.rete.BaseNode;
	using RootNode = org.jamocha.rete.RootNode;
	/// <author>  Josef Alexander Hahn
	/// ViewGraphNode represents a node in the visualisation graph
	/// 
	/// </author>
	public class ViewGraphNode
	{
		/// <returns> the corresponding rete node
		/// 
		/// </returns>
		/// <summary> sets the corresponding rete node
		/// </summary>
		/// <param name="n">rete node
		/// 
		/// </param>
		virtual public BaseNode ReteNode
		{
			get
			{
				return reteNode;
			}
			
			set
			{
				reteNode = value;
			}
			
		}
		/// <returns> x-position
		/// 
		/// </returns>
		virtual public int X
		{
			get
			{
				checkForValidAlignment();
				return x;
			}
			
		}
		/// <returns> y-position
		/// 
		/// </returns>
		virtual public int Y
		{
			get
			{
				checkForValidAlignment();
				return y;
			}
			
		}
		/// <returns> height of the tree with this node as root in logical units
		/// 
		/// </returns>
		virtual public int Height
		{
			get
			{
				int h = 0;
				for (Iterator it = childs.iterator(); it.hasNext(); )
				{
					h = System.Math.Max(h, ((ViewGraphNode) it.next()).Height);
				}
				return h + 1;
			}
			
		}
		/// <returns> width of its subtree in logical units
		/// 
		/// </returns>
		virtual public int Width
		{
			get
			{
				return SubtreeWidth;
			}
			
		}
		/// <returns> list of successors
		/// 
		/// </returns>
		virtual public org.jamocha.rete.util.Arraylist Successors
		{
			get
			{
				return childs;
			}
			
		}
		/// <returns> list of parents
		/// 
		/// </returns>
		virtual public org.jamocha.rete.util.Arraylist Parents
		{
			get
			{
				return parents;
			}
			
		}
		/// <summary> Calculates the width of the subtree in a logical unit.
		/// (one node has width=2, two nodes have width=4 and so on;
		/// this is because we need "half node widths" for centering
		/// one node related to an even numbers of other nodes)
		/// </summary>
		virtual protected internal int SubtreeWidth
		{
			get
			{
				int r = 0;
				for (Iterator it = childs.iterator(); it.hasNext(); )
				{
					ViewGraphNode nxt = (ViewGraphNode) it.next();
					int myWidth = nxt.getMyWidth(this);
					int subtreeWidth = nxt.SubtreeWidth;
					if (myWidth > 0)
						r += System.Math.Max(myWidth, subtreeWidth);
				}
				return r;
			}
			
		}
		/// <returns> the corresponding shape
		/// 
		/// </returns>
		/// <summary> sets the corresponding shape
		/// </summary>
		/// <param name="shape">the shape
		/// 
		/// </param>
		virtual public Shape Shape
		{
			get
			{
				return shape;
			}
			
			set
			{
				this.shape = value;
			}
			
		}
		/// <summary> gets the parentsChecked-Flag used
		/// in the visualiser
		/// </summary>
		/// <returns> parentsChecked-flag
		/// 
		/// </returns>
		/// <summary> sets the parentsChecked-Flag used
		/// in the visualiser
		/// </summary>
		/// <param name="parentsChecked">parentsChecked-flag
		/// 
		/// </param>
		virtual public bool ParentsChecked
		{
			get
			{
				return parentsChecked;
			}
			
			set
			{
				this.parentsChecked = value;
			}
			
		}
		
		protected internal int subtreewidth;
		protected internal BaseNode reteNode;
		protected internal bool parentsChecked;
		protected internal Shape shape;
		protected internal org.jamocha.rete.util.Arraylist childs;
		protected internal org.jamocha.rete.util.Arraylist parents;
		protected internal int x;
		protected internal int y;
		
		
		
		protected internal virtual void  checkForValidAlignment()
		{
			if (x == - 1)
			{
				ViewGraphNode r = this;
				while (!r.parents.isEmpty())
					r = (ViewGraphNode) parents.get(0);
				r.calculateAlignment(0, 0);
			}
		}
		
		
		
		
		
		
		
		
		protected internal virtual void  calculateAlignment(int offsetX, int offsetY)
		{
			x = SubtreeWidth / 2 - 1 + offsetX;
			if (x < offsetX)
				x = offsetX;
			y = offsetY;
			offsetY++;
			for (Iterator it = childs.iterator(); it.hasNext(); )
			{
				ViewGraphNode sub = (ViewGraphNode) it.next();
				sub.calculateAlignment(offsetX, offsetY);
				offsetX += sub.SubtreeWidth;
				if (sub.SubtreeWidth == 0)
					offsetX += 2;
			}
		}
		
		protected internal virtual void  invalidateSubtreeWidth()
		{
			subtreewidth = - 1;
			x = - 1;
			y = - 1;
			for (Iterator it = parents.iterator(); it.hasNext(); )
			{
				((ViewGraphNode) it.next()).invalidateSubtreeWidth();
			}
		}
		
		/// <summary> Yet another constructor ;)
		/// </summary>
		public ViewGraphNode(BaseNode n):this()
		{
			ReteNode = n;
		}
		
		/// <summary> Builds a complete Graph by traversing root
		/// </summary>
		/// <param name="">root
		/// 
		/// </param>
		public static ViewGraphNode buildFromRete(RootNode root)
		{
			Collection firstLevel = root.ObjectTypeNodes.values();
			ViewGraphNode res = new ViewGraphNode();
			System.Collections.Hashtable ht = new System.Collections.Hashtable();
			for (Iterator iter = firstLevel.iterator(); iter.hasNext(); )
			{
				BaseNode b = (BaseNode) iter.next();
				res.addToChilds(buildFromRete(b, ht));
			}
			return res;
		}
		
		protected internal static ViewGraphNode buildFromRete(BaseNode root, System.Collections.Hashtable ht)
		{
			System.Object[] succ = root.SuccessorNodes;
			ViewGraphNode foo = (ViewGraphNode) ht[root];
			ViewGraphNode res = null;
			if (foo == null)
			{
				res = new ViewGraphNode(root);
				for (int i = 0; i < succ.Length; i++)
				{
					res.addToChilds(buildFromRete((BaseNode) succ[i], ht));
				}
				SupportClass.PutElement(ht, root, res);
			}
			else
			{
				res = foo;
			}
			return res;
		}
		
		public ViewGraphNode()
		{
			subtreewidth = - 1;
			x = - 1;
			parentsChecked = false;
			y = - 1;
			shape = null;
			childs = new org.jamocha.rete.util.Arraylist();
			parents = new org.jamocha.rete.util.Arraylist();
		}
		
		/// <summary> Add a Node to its childs
		/// </summary>
		/// <param name="n">new node
		/// 
		/// </param>
		public virtual void  addToChilds(BaseNode n)
		{
			ViewGraphNode node = new ViewGraphNode(n);
			childs.add(node);
			node.parents.add(this);
			invalidateSubtreeWidth();
		}
		
		/// <summary> Add a Node to its childs
		/// </summary>
		/// <param name="n">new node
		/// 
		/// </param>
		public virtual void  addToChilds(ViewGraphNode n)
		{
			childs.add(n);
			n.parents.add(this);
			invalidateSubtreeWidth();
		}
		
		/// <summary> Returns the logical width of this node. It is 1 iff
		/// whichSubtree is the first parent. else 0
		/// </summary>
		/// <param name="">whichSubtree
		/// 
		/// </param>
		protected internal virtual int getMyWidth(ViewGraphNode whichSubtree)
		{
			if (parents.get(0) == whichSubtree)
				return 2;
			return 0;
		}
		
		
		
		
		
	}
}