namespace org.jamocha.rete.visualisation
{
	using System;
	using BasicStroke = java.awt.BasicStroke;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// This Class represents a connection between two Shapes,
	/// which will be drawn as an arrow
	/// 
	/// </author>
	public class ConnectorLine:Primitive
	{
		/// <summary> Returns the color of the representing arrow
		/// </summary>
		/// <returns> Actual arrow color
		/// 
		/// </returns>
		/// <summary> Sets the color of the representing arrow
		/// </summary>
		/// <param name="color">Arrow color
		/// 
		/// </param>
		virtual public System.Drawing.Color Color
		{
			get
			{
				return color;
			}
			
			set
			{
				this.color = value;
			}
			
		}
		
		private Shape from;
		private Shape to;
		private System.Drawing.Color color;
		
		
		
		/// <summary> Draws an arrow from the From-Shape to the To-Shape
		/// with the choosen color. The draw-position is translated
		/// by (-offsetX,-offsetY).
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
		
		/// <summary> Draws an arrow from the From-Shape to the To-Shape
		/// with the choosen color. It will be scaled by (factorX,factorY).
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
		
		/// <summary> Draws an arrow from the From-Shape to the To-Shape
		/// with the choosen color. The draw-position is translated
		/// by (-offsetX,-offsetY) and then scaled by (factorX,factorY).
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
			canvas.setColor(color);
			
			/* angleFromTo is this angle ;)
			* (between -PI;+PI)
			*                     TO
			*    ----------------(x)-------------------
			*                   /
			*                 /)
			*               / @ )
			*    --------(x)---------------------------
			*            FROM 
			*/
			double angleFromTo = System.Math.Atan2(- to.Y + from.Y, to.X - from.X);
			
			/* angleToFrom is this angle ;)
			* (between -PI;+PI)
			*                     TO
			*    ----------------(x)-------------------
			*               ( @ /
			*                (/
			*               /
			*    --------(x)---------------------------
			*            FROM 
			*/
			double angleToFrom;
			if (angleFromTo > 0)
			{
				angleToFrom = angleFromTo - System.Math.PI;
			}
			else
			{
				angleToFrom = angleFromTo + System.Math.PI;
			}
			System.Drawing.Point pfrom = from.calculateIntersection(angleFromTo);
			System.Drawing.Point pto = to.calculateIntersection(angleToFrom);
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			System.Drawing.Point parrow1 = new System.Drawing.Point((int) System.Math.Round(6 * System.Math.Cos(angleToFrom + System.Math.PI / 4.0)), (int) System.Math.Round(6 * System.Math.Sin(angleToFrom + System.Math.PI / 4.0)));
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			System.Drawing.Point parrow2 = new System.Drawing.Point((int) System.Math.Round(6 * System.Math.Cos(angleToFrom - System.Math.PI / 4.0)), (int) System.Math.Round(6 * System.Math.Sin(angleToFrom - System.Math.PI / 4.0)));
			pfrom.X -= offsetX;
			pfrom.Y -= offsetY;
			pto.X -= offsetX;
			pto.Y -= offsetY;
			pfrom.X *= (int) (factorX);
			pfrom.Y *= (int) (factorY);
			pto.X *= (int) (factorX);
			pto.Y *= (int) (factorY);
			parrow1.X *= (int) (factorX);
			parrow1.Y *= (int) (factorY);
			parrow2.X *= (int) (factorX);
			parrow2.Y *= (int) (factorY);
			
			//calculate a good line width
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			int linewidth = (int) System.Math.Round(1 * System.Math.Min(factorX, factorY));
			if (linewidth == 0)
				linewidth = 1;
			canvas.setStroke(new BasicStroke(linewidth));
			
			//draw the arrow
			canvas.drawLine(pfrom.X, pfrom.Y, pto.X, pto.Y);
			canvas.drawLine(pto.X + parrow1.X, pto.Y - parrow1.Y, pto.X, pto.Y);
			canvas.drawLine(pto.X + parrow2.X, pto.Y - parrow2.Y, pto.X, pto.Y);
		}
		
		/// <param name="from">The From-Shape for the new ConnectorLine
		/// </param>
		/// <param name="to">The To-Shape for the new ConnectorLine
		/// 
		/// </param>
		public ConnectorLine(Shape from, Shape to)
		{
			this.from = from;
			this.to = to;
		}
	}
}