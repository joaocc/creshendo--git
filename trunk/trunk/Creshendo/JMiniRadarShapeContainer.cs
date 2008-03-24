namespace org.jamocha.rete.visualisation
{
	using System;
	using Graphics2D = java.awt.Graphics2D;
	/// <author>  Josef Alexander Hahn
	/// A special JShapeContainer which can be used as
	/// mini-map. Here you can scroll in the network and
	/// the new offset is given to the MasterShapeContainer.
	/// 
	/// </author>
	public class JMiniRadarShapeContainer:JShapeContainer
	{
		/// <summary> Sets the font height in pixels (normalized by the scaling factor,
		/// which means, when scaling-factor=1)
		/// </summary>
		/// <param name="h">font height in pixels
		/// 
		/// </param>
		virtual public int NormalizedFontHeight
		{
			set
			{
				normalizedFontHeight = value;
			}
			
		}
		/// <summary> Returns the actual scaling factor of the mini-map
		/// </summary>
		/// <returns> scaling factor
		/// 
		/// </returns>
		virtual public double Factor
		{
			get
			{
				double factorX = ((double) Width) / ((double) graphwidth + 10);
				double factorY = ((double) Height) / ((double) graphheight + 10);
				double factor = System.Math.Min(factorX, factorY);
				return factor;
			}
			
		}
		virtual public System.Drawing.Size PreferredSize
		{
			get
			{
				return new System.Drawing.Size(150, 100);
			}
			
		}
		/// <returns> the MasterShapeContainer. That the
		/// JShapeContainer which is controlled by
		/// this MiniRadar.
		/// 
		/// </returns>
		virtual public JShapeContainer MasterShapeContainer
		{
			get
			{
				return masterShapeContainer;
			}
			
		}
		//UPGRADE_TODO: Method 'setMasterShapeContainer' was converted to a set modifier. This name conflicts with another property. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1137"'
		/// <summary> sets the MasterShapeContainer. That the
		/// JShapeContainer which is controlled by
		/// this MiniRadar.
		/// </summary>
		/// <param name="masterShapeContainer">The MasterShapeContainer
		/// 
		/// </param>
		virtual public JZoomableShapeContainer MasterShapeContainer
		{
			set
			{
				this.masterShapeContainer = value;
				value.addComponentListener(this);
			}
			
		}
		
		protected internal JZoomableShapeContainer masterShapeContainer;
		new protected internal int offsetX;
		new protected internal int offsetY;
		protected internal int normalizedFontHeight;
		// that is the Font-Height in pixel
		// which should be used normalized by
		// the scaling-factor
		
		public JMiniRadarShapeContainer():base()
		{
			this.addMouseListener(this);
			this.addMouseMotionListener(this);
			offsetX = offsetY = 0;
		}
		
		
		/// <summary> Paints itself to the given Graphics
		/// </summary>
		/// <param name="g">The Graphics-canvas
		/// 
		/// </param>
		public override void  paint(System.Drawing.Graphics g)
		{
			base.paint(g);
			System.Drawing.Color clr = new Color(100, 100, 255, 100);
			SupportClass.GraphicsManager.manager.SetColor(g, clr);
			double zoomFactor = masterShapeContainer.ZoomFactor;
			double factorX = ((double) Width) / ((double) graphwidth + 10);
			double factorY = ((double) Height) / ((double) graphheight + 10);
			double factor = System.Math.Min(factorX, factorY);
			int rectwidth = (int) (masterShapeContainer.Width * factor / zoomFactor) + 1;
			int rectheight = (int) (masterShapeContainer.Height * factor / zoomFactor) + 1;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int rectx = (int) (offsetX * factor);
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int recty = (int) (offsetY * factor);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetBrush(g), rectx, recty, rectwidth, rectheight);
		}
		
		
		protected internal override void  drawPrimitive(Primitive p, Graphics2D g)
		{
			double factor = Factor;
			p.draw(g, factor, factor);
		}
		
		
		/// <summary> Adds a Shape into the container
		/// </summary>
		/// <param name="p">the primitive which should become added
		/// 
		/// </param>
		public override void  addPrimitive(Shape s)
		{
			base.addPrimitive(s);
			if (s.width + s.x > graphwidth)
			{
				graphwidth = s.width + s.x;
				repaint();
			}
			if (s.height + s.y > graphheight)
			{
				graphheight = s.height + s.y;
				repaint();
			}
		}
		
		
		
		public virtual void  mouseClicked(System.Object event_sender, System.EventArgs arg0)
		{
			radarNewPosition(arg0.X, arg0.Y);
		}
		
		public virtual void  mouseDragged(System.Object event_sender, System.Windows.Forms.MouseEventArgs arg0)
		{
			//UPGRADE_TODO: Method java.awt.event.MouseListener.mouseClicked was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
			mouseClicked(event_sender, arg0);
		}
		
		public virtual void  mouseEntered(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  mouseExited(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  mousePressed(System.Object event_sender, System.Windows.Forms.MouseEventArgs arg0)
		{
		}
		public virtual void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs arg0)
		{
		}
		public virtual void  mouseMoved(System.Object event_sender, System.Windows.Forms.MouseEventArgs arg0)
		{
		}
		public virtual void  componentHidden(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  componentMoved(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  componentShown(System.Object event_sender, System.EventArgs arg0)
		{
		}
		
		protected internal virtual void  radarNewPosition(int x, int y)
		{
			double masterZoomFactor = masterShapeContainer.ZoomFactor;
			double myScalingFactor = Factor;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int x1 = (int) (x / myScalingFactor);
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int y1 = (int) (y / myScalingFactor);
			x1 -= masterShapeContainer.Width / 2 / masterZoomFactor;
			y1 -= masterShapeContainer.Height / 2 / masterZoomFactor;
			int offsetXmax = graphwidth + 10 - (int) (masterShapeContainer.Width / masterZoomFactor);
			int offsetYmax = graphheight + 10 - (int) (masterShapeContainer.Height / masterZoomFactor);
			if (x1 > offsetXmax)
				x1 = offsetXmax;
			if (y1 > offsetYmax)
				y1 = offsetYmax;
			if (x1 < 0)
				x1 = 0;
			if (y1 < 0)
				y1 = 0;
			masterShapeContainer.OffsetX = x1;
			masterShapeContainer.OffsetY = y1;
			offsetX = x1;
			offsetY = y1;
			masterShapeContainer.repaint();
			this.repaint();
		}
		
		public virtual void  componentResized(System.Object event_sender, System.EventArgs arg0)
		{
			bool weHadToChangeOffset = false;
			double zoomFactor = masterShapeContainer.ZoomFactor;
			int offsetXmax = graphwidth + 10 - (int) (masterShapeContainer.Width / zoomFactor);
			int offsetYmax = graphheight + 10 - (int) (masterShapeContainer.Height / zoomFactor);
			if (offsetX > offsetXmax)
			{
				offsetX = offsetXmax; weHadToChangeOffset = true;
			}
			if (offsetY > offsetYmax)
			{
				offsetY = offsetYmax; weHadToChangeOffset = true;
			}
			if (offsetX < 0)
			{
				offsetX = 0; weHadToChangeOffset = true;
			}
			if (offsetY < 0)
			{
				offsetY = 0; weHadToChangeOffset = true;
			}
			if (weHadToChangeOffset)
			{
				masterShapeContainer.OffsetX = offsetX;
				masterShapeContainer.OffsetY = offsetY;
			}
			
			//calculate good font size
			int dpi = Toolkit.ScreenResolution;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int ppRadarContainer = (int) ((normalizedFontHeight * Factor / dpi) * 72);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			Font = new System.Drawing.Font("SansSerif", ppRadarContainer, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular);
			
			repaint();
		}
	}
}