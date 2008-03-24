namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// This is a concrete Shape-Implementation for an
	/// ellipse with a topleft-point, a width, a height
	/// 
	/// </author>
	public class Ellipse:Shape
	{
		
		/// <param name="bgcolor">The Fill-Color the ellipse should get
		/// </param>
		/// <param name="bordercolor">The Border-Color the ellipse should get
		/// </param>
		/// <param name="x">the x-coordinate of the topleft-point
		/// </param>
		/// <param name="y">the y-coordinate of the topleft-point
		/// </param>
		/// <param name="width">the width of the ellipse
		/// </param>
		/// <param name="height">the height of the ellipse
		/// </param>
		/// <param name="text">the short-description which will be drawn into
		/// 
		/// </param>
		public Ellipse(System.Drawing.Color bgcolor, System.Drawing.Color bordercolor, int x, int y, int width, int height, System.String text):base(bgcolor, bordercolor, x, y, width, height, text)
		{
		}
		
		public Ellipse():base()
		{
		}
		
		/// <summary> Draws the ellipse.
		/// The draw-position is translated by (-offsetX,-offsetY).
		/// </summary>
		/// <param name="canvas">The canvas to draw the arrow on
		/// </param>
		/// <param name="offsetX">Translation-Vector's negative x-component
		/// </param>
		/// <param name="offsetY">Translation-Vector's negative y-component
		/// 
		/// </param>
		public virtual void  draw(Graphics2D canvas, int offsetX, int offsetY)
		{
			draw(canvas, offsetX, offsetY, 1.0, 1.0);
		}
		
		/// <summary> Draws the ellipse.
		/// The draw-position is translated by (-offsetX,-offsetY)
		/// and scaled by (factorX,factorY).
		/// </summary>
		/// <param name="canvas">The canvas to draw the arrow on
		/// </param>
		/// <param name="offsetX">Translation-Vector's negative x-component
		/// </param>
		/// <param name="offsetY">Translation-Vector's negative y-component
		/// </param>
		/// <param name="factorX">Scaling-Vector*s x-component
		/// </param>
		/// <param name="factorY">Scaling-Vector*s y-component
		/// 
		/// </param>
		public virtual void  draw(Graphics2D canvas, int offsetX, int offsetY, double factorX, double factorY)
		{
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int x = (int) System.Math.Round((this.x - offsetX) * factorX);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int y = (int) System.Math.Round((this.y - offsetY) * factorY);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int width = (int) System.Math.Round(this.width * factorX);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int height = (int) System.Math.Round(this.height * factorY);
			// set colors and draw
			canvas.setColor(bgcolor);
			canvas.fillOval(x, y, width + 1, height + 1);
			canvas.setColor(bordercolor);
			canvas.drawOval(x, y, width, height);
			// draw short-description
			canvas.setColor(System.Drawing.Color.Black);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRGB' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			if (bgcolor.ToArgb() == System.Drawing.Color.Black.ToArgb())
				canvas.setColor(System.Drawing.Color.White);
			if (height > 10)
			{
				System.Drawing.Point textpos = calculateTextPosition(text, canvas, width, height);
				canvas.drawString(text, (int) textpos.X + x, (int) textpos.Y + y);
			}
		}
		
		/// <summary> Draws the ellipse.
		/// The draw-position is scaled by (factorX,factorY).
		/// </summary>
		/// <param name="canvas">The canvas to draw the arrow on
		/// </param>
		/// <param name="factorX">Scaling-Vector*s x-component
		/// </param>
		/// <param name="factorY">Scaling-Vector*s y-component
		/// 
		/// </param>
		public virtual void  draw(Graphics2D canvas, double factorX, double factorY)
		{
			draw(canvas, 0, 0, factorX, factorY);
		}
		
		public override System.Drawing.Point calculateIntersection(double angle)
		{
			System.Drawing.Point result = new System.Drawing.Point(0, 0);
			//TODO: That calculation is NOT correct! That leads to wrong angles
			//      in the visualiser. looks not sooo good, but not that problem
			//      for now ;)
			//double r=Math.atan( Math.tan(angle) * ((double)width/(double)height));
			double r = System.Math.Atan2(System.Math.Sin(angle) * width, System.Math.Cos(angle) * height);
			double xrel = System.Math.Cos(r) * width / 2.0;
			double yrel = System.Math.Sin(r) * height / 2.0;
			
			
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			result.X = (int) System.Math.Round(xrel + x + (width * 0.5));
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			result.Y = (int) System.Math.Round(- yrel + y + (height * 0.5));
			return result;
		}
	}
}