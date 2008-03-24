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
	using JCheckBox = javax.swing.JCheckBox;
	using JLabel = javax.swing.JLabel;
	using JPanel = javax.swing.JPanel;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	
	public class EngineSettingsPanel:AbstractSettingsPanel
	{
		
		private const long serialVersionUID = - 7136144663514250335L;
		
		private JCheckBox evaluationCheckBox;
		
		private JCheckBox profileAssertCheckBox;
		
		private JCheckBox profileRetractCheckBox;
		
		private JCheckBox profileFireCheckBox;
		
		private JCheckBox profileAddActivationCheckBox;
		
		private JCheckBox profileRemoveActivationCheckBox;
		
		private JCheckBox watchActivationsCheckBox;
		
		private JCheckBox watchFactsCheckBox;
		
		private JCheckBox watchRulesCheckBox;
		
		public EngineSettingsPanel(JamochaGui gui):base(gui)
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
			
			// Evaluation
			addLabel(this, new JLabel("Evaluation"), gridbag, c, 0);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel evaluationPanel = new JPanel(new BorderLayout());
			
			evaluationCheckBox = new JCheckBox();
			evaluationCheckBox.setEnabled(true);
			evaluationCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			evaluationPanel.add(evaluationCheckBox, BorderLayout.WEST);
			addInputComponent(this, evaluationPanel, gridbag, c, 0);
			
			// Profile Assert
			addLabel(this, new JLabel("Profile Assert:"), gridbag, c, 1);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel profileAssertPanel = new JPanel(new BorderLayout());
			
			profileAssertCheckBox = new JCheckBox();
			profileAssertCheckBox.setEnabled(true);
			profileAssertCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			profileAssertPanel.add(profileAssertCheckBox, BorderLayout.WEST);
			addInputComponent(this, profileAssertPanel, gridbag, c, 1);
			
			// Profile Retract
			addLabel(this, new JLabel("Profile Retract:"), gridbag, c, 2);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel profileRetractPanel = new JPanel(new BorderLayout());
			
			profileRetractCheckBox = new JCheckBox();
			profileRetractCheckBox.setEnabled(true);
			profileRetractCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			profileRetractPanel.add(profileRetractCheckBox, BorderLayout.WEST);
			addInputComponent(this, profileRetractPanel, gridbag, c, 2);
			
			// Profile Fire
			addLabel(this, new JLabel("Profile Fire:"), gridbag, c, 3);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel profileFirePanel = new JPanel(new BorderLayout());
			
			profileFireCheckBox = new JCheckBox();
			profileFireCheckBox.setEnabled(true);
			profileFireCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			profileFirePanel.add(profileFireCheckBox, BorderLayout.WEST);
			addInputComponent(this, profileFirePanel, gridbag, c, 3);
			
			// Profile Add Activation
			addLabel(this, new JLabel("Profile Add Activation:"), gridbag, c, 4);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel profileAddActivationPanel = new JPanel(new BorderLayout());
			
			profileAddActivationCheckBox = new JCheckBox();
			profileAddActivationCheckBox.setEnabled(true);
			profileAddActivationCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			profileAddActivationPanel.add(profileAddActivationCheckBox, BorderLayout.WEST);
			addInputComponent(this, profileAddActivationPanel, gridbag, c, 4);
			
			// Profile Remove Activation
			addLabel(this, new JLabel("Profile Remove Activation:"), gridbag, c, 5);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel profileRemoveActivationPanel = new JPanel(new BorderLayout());
			
			profileRemoveActivationCheckBox = new JCheckBox();
			profileRemoveActivationCheckBox.setEnabled(true);
			profileRemoveActivationCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			profileRemoveActivationPanel.add(profileRemoveActivationCheckBox, BorderLayout.WEST);
			addInputComponent(this, profileRemoveActivationPanel, gridbag, c, 5);
			
			// Activations
			addLabel(this, new JLabel(" Watch Activations:"), gridbag, c, 6);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel watchActivationsPanel = new JPanel(new BorderLayout());
			
			watchActivationsCheckBox = new JCheckBox();
			watchActivationsCheckBox.setEnabled(true);
			watchActivationsCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			watchActivationsPanel.add(watchActivationsCheckBox, BorderLayout.WEST);
			addInputComponent(this, watchActivationsPanel, gridbag, c, 6);
			
			// Facts 
			addLabel(this, new JLabel("Watch Facts:"), gridbag, c, 7);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel watchFactsPanel = new JPanel(new BorderLayout());
			
			watchFactsCheckBox = new JCheckBox();
			watchFactsCheckBox.setEnabled(true);
			watchFactsCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			watchFactsPanel.add(watchFactsCheckBox, BorderLayout.WEST);
			addInputComponent(this, watchFactsPanel, gridbag, c, 7);
			
			// Rules 
			addLabel(this, new JLabel("Watch Rules:"), gridbag, c, 8);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel watchRulesPanel = new JPanel(new BorderLayout());
			
			watchRulesCheckBox = new JCheckBox();
			watchRulesCheckBox.setEnabled(true);
			watchRulesCheckBox.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			watchRulesPanel.add(watchRulesCheckBox, BorderLayout.WEST);
			addInputComponent(this, watchRulesPanel, gridbag, c, 8);
		}
		
		public override void  save()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.evaluation", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.profileAssert", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.profileRetract", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.profileFire", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.profileAddActivation", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.profileRemoveActivation", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.watchActivations", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.watchFacts", false.ToString());
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			gui.Preferences.put("engine.watchRules", false.ToString());
		}
		
		
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			
			StringChannel guiStringChannel = gui.StringChannel;
			
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == evaluationCheckBox)
			{
				if (evaluationCheckBox.isSelected())
					guiStringChannel.executeCommand("(watch evaluation)");
				else
					guiStringChannel.executeCommand("(unwatch evaluation)");
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == profileAssertCheckBox)
				{
					if (profileAssertCheckBox.isSelected())
						guiStringChannel.executeCommand("(profile assert)");
					else
						guiStringChannel.executeCommand("(unprofile assert)");
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender == profileRetractCheckBox)
					{
						if (profileRetractCheckBox.isSelected())
							guiStringChannel.executeCommand("(profile retract)");
						else
							guiStringChannel.executeCommand("(unprofile retract)");
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender == profileFireCheckBox)
						{
							if (profileFireCheckBox.isSelected())
								guiStringChannel.executeCommand("(profile fire)");
							else
								guiStringChannel.executeCommand("(unprofile fire)");
						}
						else
						{
							//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
							if (event_sender == profileAddActivationCheckBox)
							{
								if (profileAddActivationCheckBox.isSelected())
									guiStringChannel.executeCommand("(profile add-activation)");
								else
									guiStringChannel.executeCommand("(unprofile add-activation)");
							}
							else
							{
								//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
								if (event_sender == profileRemoveActivationCheckBox)
								{
									if (profileRemoveActivationCheckBox.isSelected())
										guiStringChannel.executeCommand("(profile remove-activation)");
									else
										guiStringChannel.executeCommand("(unprofile remove-activation)");
								}
								else
								{
									//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
									if (event_sender == watchActivationsCheckBox)
									{
										if (profileRemoveActivationCheckBox.isSelected())
											guiStringChannel.executeCommand("(watch activations)");
										else
											guiStringChannel.executeCommand("(unwatch activations)");
									}
									else
									{
										//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
										if (event_sender == watchFactsCheckBox)
										{
											if (profileRemoveActivationCheckBox.isSelected())
												guiStringChannel.executeCommand("(watch facts)");
											else
												guiStringChannel.executeCommand("(unwatch facts)");
										}
										else
										{
											//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
											if (event_sender == watchRulesCheckBox)
											{
												if (profileRemoveActivationCheckBox.isSelected())
													guiStringChannel.executeCommand("(watch rules)");
												else
													guiStringChannel.executeCommand("(unwatch rules)");
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}