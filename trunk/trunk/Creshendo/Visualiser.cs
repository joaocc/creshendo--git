namespace org.jamocha.rete.visualisation
{
	using System;
	using HashSet = java.util.HashSet;
	using Iterator = java.util.Iterator;
	using LinkedList = java.util.LinkedList;
	using Set = java.util.Set;
	using JButton = javax.swing.JButton;
	using JFrame = javax.swing.JFrame;
	using JPanel = javax.swing.JPanel;
	using JScrollPane = javax.swing.JScrollPane;
	using JSplitPane = javax.swing.JSplitPane;
	using JTextPane = javax.swing.JTextPane;
	using JToggleButton = javax.swing.JToggleButton;
	using BadLocationException = javax.swing.text.BadLocationException;
	using SimpleAttributeSet = javax.swing.text.SimpleAttributeSet;
	using StyleConstants = javax.swing.text.StyleConstants;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using AlphaNodePredConstr = org.jamocha.rete.AlphaNodePredConstr;
	using BaseAlpha2 = org.jamocha.rete.BaseAlpha2;
	using BaseJoin = org.jamocha.rete.BaseJoin;
	using BaseNode = org.jamocha.rete.BaseNode;
	using EngineEvent = org.jamocha.rete.EngineEvent;
	using EngineEventListener = org.jamocha.rete.EngineEventListener;
	using LIANode = org.jamocha.rete.LIANode;
	using ObjectTypeNode = org.jamocha.rete.ObjectTypeNode;
	using Rete = org.jamocha.rete.Rete;
	using RootNode = org.jamocha.rete.RootNode;
	using TerminalNode = org.jamocha.rete.TerminalNode;
	/// <author>  Josef Alexander Hahn
	/// a class which can visualise a rete-network
	/// this class does not extend a swing-class since
	/// it has a method show for creating and opening
	/// a window. but you can get a JPanel by calling
	/// getVisualiserPanel. That JPanel-instance you can
	/// embed somewhere.
	/// 
	/// </author>
	public class Visualiser : EngineEventListener
	{
		private void  InitBlock()
		{
			coordinates = new System.Collections.Hashtable();
		}
		virtual protected internal JFrame MyFrame
		{
			set
			{
				myFrame = value;
			}
			
		}
		/// <returns> a JPanel, which contains the whole visualiser. you can embed it.
		/// 
		/// </returns>
		virtual public JPanel VisualiserPanel
		{
			get
			{
				JPanel panel = new JPanel();
				// we set the preferred size so that when (view) command is executed, the
				// window displays correct without having to resize the window
				panel.setPreferredSize(new System.Drawing.Size(700, 450));
				JPanel toolBox = new JPanel();
				//UPGRADE_ISSUE: Class 'java.awt.GridLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridLayout"'
				//UPGRADE_ISSUE: Constructor 'java.awt.GridLayout.GridLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridLayout"'
				GridLayout toolBoxLayout = new GridLayout(2, 1);
				toolBox.setLayout(toolBoxLayout);
				
				// Zoom Buttons
				zoomInButton = new JButton("Zoom In", IconLoader.getImageIcon("magnifier_zoom_in", typeof(Visualiser)));
				zoomOutButton = new JButton("Zoom Out", IconLoader.getImageIcon("magnifier_zoom_out", typeof(Visualiser)));
				zoomInButton.addActionListener(this);
				zoomOutButton.addActionListener(this);
				
				// Dump Field
				dump = new JTextPane();
				scrollPane = new JScrollPane(dump);
				dump.setText("This is the node dump area. Click on a node and you will get some information here\n");
				scrollPane.setAutoscrolls(true);
				//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				JPanel dumpPanel = new JPanel(new BorderLayout());
				dumpPanel.add(scrollPane);
				
				// Sidebar (Where Toolbox and Dump Field is; NOT the radar)
				//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				JPanel sideBar = new JPanel(new BorderLayout());
				//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				sideBar.add(toolBox, BorderLayout.WEST);
				//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				sideBar.add(dumpPanel, BorderLayout.CENTER);
				
				// Main Window with two Splitters (between radar, sidebar and main)
				//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				panel.setLayout(new BorderLayout());
				JSplitPane sideSplitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT, radar, sideBar);
				JSplitPane mainSplitPane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, container, sideSplitPane);
				mainSplitPane.setResizeWeight(1.0);
				mainSplitPane.setOneTouchExpandable(true);
				sideSplitPane.setOneTouchExpandable(true);
				//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
				panel.add(mainSplitPane, BorderLayout.CENTER);
				
				// adding the buttons to buttonPanel
				JPanel buttonPanel = new JPanel();
				//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
				//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
				buttonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
				buttonPanel.add(zoomInButton);
				buttonPanel.add(zoomOutButton);
				autoReloadButton = new JToggleButton("Automatic Reload", IconLoader.getImageIcon("arrow_refresh"));
				autoReloadButton.setSelected(false);
				autoReloadButton.addActionListener(this);
				buttonPanel.add(autoReloadButton);
				reloadButton = new JButton("Reload View", IconLoader.getImageIcon("arrow_refresh"));
				reloadButton.addActionListener(this);
				buttonPanel.add(reloadButton);
				//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
				panel.add(buttonPanel, BorderLayout.PAGE_END);
				
				return panel;
			}
			
		}
		protected internal JZoomableShapeContainer container;
		protected internal JMiniRadarShapeContainer radar;
		protected internal ViewGraphNode root;
		protected internal JButton zoomInButton, zoomOutButton, reloadButton;
		protected internal JScrollPane scrollPane;
		protected internal JToggleButton autoReloadButton;
		protected internal JTextPane dump;
		protected internal JFrame myFrame;
		protected internal Rete engine;
		protected internal bool dumpEmpty = true;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spaceHorizontal '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		protected internal int spaceHorizontal = 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spaceVertical '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		protected internal int spaceVertical = 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nodeHorizontal '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		protected internal int nodeHorizontal = 45;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nodeVertical '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		protected internal int nodeVertical = 16;
		protected internal SimpleAttributeSet even, odd, actAttributes;
		//UPGRADE_NOTE: The initialization of  'coordinates' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		protected internal System.Collections.Hashtable coordinates;
		
		protected internal virtual System.Drawing.Color getBackgroundColorForNode(ViewGraphNode node)
		{
			System.Drawing.Color bg = System.Drawing.Color.Black;
			if (node.ReteNode is TerminalNode)
				bg = System.Drawing.Color.Black;
			if (node.ReteNode is BaseJoin)
				bg = System.Drawing.Color.Green;
			if (node.ReteNode is LIANode)
				bg = System.Drawing.Color.Cyan;
			if (node.ReteNode is ObjectTypeNode)
				bg = System.Drawing.Color.Orange;
			if (node.ReteNode is AlphaNodePredConstr)
				bg = System.Drawing.Color.Red;
			if (node.ReteNode is BaseAlpha2)
				bg = System.Drawing.Color.Red;
			return bg;
		}
		
		protected internal virtual System.Drawing.Color getBorderColorForNode(ViewGraphNode node)
		{
			System.Drawing.Color temp_Color;
			temp_Color = getBackgroundColorForNode(node);
			return System.Drawing.Color.FromArgb(System.Convert.ToInt32(temp_Color.R * 0.7), System.Convert.ToInt32(temp_Color.G * 0.7), System.Convert.ToInt32(temp_Color.B * 0.7));
		}
		
		protected internal virtual void  addPrimitive(Shape p)
		{
			container.addPrimitive(p);
			radar.addPrimitive(p);
		}
		
		protected internal virtual void  addPrimitive(ConnectorLine p)
		{
			container.addPrimitive(p);
			radar.addPrimitive(p);
		}
		
		protected internal virtual void  getCorrespondingTerminalNodes(ViewGraphNode root, Set target)
		{
			BaseNode n = root.ReteNode;
			if (n is TerminalNode)
				target.add(n);
			Iterator it = root.childs.iterator();
			while (it.hasNext())
			{
				ViewGraphNode succ = (ViewGraphNode) it.next();
				getCorrespondingTerminalNodes(succ, target);
			}
		}
		
		
		protected internal virtual Shape makeShapeFromNode(ViewGraphNode act, LinkedList queue)
		{
			System.Drawing.Color bg = getBackgroundColorForNode(act);
			System.Drawing.Color border = getBorderColorForNode(act);
			System.String desc = "";
			BaseNode reteNode = act.ReteNode;
			HashSet terminalNodes = new HashSet();
			getCorrespondingTerminalNodes(act, terminalNodes);
			if (reteNode != null)
				desc = reteNode.NodeId.ToString();
			Shape s;
			if (reteNode == null)
			{
				// ROOT NODE
				s = new Ellipse();
			}
			else if (reteNode is BaseJoin || act.ReteNode is BaseAlpha2)
			{
				s = new Trapezoid();
			}
			else if (reteNode is TerminalNode)
			{
				s = new RoundedRectangle();
			}
			else if (reteNode is LIANode)
			{
				s = new Ellipse();
			}
			else
			{
				s = new Rectangle();
			}
			s.Bgcolor = bg;
			s.Bordercolor = border;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int x = (spaceHorizontal / 2) + (int) ((float) (act.X * (spaceHorizontal + nodeHorizontal)) / 2.0);
			int y = (spaceVertical / 2) + act.Y * (spaceVertical + nodeVertical);
			System.String key = x + "," + y;
			// if there is already a node at the given location, we shift it right
			while (this.coordinates.ContainsKey(key))
			{
				x = x + ((spaceHorizontal + nodeHorizontal) * 2);
				key = x + "," + y;
			}
			SupportClass.PutElement(coordinates, key, s);
			s.X = x;
			s.Y = y;
			s.Width = nodeHorizontal;
			s.Height = nodeVertical;
			System.String longdesc = "";
			if (reteNode == null)
			{
				longdesc = "Root Node";
			}
			else
			{
				longdesc = "ID:" + reteNode.NodeId + "  NodeType:" + reteNode.GetType().FullName;
				longdesc += "  Details:" + reteNode.toPPString();
			}
			longdesc += "  Rules:";
			Iterator iter = terminalNodes.iterator();
			while (iter.hasNext())
			{
				TerminalNode t = (TerminalNode) iter.next();
				
				longdesc += t.Rule.Name;
				if (iter.hasNext())
					longdesc += ";";
			}
			s.LongDescription = longdesc;
			if (reteNode is LIANode)
				s.incWidth(- nodeHorizontal / 3);
			s.Text = desc;
			act.Shape = s;
			addPrimitive(s);
			for (Iterator it = act.Successors.iterator(); it.hasNext(); )
			{
				ViewGraphNode n = (ViewGraphNode) it.next();
				queue.add(n);
			}
			return s;
		}
		
		protected internal virtual void  createPrimitives(ViewGraphNode root)
		{
			LinkedList queue = new LinkedList();
			queue.add(root);
			while (!queue.isEmpty())
			{
				ViewGraphNode act = (ViewGraphNode) queue.remove(0);
				Shape s = null;
				if (act.Shape == null)
				{
					s = makeShapeFromNode(act, queue);
				}
				else
				{
					s = act.Shape;
				}
				if (act.ParentsChecked)
					continue;
				act.ParentsChecked = true;
				for (Iterator it = act.Parents.iterator(); it.hasNext(); )
				{
					ViewGraphNode n = (ViewGraphNode) it.next();
					Shape s1 = n.Shape;
					if (s1 == null)
						s1 = makeShapeFromNode(n, queue);
					ConnectorLine line = new ConnectorLine(s1, s);
					line.Color = System.Drawing.Color.Blue;
					if (n.ReteNode is BaseJoin)
						line.Color = System.Drawing.Color.Red;
					addPrimitive(line);
				}
			}
		}
		
		/// <param name="engine">the rete-engine which should become visualised
		/// 
		/// </param>
		public Visualiser(Rete engine)
		{
			InitBlock();
			this.engine = engine;
			container = new JZoomableShapeContainer();
			container.addMouseListener(this);
			radar = new JMiniRadarShapeContainer();
			radar.MasterShapeContainer = container;
			radar.NormalizedFontHeight = nodeVertical;
			container.RadarShapeContainer = radar;
			calculateMainContainerFont();
			even = new SimpleAttributeSet();
			odd = new SimpleAttributeSet();
			StyleConstants.setForeground(even, System.Drawing.Color.Blue);
			System.Drawing.Color temp_Color;
			temp_Color = System.Drawing.Color.Green;
			StyleConstants.setForeground(odd, System.Drawing.Color.FromArgb(System.Convert.ToInt32(temp_Color.R * 0.7), System.Convert.ToInt32(temp_Color.G * 0.7), System.Convert.ToInt32(temp_Color.B * 0.7)));
			actAttributes = even;
			reloadView();
		}
		
		protected internal virtual void  calculateMainContainerFont()
		{
			int dpi = container.Toolkit.ScreenResolution;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			int ppMainContainer = (int) ((nodeVertical * container.ZoomFactor / dpi) * 72);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			container.Font = new System.Drawing.Font("SansSerif", ppMainContainer, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular);
		}
		
		
		/// <summary> Creates a window, embeds the visualiser in it
		/// and shows that window.
		/// </summary>
		public virtual void  show()
		{
			JFrame frame = new JFrame(getCaption(System.DateTime.Now));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			frame.ContentPane.add(VisualiserPanel, BorderLayout.CENTER);
			frame.pack();
			frame.setVisible(true);
			frame.setSize(700, 500);
			this.setMyFrame(frame);
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs arg0)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == zoomInButton)
			{
				container.zoomIn();
				calculateMainContainerFont();
				container.repaint();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == zoomOutButton)
				{
					container.zoomOut();
					calculateMainContainerFont();
					container.repaint();
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender == reloadButton)
					{
						reloadView();
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender == autoReloadButton)
						{
							if (autoReloadButton.isSelected())
							{
								reloadButton.setEnabled(false);
								engine.addEngineEventListener(this);
							}
							else
							{
								reloadButton.setEnabled(true);
								engine.removeEngineEventListener(this);
							}
						}
					}
				}
			}
		}
		
		protected internal virtual System.String getCaption(System.DateTime date)
		{
			return "Jamocha - Rete Network - " + SupportClass.FormatDateTime(SupportClass.GetDateTimeFormatInstance(2, 2, System.Globalization.CultureInfo.CurrentCulture), date);
		}
		
		protected internal virtual void  reloadView()
		{
			this.coordinates.Clear();
			RootNode root = engine.RootNode;
			ViewGraphNode t = ViewGraphNode.buildFromRete(root);
			this.root = t;
			container.removeAllPrimitives();
			radar.removeAllPrimitives();
			createPrimitives(t);
			if (myFrame != null)
			{
				myFrame.setTitle(getCaption(System.DateTime.Now));
			}
		}
		
		public virtual void  mouseClicked(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  mouseEntered(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  mouseExited(System.Object event_sender, System.EventArgs arg0)
		{
		}
		public virtual void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs arg0)
		{
		}
		public virtual void  eventOccurred(EngineEvent event_Renamed)
		{
		}
		
		public virtual void  mousePressed(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
		{
			Shape shape = container.getShapeAtPosition(event_Renamed.X, event_Renamed.Y);
			if (dumpEmpty)
			{
				dump.setText("");
				dumpEmpty = false;
			}
			if (shape == null)
				return ;
			try
			{
				dump.Document.insertString(dump.Document.Length, shape.LongDescription + "\n", actAttributes);
				if (actAttributes == even)
				{
					actAttributes = odd;
				}
				else
				{
					actAttributes = even;
				}
			}
			catch (BadLocationException e)
			{
				
			}
		}
	}
}