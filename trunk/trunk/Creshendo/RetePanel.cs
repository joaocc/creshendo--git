/// <summary> Copyright 2007 Karl-Heinz Krempels, Alexander Wilden
/// *
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// *
/// http://jamocha.sourceforge.net/
/// *
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// </summary>
namespace org.jamocha.gui.tab
{
	using System;
	using JPanel = javax.swing.JPanel;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using Visualiser = org.jamocha.rete.visualisation.Visualiser;
	/// <summary> This class integrates the Visualiser view into the Jamocha GUI.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class RetePanel:AbstractJamochaPanel
	{
		
		private const long serialVersionUID = - 651077761699385096L;
		
		/// <summary> The Visualiser Object.
		/// </summary>
		private Visualiser visualiser = null;
		
		/// <summary> The Panel containing the Visualiser.
		/// </summary>
		private JPanel visualiserPanel;
		
		/// <summary> The main constructor for a RetePanel.
		/// 
		/// </summary>
		/// <param name="">engine
		/// The Jamocha engine that should be used with this GUI.
		/// 
		/// </param>
		public RetePanel(JamochaGui gui):base(gui)
		{
			
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			
			visualiserPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.CardLayout.CardLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCardLayout"'
			visualiserPanel.setLayout(new CardLayout());
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(visualiserPanel, BorderLayout.CENTER);
			
			initVisualiser();
		}
		
		/// <summary> Sets the visualiser to null when closing the GUI.
		/// </summary>
		public override void  close()
		{
			visualiser = null;
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		/// <summary> Initializes the Visualiser with the current Rete-network.
		/// 
		/// </summary>
		private void  initVisualiser()
		{
			visualiser = new Visualiser(gui.Engine);
			JPanel panel = visualiser.VisualiserPanel;
			visualiserPanel.removeAll();
			visualiserPanel.add("view", panel);
			//UPGRADE_ISSUE: Class 'java.awt.CardLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCardLayout"'
			((CardLayout) visualiserPanel.Layout).last(visualiserPanel);
		}
	}
}