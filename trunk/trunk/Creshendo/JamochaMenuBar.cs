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
	using JFileChooser = javax.swing.JFileChooser;
	using JMenu = javax.swing.JMenu;
	using JMenuBar = javax.swing.JMenuBar;
	using JMenuItem = javax.swing.JMenuItem;
	using JOptionPane = javax.swing.JOptionPane;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	/// <summary> This class provides the Mainmenubar for the whole gui.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class JamochaMenuBar:JMenuBar
	{
		
		private const long serialVersionUID = 2908247560107956066L;
		
		private JamochaGui gui;
		
		private JMenu fileMenu;
		
		private JMenuItem fileMenuBatch;
		
		private JMenuItem fileMenuCloseGui;
		
		private JMenuItem fileMenuQuit;
		
		public JamochaMenuBar(JamochaGui gui):base()
		{
			this.gui = gui;
			
			// adding the file menu
			fileMenu = new JMenu("File");
			fileMenuBatch = new JMenuItem("Batch File ...", IconLoader.getImageIcon("lorry"));
			fileMenuBatch.addActionListener(this);
			fileMenuCloseGui = new JMenuItem("Close Gui", IconLoader.getImageIcon("disconnect"));
			fileMenuCloseGui.addActionListener(this);
			fileMenuQuit = new JMenuItem("Quit", IconLoader.getImageIcon("door_in"));
			fileMenuQuit.addActionListener(this);
			fileMenu.add(fileMenuBatch);
			fileMenu.addSeparator();
			fileMenu.add(fileMenuCloseGui);
			fileMenu.add(fileMenuQuit);
			add(fileMenu);
		}
		
		public virtual void  showCloseGui(bool show)
		{
			fileMenuCloseGui.setVisible(show);
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == fileMenuQuit)
			{
				gui.ExitOnClose = true;
				gui.close();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == fileMenuCloseGui)
				{
					gui.ExitOnClose = false;
					gui.close();
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender == fileMenuBatch)
					{
						JFileChooser chooser = new JFileChooser(gui.Preferences.get("menubar.batchLastPath", ""));
						chooser.setMultiSelectionEnabled(false);
						if (chooser.showOpenDialog(this) == JFileChooser.APPROVE_OPTION)
						{
							System.IO.FileInfo file = chooser.SelectedFile;
							if (file != null && System.IO.File.Exists(file.FullName))
							{
								gui.Preferences.put("menubar.batchLastPath", file.FullName);
								gui.StringChannel.executeCommand("(batch " + file.FullName + ")");
								JOptionPane.showMessageDialog(this, "Batch process started.\nPlease check the log for Messages.\nThe process might be running in the background for a while.");
							}
						}
					}
				}
			}
		}
	}
}