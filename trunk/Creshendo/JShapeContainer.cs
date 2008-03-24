namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	using RenderingHints = java.awt.RenderingHints;
	using JComponent = javax.swing.JComponent;
	using List = java.util.List;
	using ArrayList = java.util.ArrayList;
	using Iterator = java.util.Iterator;
	/// <author>  Josef Alexander Hahn
	/// A Swing component which helds a list of primitives
	/// and draws them
	/// 
	/// </author>
	public class JShapeContainer:JComponent
	{
		/// <summary> sets the Font, which should be used while painting
		/// the short-description
		/// </summary>
		/// <param name="f">the font
		/// 
		/// </param>
		virtual public System.Drawing.Font Font
		{
			set
			{
				this.font = value;
			}
			
		}
		/// <returns> negative x-coordinate of the translation
		/// 
		/// </returns>
		/// <summary> sets the negative x-coordinate of the translation
		/// </summary>
		/// <param name="offsetX">negative x-coord
		/// 
		/// </param>
		virtual public int OffsetX
		{
			get
			{
				return offsetX;
			}
			
			set
			{
				this.offsetX = value;
			}
			
		}
		/// <returns> negative y-coordinate of the translation
		/// 
		/// </returns>
		/// <summary> sets the negative y-coordinate of the translation
		/// </summary>
		/// <param name="offsetX">negative y-coord
		/// 
		/// </param>
		virtual public int OffsetY
		{
			get
			{
				return offsetY;
			}
			
			set
			{
				this.offsetY = value;
			}
			
		}
		
		protected internal List lines;
		protected internal List shapes;
		protected internal int graphwidth;
		protected internal int graphheight;
		protected internal int offsetX;
		protected internal int offsetY;
		protected internal System.Drawing.Font font;
		
		public JShapeContainer()
		{
			lines = new ArrayList();
			shapes = new ArrayList();
			offsetX = offsetY = 0;
		}
		
		
		/// <summary> Adds a ConnectorLine into the container
		/// </summary>
		/// <param name="p">the primitive which should become added
		/// 
		/// </param>
		public virtual void  addPrimitive(ConnectorLine c)
		{
			lines.add(c);
			Graphics2D gr = (Graphics2D) Graphics;
			if (gr == null)
				return ;
			drawPrimitive(c, gr);
		}
		
		/// <summary> Adds a Shape into the container
		/// </summary>
		/// <param name="p">the primitive which should become added
		/// 
		/// </param>
		public virtual void  addPrimitive(Shape s)
		{
			shapes.add(s);
			if (s.width + s.x > graphwidth)
				graphwidth = s.width + s.x;
			if (s.height + s.y > graphheight)
				graphheight = s.height + s.y;
			Graphics2D gr = (Graphics2D) Graphics;
			if (gr == null)
				return ;
			drawPrimitive(s, gr);
		}
		
		/// <summary> Removes a Primitive from the container
		/// </summary>
		/// <param name="p">primitive to delete
		/// 
		/// </param>
		public virtual void  removePrimitive(Primitive p)
		{
			if (p is ConnectorLine)
				lines.remove(p);
			if (p is Shape)
				shapes.remove(p);
			repaint();
		}
		
		/// <summary> Flushes the container
		/// </summary>
		public virtual void  removeAllPrimitives()
		{
			lines.clear();
			shapes.clear();
			repaint();
		}
		
		protected internal virtual void  drawPrimitive(Primitive p, Graphics2D g)
		{
			p.draw(g, offsetX, offsetY);
		}
		
		
		
		public virtual void  paint(System.Drawing.Graphics g)
		{
			Graphics2D gr = (Graphics2D) g;
			gr.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
			gr.setFont(font);
			Iterator itshapes = shapes.iterator();
			Iterator itarrows = lines.iterator();
			while (itshapes.hasNext())
			{
				Shape s = (Shape) itshapes.next();
				drawPrimitive(s, gr);
			}
			while (itarrows.hasNext())
			{
				ConnectorLine l = (ConnectorLine) itarrows.next();
				drawPrimitive(l, gr);
			}
		}
		
		
		
		/// <summary> Returns the Shape at the given position
		/// </summary>
		/// <param name="x">x-coordinate of the absolute position
		/// </param>
		/// <param name="y">y-coordinate of the absolute position
		/// </param>
		/// <returns> the shape at that position
		/// 
		/// </returns>
		public virtual Shape getShapeAtPosition(int x, int y)
		{
			//TODO: Not so efficient. Later, maybe, we should use a tricky
			//      data structure for finding the shape faster ;)
			for (Iterator it = shapes.iterator(); it.hasNext(); )
			{
				Shape s = (Shape) it.next();
				int offX = (x - s.X);
				int offY = (y - s.Y);
				if (offX >= 0 && offY >= 0 && offX <= s.Width && offY <= s.Height)
					return s;
			}
			return null;
		}
	}
}