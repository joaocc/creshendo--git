namespace org.jamocha.gui.tab.settings
{
	using System;
	using JComponent = javax.swing.JComponent;
	using JLabel = javax.swing.JLabel;
	using JPanel = javax.swing.JPanel;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	
	public abstract class AbstractSettingsPanel:JPanel
	{
		
		protected internal JamochaGui gui;
		
		public AbstractSettingsPanel(JamochaGui gui)
		{
			this.gui = gui;
		}
		
		public abstract void  save();
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		public virtual void  addLabel(JPanel parent, JLabel label, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.VERTICAL' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.VERTICAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.EAST;
			gridbag.setConstraints(label, c);
			parent.add(label);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		public virtual void  addInputComponent(JPanel parent, JComponent comp, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.WEST;
			gridbag.setConstraints(comp, c);
			parent.add(comp);
		}
	}
}