namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// This is a concrete Shape-Implementation for an
	/// rectangle with fully-rounded left and right sides
	/// with a topleft-point, a width, a height
	/// 
	/// </author>
	public class RoundedRectangle:Shape
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
		
		public RoundedRectangle(System.Drawing.Color bgcolor, System.Drawing.Color bordercolor, int x, int y, int width, int height, System.String text):base(bgcolor, bordercolor, x, y, width, height, text)
		{
		}
		
		public RoundedRectangle():base()
		{
		}
		
		
		
		protected internal virtual void  calculateCornerAngles()
		{
			angleTopRight = System.Math.Atan2(height, width - height);
			angleTopLeft = System.Math.PI - angleTopRight;
			angleBottomRight = - angleTopRight;
			angleBottomLeft = - angleTopLeft;
		}
		
		public virtual void  draw(Graphics2D canvas, int offsetX, int offsetY)
		{
			draw(canvas, offsetX, offsetY, 1.0, 1.0);
		}
		
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
			// draw the left oval
			canvas.setColor(bgcolor);
			canvas.fillOval(x, y, height + 1, height + 1);
			canvas.setColor(bordercolor);
			canvas.drawOval(x, y, height, height);
			// draw the right oval
			canvas.setColor(bgcolor);
			canvas.fillOval(x + width - height, y, height + 1, height + 1);
			canvas.setColor(bordercolor);
			canvas.drawOval(x + width - height, y, height, height);
			// draw the center rectangle
			canvas.setColor(bgcolor);
			canvas.fillRect(x + (height / 2), y, width - height + 1, height + 1);
			canvas.setColor(bordercolor);
			canvas.drawLine(x + (height / 2), y, x + width - height / 2, y);
			canvas.drawLine(x + (height / 2), y + height + 1, x + width - height / 2, y + height + 1);
			
			// draw the text	
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
		
		public virtual void  draw(Graphics2D canvas, double factorX, double factorY)
		{
			draw(canvas, 0, 0, factorX, factorY);
		}
		
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
				double ys = System.Math.Tan(alpha) * (width - height) * 0.5;
				double newalpha = System.Math.PI - System.Math.Acos(ys / height);
				double x0 = height * System.Math.Cos(newalpha);
				double y0 = height * System.Math.Sin(newalpha);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(x + (height * 0.5) - x0);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(y + (height * 0.5) - y0);
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
				double ys = System.Math.Tan(alpha) * (width - height) * 0.5;
				double newalpha = System.Math.PI - System.Math.Acos(ys / height);
				double x0 = height * System.Math.Cos(newalpha);
				double y0 = height * System.Math.Sin(newalpha);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(x + (width - height * 0.5) + x0);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(y + (height * 0.5) - y0);
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