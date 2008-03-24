namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// This is a concrete Shape-Implementation for an
	/// rectangle with a topleft-point, a width, a height
	/// 
	/// </author>
	public class Rectangle:Shape
	{
		override public int Width
		{
			set
			{
				base.Width = value;
				calculateCornerAngles();
			}
			
		}
		override public int Height
		{
			set
			{
				base.Height = value;
				calculateCornerAngles();
			}
			
		}
		
		protected internal double angleTopLeft;
		protected internal double angleTopRight;
		protected internal double angleBottomLeft;
		protected internal double angleBottomRight;
		
		/// <param name="bgcolor">The Fill-Color the ellipse should get
		/// </param>
		/// <param name="bordercolor">The Border-Color the ellipse should get
		/// </param>
		/// <param name="x">the x-coordinate of the centre
		/// </param>
		/// <param name="y">the y-coordinate of the centre
		/// </param>
		/// <param name="width">the width of the ellipse
		/// </param>
		/// <param name="height">the height of the ellipse
		/// </param>
		/// <param name="text">the short-description which will be drawn into
		/// 
		/// </param>
		public Rectangle(System.Drawing.Color bgcolor, System.Drawing.Color bordercolor, int x, int y, int width, int height, System.String text):base(bgcolor, bordercolor, x, y, width, height, text)
		{
		}
		
		public Rectangle():base()
		{
		}
		
		
		
		protected internal virtual void  calculateCornerAngles()
		{
			angleTopRight = System.Math.Atan2(height, width);
			angleTopLeft = System.Math.PI - angleTopRight;
			angleBottomRight = - angleTopRight;
			angleBottomLeft = - angleTopLeft;
		}
		
		/// <summary> Draws the rectangle.
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
		
		/// <summary> Draws the rectangle.
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
			canvas.setColor(bgcolor);
			canvas.fillRect(x, y, width + 1, height + 1);
			canvas.setColor(bordercolor);
			canvas.drawRect(x, y, width, height);
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
		
		/// <summary> Draws the rectangle.
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
		
		/// <summary> Calculates the intersection-coordinate between
		/// the borderline of that shape and a line, starting in
		/// the centre with the given angle
		/// </summary>
		/// <param name="angle">the angle to the x-axis
		/// </param>
		/// <returns> the intersection-point
		/// 
		/// </returns>
		public override System.Drawing.Point calculateIntersection(double angle)
		{
			System.Drawing.Point result = new System.Drawing.Point(0, 0);
			if (angle > angleTopLeft || angle < angleBottomLeft)
			{
				// left
				double alpha;
				if (angle > 0)
				{
					alpha = System.Math.PI - angle;
				}
				else
				{
					alpha = - angle - System.Math.PI;
				}
				result.X = x;
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(- System.Math.Tan(alpha) * width * 0.5 + y + height * 0.5);
			}
			else if (angle > angleTopRight)
			{
				// top
				double alpha = System.Math.PI * 0.5 - angle;
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(System.Math.Tan(alpha) * height * 0.5 + x + width * 0.5);
				result.Y = y;
			}
			else if (angle > angleBottomRight)
			{
				// right
				double alpha = angle;
				result.X = x + width;
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(- System.Math.Tan(alpha) * width * 0.5 + y + height * 0.5);
			}
			else
			{
				// bottom
				double alpha = System.Math.PI * 0.5 + angle;
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(System.Math.Tan(alpha) * height * 0.5 + x + width * 0.5);
				result.Y = y + height;
			}
			return result;
		}
	}
}