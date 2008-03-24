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
namespace org.jamocha.gui
{
	using System;
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using BackingStoreException = java.util.prefs.BackingStoreException;
	using Preferences = java.util.prefs.Preferences;
	using JFrame = javax.swing.JFrame;
	using JLabel = javax.swing.JLabel;
	using JPanel = javax.swing.JPanel;
	using JTabbedPane = javax.swing.JTabbedPane;
	using ChangeEvent = javax.swing.event.ChangeEvent;
	using ChangeListener = javax.swing.event.ChangeListener;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using AbstractJamochaPanel = org.jamocha.gui.tab.AbstractJamochaPanel;
	using FactsPanel = org.jamocha.gui.tab.FactsPanel;
	using FunctionsPanel = org.jamocha.gui.tab.FunctionsPanel;
	using LogPanel = org.jamocha.gui.tab.LogPanel;
	using RetePanel = org.jamocha.gui.tab.RetePanel;
	using SettingsPanel = org.jamocha.gui.tab.SettingsPanel;
	using ShellPanel = org.jamocha.gui.tab.ShellPanel;
	using TemplatesPanel = org.jamocha.gui.tab.TemplatesPanel;
	using InterestType = org.jamocha.messagerouter.InterestType;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	using Rete = org.jamocha.rete.Rete;
	/// <summary> 
	/// JamochaGui implements a GUI for the Jamocha Rule Engine
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// </author>
	/// <version>  0.01
	/// 
	/// </version>
	public class JamochaGui:JFrame, ChangeListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassWindowAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassWindowAdapter
		{
			public AnonymousClassWindowAdapter(JamochaGui enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JamochaGui enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JamochaGui enclosingInstance;
			public JamochaGui Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
			{
				Enclosing_Instance.close();
			}
		}
		private void  InitBlock()
		{
			panels = new LinkedList();
		}
		/// <summary> This sets if only the Gui is closed on exit or also the engine. By
		/// default only the Gui will be closed.
		/// 
		/// </summary>
		/// <param name="">exitOnClose
		/// If false, only the gui will be closed
		/// 
		/// </param>
		virtual public bool ExitOnClose
		{
			set
			{
				this.exitOnClose = value;
				menuBar.showCloseGui(!value);
			}
			
		}
		/// <summary> Get the current Jamocha-engine.
		/// 
		/// </summary>
		/// <returns> The Jamocha-engine.
		/// 
		/// </returns>
		virtual public Rete Engine
		{
			get
			{
				return engine;
			}
			
		}
		virtual public StringChannel StringChannel
		{
			get
			{
				if (stringChannel == null)
					stringChannel = Engine.MessageRouter.openChannel("gui_string_channel", InterestType.NONE);
				return stringChannel;
			}
			
		}
		/// <summary> Returns the preferences for the JamochaGui
		/// 
		/// </summary>
		/// <returns> The Preferences-Node
		/// 
		/// </returns>
		virtual public Preferences Preferences
		{
			get
			{
				return preferences;
			}
			
		}
		
		internal const long serialVersionUID = 1L;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'preferences '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		//UPGRADE_NOTE: The initialization of  'preferences' was moved to static method 'org.jamocha.gui.JamochaGui'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		internal static readonly Preferences preferences;
		
		private Rete engine;
		
		private JamochaMenuBar menuBar;
		
		private JTabbedPane tabbedPane;
		
		//UPGRADE_NOTE: The initialization of  'panels' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private List panels;
		
		private bool exitOnClose = false;
		
		private StringChannel stringChannel;
		
		/// <summary> Create a GUI-Instance for Jamocha.
		/// 
		/// </summary>
		/// <param name="">engine
		/// The Jamocha-engine that will be used in the GUI.
		/// 
		/// </param>
		public JamochaGui(Rete engine)
		{
			InitBlock();
			
			// set up the frame
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			this.ContentPane.setLayout(new BorderLayout());
			this.setTitle("Jamocha");
			setSizeAndLocation();
			
			// show logo
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel logoPanel = new JPanel(new BorderLayout());
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.EAST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			logoPanel.add(new JLabel(IconLoader.getImageIcon("jamocha")), BorderLayout.EAST);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.NORTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			this.ContentPane.add(logoPanel, BorderLayout.NORTH);
			
			// create a tabbed pane
			tabbedPane = new JTabbedPane();
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			this.ContentPane.add(tabbedPane, BorderLayout.CENTER);
			
			// add MenuBar
			menuBar = new JamochaMenuBar(this);
			this.setJMenuBar(menuBar);
			
			// create a rete engine
			this.engine = engine;
			
			// create a shell tab and add it to the tabbed pane
			ShellPanel shellPanel = new ShellPanel(this);
			tabbedPane.addTab("Shell", IconLoader.getImageIcon("application_osx_terminal"), shellPanel, "Jamocha Shell");
			panels.add(shellPanel);
			FactsPanel factsPanel = new FactsPanel(this);
			tabbedPane.addTab("Facts", IconLoader.getImageIcon("database"), factsPanel, "View or modify Facts");
			panels.add(factsPanel);
			TemplatesPanel templatesPanel = new TemplatesPanel(this);
			tabbedPane.addTab("Templates", IconLoader.getImageIcon("brick"), templatesPanel, "View or modify Templates");
			panels.add(templatesPanel);
			FunctionsPanel functionsPanel = new FunctionsPanel(this);
			tabbedPane.addTab("Functions", IconLoader.getImageIcon("cog"), functionsPanel, "View Functions");
			panels.add(functionsPanel);
			RetePanel retePanel = new RetePanel(this);
			tabbedPane.addTab("Rete", IconLoader.getImageIcon("eye"), retePanel, "View the Rete-network");
			panels.add(retePanel);
			LogPanel logPanel = new LogPanel(this);
			tabbedPane.addTab("Log", IconLoader.getImageIcon("monitor"), logPanel, "View alle messages from or to the Rete-engine");
			panels.add(logPanel);
			SettingsPanel settingsPanel = new SettingsPanel(this);
			tabbedPane.addTab("Settings", IconLoader.getImageIcon("wrench"), settingsPanel, "Settings for Jamocha");
			panels.add(settingsPanel);
			
			// add the tab pane to the frame
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			this.ContentPane.add(tabbedPane, BorderLayout.CENTER);
			
			tabbedPane.addChangeListener(this);
			// add a listener to the frame to kill the engine when the GUI is closed
			addWindowListener(new AnonymousClassWindowAdapter(this));
		}
		
		private void  setSizeAndLocation()
		{
			// TODO Auto-generated method stub
			int width = preferences.getInt("gui.width", 0);
			int height = preferences.getInt("gui.height", 0);
			int locx = preferences.getInt("gui.locx", - 1);
			int locy = preferences.getInt("gui.locy", - 1);
			if (locx == - 1 || locy == - 1)
			{
				// this.setLocationByPlatform(true);
			}
			else
			{
				this.setLocation(locx, locy);
			}
			if (width <= 0 || height <= 0)
			{
				this.setSize(750, 550);
			}
			else
			{
				this.setSize(width, height);
			}
		}
		
		
		
		
		/// <summary> Sets the GUI visible and calls setFocus() on the shellPanel.
		/// 
		/// </summary>
		public virtual void  showGui()
		{
			setSize(new System.Drawing.Size(600, 400));
			setVisible(true);
			// panels.get(0).setFocus();
		}
		
		
		/// <summary> Informs all Panels, that some Settings might have changed.
		/// 
		/// </summary>
		public virtual void  settingsChanged()
		{
			for (int idx = 0; idx < panels.size(); idx++)
			{
				AbstractJamochaPanel panel = (AbstractJamochaPanel) panels.get(idx);
				panel.settingsChanged();
			}
		}
		
		/// <summary> Closes the GUI and informs all Panels.
		/// 
		/// </summary>
		public virtual void  close()
		{
			if (stringChannel != null)
				Engine.MessageRouter.closeChannel(stringChannel);
			// save position and size
			preferences.putInt("gui.width", Width);
			preferences.putInt("gui.height", Height);
			preferences.putInt("gui.locx", X);
			preferences.putInt("gui.locy", Y);
			
			// inform other panels
			for (int idx = 0; idx < panels.size(); idx++)
			{
				AbstractJamochaPanel panel = (AbstractJamochaPanel) panels.get(idx);
				panel.close();
			}
			try
			{
				preferences.flush();
			}
			catch (BackingStoreException e)
			{
				e.printStackTrace();
			}
			setVisible(false);
			dispose();
			if (exitOnClose)
			{
				engine.close();
				System.Environment.Exit(0);
			}
		}
		
		/// <summary> Calls setFocus() on the currently selected Component in the tabbedPane.
		/// </summary>
		public virtual void  stateChanged(ChangeEvent event_Renamed)
		{
			((AbstractJamochaPanel) tabbedPane.SelectedComponent).setFocus();
		}
		static JamochaGui()
		{
			preferences = Preferences.userRoot().node("org.jamocha.gui");
		}
	}
}