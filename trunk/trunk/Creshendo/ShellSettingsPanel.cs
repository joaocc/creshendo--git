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
namespace org.jamocha.gui.tab.settings
{
	using System;
	using GraphicsEnvironment = java.awt.GraphicsEnvironment;
	using DefaultListCellRenderer = javax.swing.DefaultListCellRenderer;
	using JButton = javax.swing.JButton;
	using JColorChooser = javax.swing.JColorChooser;
	using JComboBox = javax.swing.JComboBox;
	using JLabel = javax.swing.JLabel;
	using JList = javax.swing.JList;
	using JPanel = javax.swing.JPanel;
	using JTextField = javax.swing.JTextField;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	
	public class ShellSettingsPanel:AbstractSettingsPanel
	{
		
		private const long serialVersionUID = - 7136144663514250335L;
		
		private JComboBox fonts;
		
		private JComboBox fontsizes;
		
		private JButton fontColorChooserButton;
		
		private JTextField fontColorChooserPreview;
		
		private JButton backgroundColorChooserButton;
		
		private JTextField backgroundColorChooserPreview;
		
		public ShellSettingsPanel(JamochaGui gui):base(gui)
		{
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			GridBagLayout gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			GridBagConstraints c = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.weightx = 1.0;
			setLayout(gridbag);
			
			// Font
			addLabel(this, new JLabel("Font:"), gridbag, c, 0);
			GraphicsEnvironment ge = GraphicsEnvironment.LocalGraphicsEnvironment;
			System.Drawing.Font[] allFonts = ge.AllFonts;
			fonts = new JComboBox(allFonts);
			System.Drawing.Font selFont = null;
			System.String selFontName = gui.Preferences.get("shell.font", "Courier");
			for (int idx = 0; idx < allFonts.Length; idx++)
			{
				System.Drawing.Font curFont = allFonts[idx];
				if (curFont.FontName.equals(selFontName))
				{
					selFont = curFont;
					break;
				}
			}
			if (selFont != null)
			{
				fonts.setSelectedItem(selFont);
			}
			fonts.setRenderer(new FontListCellRenderer(this));
			addInputComponent(this, fonts, gridbag, c, 0);
			
			// Fontsize
			addLabel(this, new JLabel("Fontsize:"), gridbag, c, 1);
			System.Int32[] sizes = new System.Int32[17];
			for (int i = 0; i < sizes.Length; ++i)
			{
				sizes[i] = 8 + i;
			}
			fontsizes = new JComboBox(sizes);
			//fontsizes.setSelectedItem(gui.getPreferences().getInt("shell.fontsize",
			//		12));
			addInputComponent(this, fontsizes, gridbag, c, 1);
			
			// Fontcolor
			addLabel(this, new JLabel("Fontcolor:"), gridbag, c, 2);
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			JPanel fontColorChooserPanel = new JPanel(new FlowLayout());
			fontColorChooserPreview = new JTextField(5);
			fontColorChooserPreview.setEditable(false);
			fontColorChooserPreview.setBackground(new Color(gui.Preferences.getInt("shell.fontcolor", System.Drawing.Color.WHITE.RGB)));
			fontColorChooserButton = new JButton("Choose Color", IconLoader.getImageIcon("color_swatch"));
			fontColorChooserButton.addActionListener(this);
			fontColorChooserPanel.add(fontColorChooserPreview);
			fontColorChooserPanel.add(fontColorChooserButton);
			addInputComponent(this, fontColorChooserPanel, gridbag, c, 2);
			
			// Backgroundcolor
			addLabel(this, new JLabel("Backgroundcolor:"), gridbag, c, 3);
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			JPanel backgroundColorChooserPanel = new JPanel(new FlowLayout());
			backgroundColorChooserPreview = new JTextField(5);
			backgroundColorChooserPreview.setEditable(false);
			backgroundColorChooserPreview.setBackground(new Color(gui.Preferences.getInt("shell.backgroundcolor", System.Drawing.Color.BLACK.RGB)));
			backgroundColorChooserButton = new JButton("Choose Color", IconLoader.getImageIcon("color_swatch"));
			backgroundColorChooserButton.addActionListener(this);
			backgroundColorChooserPanel.add(backgroundColorChooserPreview);
			backgroundColorChooserPanel.add(backgroundColorChooserButton);
			addInputComponent(this, backgroundColorChooserPanel, gridbag, c, 3);
		}
		
		public override void  save()
		{
			gui.Preferences.put("shell.font", ((System.Drawing.Font) fonts.SelectedItem).FontName);
			//gui.getPreferences().putInt("shell.fontsize",
			//		(Integer) fontsizes.getSelectedItem());
			//gui.getPreferences().putInt("shell.fontcolor",
			//		(Integer) fontColorChooserPreview.getBackground().getRGB());
			//gui.getPreferences().putInt("shell.backgroundcolor",
			//		(Integer) backgroundColorChooserPreview.getBackground().getRGB());
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FontListCellRenderer' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class FontListCellRenderer:DefaultListCellRenderer
		{
			private void  InitBlock(ShellSettingsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellSettingsPanel enclosingInstance;
			public ShellSettingsPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			public FontListCellRenderer(ShellSettingsPanel enclosingInstance):base()
			{
				InitBlock(enclosingInstance);
			}
			
			public virtual System.Windows.Forms.Control getListCellRendererComponent(JList list, System.Object value_Renamed, int index, bool isSelected, bool cellHasFocus)
			{
				base.getListCellRendererComponent(list, value_Renamed, index, isSelected, cellHasFocus);
				setFont(((System.Drawing.Font) value_Renamed).deriveFont(12.0f));
				setText(((System.Drawing.Font) value_Renamed).FontName);
				return this;
			}
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == fontColorChooserButton)
			{
				System.Drawing.Color newColor = JColorChooser.showDialog(this, "Choose a Fontcolor", new Color(gui.Preferences.getInt("shell.fontcolor", System.Drawing.Color.WHITE.RGB)));
				if (newColor != null)
				{
					fontColorChooserPreview.setBackground(newColor);
				}
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == backgroundColorChooserButton)
				{
					System.Drawing.Color newColor = JColorChooser.showDialog(this, "Choose a Backgroundcolor", new Color(gui.Preferences.getInt("shell.backgroundcolor", System.Drawing.Color.BLACK.RGB)));
					if (newColor != null)
					{
						backgroundColorChooserPreview.setBackground(newColor);
					}
				}
			}
		}
	}
}