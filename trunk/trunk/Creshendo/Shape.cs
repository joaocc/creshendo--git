namespace org.jamocha.rete.visualisation
{
	using System;
	/// <author>  Josef Alexander Hahn
	/// abstract Shape-Class for use in visualiser
	/// 
	/// </author>
	public abstract class Shape:Primitive
	{
		/// <summary> gets the optional long description text
		/// </summary>
		/// <returns> long description text
		/// 
		/// </returns>
		/// <summary> sets the optional long description text
		/// (will not become printed with the shape)
		/// </summary>
		/// <param name="val">long description text
		/// 
		/// </param>
		virtual public System.String LongDescription
		{
			get
			{
				return longDescription;
			}
			
			set
			{
				longDescription = value;
			}
			
		}
		/// <returns> the fill-color
		/// 
		/// </returns>
		/// <summary> sets the fill-color
		/// </summary>
		/// <param name="bgcolor">fill-color
		/// 
		/// </param>
		virtual public System.Drawing.Color Bgcolor
		{
			get
			{
				return bgcolor;
			}
			
			set
			{
				this.bgcolor = value;
			}
			
		}
		/// <returns> the border-color
		/// 
		/// </returns>
		/// <summary> sets the border-color
		/// </summary>
		/// <param name="bordercolor">bordercolor
		/// 
		/// </param>
		virtual public System.Drawing.Color Bordercolor
		{
			get
			{
				return bordercolor;
			}
			
			set
			{
				this.bordercolor = value;
			}
			
		}
		/// <returns> x-coordinate of the topleft-point
		/// 
		/// </returns>
		/// <summary> sets the shape-position
		/// </summary>
		/// <param name="x">the x-coordinate of the topleft-point
		/// 
		/// </param>
		virtual public int X
		{
			get
			{
				return x;
			}
			
			set
			{
				this.x = value;
			}
			
		}
		/// <returns> y-coordinate of the topleft-point
		/// 
		/// </returns>
		/// <summary> sets the shape-position
		/// </summary>
		/// <param name="y">the y-coordinate of the topleft-point
		/// 
		/// </param>
		virtual public int Y
		{
			get
			{
				return y;
			}
			
			set
			{
				this.y = value;
			}
			
		}
		/// <returns> width of the shape
		/// 
		/// </returns>
		/// <summary> sets the width of the shape
		/// </summary>
		/// <param name="width">width
		/// 
		/// </param>
		virtual public int Width
		{
			get
			{
				return width;
			}
			
			set
			{
				this.width = value;
			}
			
		}
		/// <returns> height of the shape
		/// 
		/// </returns>
		/// <summary> sets the height of the shape
		/// </summary>
		/// <param name="height">height
		/// 
		/// </param>
		virtual public int Height
		{
			get
			{
				return height;
			}
			
			set
			{
				this.height = value;
			}
			
		}
		/// <returns> the short description
		/// 
		/// </returns>
		/// <summary> sets the short description which will be
		/// printed with the shape
		/// </summary>
		/// <param name="text">the short description
		/// 
		/// </param>
		virtual public System.String Text
		{
			get
			{
				return text;
			}
			
			set
			{
				this.text = value;
			}
			
		}
		protected internal System.Drawing.Color bgcolor;
		protected internal System.Drawing.Color bordercolor;
		protected internal int x;
		protected internal int y;
		protected internal int width;
		protected internal int height;
		protected internal System.String text;
		protected internal System.String longDescription;
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		/// <summary> increments the height _with invariant centre_
		/// </summary>
		/// <param name="dh">height to add
		/// 
		/// </param>
		public virtual void  incHeight(int dh)
		{
			y -= dh / 2;
			height += dh;
		}
		
		/// <summary> increments the width _with invariant centre_
		/// </summary>
		/// <param name="dh">width to add
		/// 
		/// </param>
		public virtual void  incWidth(int dw)
		{
			x -= dw / 2;
			width += dw;
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
		public abstract System.Drawing.Point calculateIntersection(double angle);
		
		protected internal virtual System.Drawing.Point calculateTextPosition(System.String text, System.Drawing.Graphics g, int width, int height)
		{
			int stringHeight = (int) SupportClass.GraphicsManager.manager.GetFont(g).getLineMetrics(text, g).Height;
			//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFontMetricsstringWidth_javalangString"'
			int stringWidth = SupportClass.GraphicsManager.manager.GetFont(g).stringWidth(text);
			int xpos = (width - stringWidth) / 2;
			int ypos = (height + stringHeight) / 2;
			return new System.Drawing.Point(xpos, ypos);
		}
		
		internal Shape():base()
		{
		}
		
		
		/// <param name="bgcolor">The Fill-Color the shape should get
		/// </param>
		/// <param name="bordercolor">The Border-Color the shape should get
		/// </param>
		/// <param name="x">the x-coordinate of the topleft-point
		/// </param>
		/// <param name="y">the y-coordinate of the topleft-point
		/// </param>
		/// <param name="width">the width of the shape
		/// </param>
		/// <param name="height">the height of the shape
		/// </param>
		/// <param name="text">the short-description which will be drawn into
		/// 
		/// </param>
		internal Shape(System.Drawing.Color bgcolor, System.Drawing.Color bordercolor, int x, int y, int width, int height, System.String text):base()
		{
			this.bgcolor = bgcolor;
			this.bordercolor = bordercolor;
			this.x = x;
			this.y = y;
			Width = width;
			Height = height;
			this.text = text;
		}
	}
}