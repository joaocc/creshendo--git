namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// This is a concrete Shape-Implementation for an
	/// trapezoid with a topleft-point, a width, a height
	/// in that way, that the top line is 50% of the bottom line
	/// and centered.
	/// 
	/// </author>
	public class Trapezoid:Shape
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
		
		public Trapezoid(System.Drawing.Color bgcolor, System.Drawing.Color bordercolor, int x, int y, int width, int height, System.String text):base(bgcolor, bordercolor, x, y, width, height, text)
		{
		}
		
		public Trapezoid():base()
		{
		}
		
		
		
		protected internal virtual void  calculateCornerAngles()
		{
			angleTopRight = System.Math.Atan2(height, width * 0.5);
			angleTopLeft = System.Math.PI - angleTopRight;
			angleBottomRight = - System.Math.Atan2(height, width);
			angleBottomLeft = - angleTopLeft;
		}
		
		public virtual void  draw(Graphics2D canvas, int offsetX, int offsetY)
		{
			draw(canvas, offsetX, offsetY, 1.0, 1.0);
		}
		
		public virtual void  draw(Graphics2D canvas, int offsetX, int offsetY, double factorX, double factorY)
		{
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int ourX = (int) System.Math.Round((x - offsetX) * factorX);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int ourY = (int) System.Math.Round((y - offsetY) * factorY);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int ourWidth = (int) System.Math.Round(width * factorX);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int ourHeight = (int) System.Math.Round(height * factorY);
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int[] xpoints = new int[]{ourX + (int) (ourWidth * .25), ourX + (int) (ourWidth * .75), ourX + ourWidth, ourX};
			int[] ypoints = new int[]{ourY, ourY, ourY + ourHeight, ourY + ourHeight};
			canvas.setColor(bgcolor);
			canvas.fillPolygon(xpoints, ypoints, 4);
			canvas.setColor(bordercolor);
			canvas.drawPolygon(xpoints, ypoints, 4);
			canvas.setColor(System.Drawing.Color.Black);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRGB' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			if (bgcolor.ToArgb() == System.Drawing.Color.Black.ToArgb())
				canvas.setColor(System.Drawing.Color.White);
			if (ourHeight > 10)
			{
				System.Drawing.Point textpos = calculateTextPosition(text, canvas, ourWidth, ourHeight);
				canvas.drawString(text, (int) textpos.X + ourX, (int) textpos.Y + ourY);
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
				double dy = (System.Math.Tan(alpha) * width + height) / 4.0;
				double dx = (dy * width) / (4.0 * height);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(x + dx);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(- dy + y + height);
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
				double dy = (System.Math.Tan(alpha) * width + height) / 4.0;
				double dx = (dy * width) / (4.0 * height);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.X = (int) System.Math.Round(x + width - dx);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				result.Y = (int) System.Math.Round(- dy + y + height);
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