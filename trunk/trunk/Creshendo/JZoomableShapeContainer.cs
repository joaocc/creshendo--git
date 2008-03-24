namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	using Iterator = java.util.Iterator;
	/// <author>  Josef Alexander Hahn
	/// A special JShapeContainer which can zoom.
	/// 
	/// </author>
	public class JZoomableShapeContainer:JShapeContainer
	{
		/// <returns> the actual zoom factor
		/// 
		/// </returns>
		virtual public double ZoomFactor
		{
			get
			{
				double factor = 1.0;
				for (int i = 1; i <= zoomLevel; i++)
				{
					factor *= 2.0;
				}
				for (int i = - 1; i >= zoomLevel; i--)
				{
					factor /= 2.0;
				}
				return factor;
			}
			
		}
		/// <summary> sets the corresponding mini-map
		/// </summary>
		/// <param name="radarShapeContainer">mini-map
		/// 
		/// </param>
		virtual public JMiniRadarShapeContainer RadarShapeContainer
		{
			get
			{
				return radarShapeContainer;
			}
			
			set
			{
				this.radarShapeContainer = value;
			}
			
		}
		
		protected internal JMiniRadarShapeContainer radarShapeContainer;
		internal int zoomLevel;
		
		public JZoomableShapeContainer():base()
		{
			zoomLevel = 0;
		}
		
		/// <summary> increases the zoom level
		/// </summary>
		public virtual void  zoomIn()
		{
			zoomLevel++;
			repaint();
			if (radarShapeContainer != null)
			{
				radarShapeContainer.componentResized(null);
				radarShapeContainer.repaint();
			}
		}
		
		/// <summary> decreases the zoom level
		/// </summary>
		public virtual void  zoomOut()
		{
			zoomLevel--;
			repaint();
			if (radarShapeContainer != null)
			{
				radarShapeContainer.componentResized(null);
				radarShapeContainer.repaint();
			}
		}
		
		
		protected internal override void  drawPrimitive(Primitive p, Graphics2D g)
		{
			double factor = ZoomFactor;
			p.draw(g, offsetX, offsetY, factor, factor);
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
		public override Shape getShapeAtPosition(int x, int y)
		{
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int rx = (int) System.Math.Round(x / ZoomFactor + offsetX);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int ry = (int) System.Math.Round(y / ZoomFactor + offsetY);
			for (Iterator it = shapes.iterator(); it.hasNext(); )
			{
				Shape s = (Shape) it.next();
				int offX = (rx - s.X);
				int offY = (ry - s.Y);
				if (offX >= 0 && offY >= 0 && offX <= s.Width && offY <= s.Height)
					return s;
			}
			return null;
		}
	}
}