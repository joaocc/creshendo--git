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
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using BackingStoreException = java.util.prefs.BackingStoreException;
	using JButton = javax.swing.JButton;
	using JPanel = javax.swing.JPanel;
	using JTabbedPane = javax.swing.JTabbedPane;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using AbstractSettingsPanel = org.jamocha.gui.tab.settings.AbstractSettingsPanel;
	using EngineSettingsPanel = org.jamocha.gui.tab.settings.EngineSettingsPanel;
	using ShellSettingsPanel = org.jamocha.gui.tab.settings.ShellSettingsPanel;
	/// <summary> A Panel to change the settings of the jamocha rule engine or this gui.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class SettingsPanel:AbstractJamochaPanel
	{
		private void  InitBlock()
		{
			panels = new LinkedList();
		}
		
		private const long serialVersionUID = 1934727733895902279L;
		
		private JTabbedPane tabbedPane;
		
		private JButton saveButton;
		
		//UPGRADE_NOTE: The initialization of  'panels' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private List panels;
		
		public SettingsPanel(JamochaGui gui):base(gui)
		{
			InitBlock();
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			tabbedPane = new JTabbedPane();
			
			EngineSettingsPanel engineSettingsPanel = new EngineSettingsPanel(gui);
			tabbedPane.addTab("Engine", null, engineSettingsPanel, "Engine Settings");
			panels.add(engineSettingsPanel);
			
			ShellSettingsPanel shellSettingsPanel = new ShellSettingsPanel(gui);
			tabbedPane.addTab("Shell", null, shellSettingsPanel, "Shell Settings");
			panels.add(shellSettingsPanel);
			
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(tabbedPane, BorderLayout.CENTER);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			saveButton = new JButton("Save Changes", IconLoader.getImageIcon("disk"));
			saveButton.addActionListener(this);
			buttonPanel.add(saveButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.SOUTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(buttonPanel, BorderLayout.SOUTH);
		}
		
		public override void  close()
		{
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == saveButton)
			{
				for (int idx = 0; idx < panels.size(); idx++)
				{
					AbstractSettingsPanel panel = (AbstractSettingsPanel) panels.get(idx);
					panel.save();
				}
				gui.settingsChanged();
				try
				{
					gui.Preferences.flush();
				}
				catch (BackingStoreException e)
				{
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
}