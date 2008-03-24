namespace org.jamocha.gui.editor
{
	using System;
	using Collection = java.util.Collection;
	using HashMap = java.util.HashMap;
	using Iterator = java.util.Iterator;
	using Map = java.util.Map;
	using BorderFactory = javax.swing.BorderFactory;
	using BoxLayout = javax.swing.BoxLayout;
	using DefaultListModel = javax.swing.DefaultListModel;
	using JButton = javax.swing.JButton;
	using JComboBox = javax.swing.JComboBox;
	using JComponent = javax.swing.JComponent;
	using JLabel = javax.swing.JLabel;
	using JList = javax.swing.JList;
	using JMenuItem = javax.swing.JMenuItem;
	using JOptionPane = javax.swing.JOptionPane;
	using JPanel = javax.swing.JPanel;
	using JPopupMenu = javax.swing.JPopupMenu;
	using JScrollPane = javax.swing.JScrollPane;
	using JTextArea = javax.swing.JTextArea;
	using JTextField = javax.swing.JTextField;
	using ListSelectionModel = javax.swing.ListSelectionModel;
	using SwingConstants = javax.swing.SwingConstants;
	using ListSelectionEvent = javax.swing.event.ListSelectionEvent;
	using ListSelectionListener = javax.swing.event.ListSelectionListener;
	using PopupMenuEvent = javax.swing.event.PopupMenuEvent;
	using PopupMenuListener = javax.swing.event.PopupMenuListener;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using MessageEvent = org.jamocha.messagerouter.MessageEvent;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	using Constants = org.jamocha.rete.Constants;
	using Function = org.jamocha.rete.Function;
	using Module = org.jamocha.rete.Module;
	using MultiSlot = org.jamocha.rete.MultiSlot;
	using Rete = org.jamocha.rete.Rete;
	using Slot = org.jamocha.rete.Slot;
	using Template = org.jamocha.rete.Template;
	
	public class FactEditor:AbstractJamochaEditor, ListSelectionListener
	{
		private void  InitBlock()
		{
			dumpAreaTemplate = new JTextArea();
			dumpAreaFact = new JTextArea();
			moduleListModel = new DefaultListModel();
			templateListModel = new DefaultListModel();
			factComponents = new HashMap();
		}
		virtual public StringChannel StringChannel
		{
			set
			{
				this.channel = value;
			}
			
		}
		
		private const long serialVersionUID = 6037731034903564707L;
		
		private int step = 0;
		
		private JPanel contentPanel;
		
		private JButton cancelButton;
		
		private JButton assertButton;
		
		private JButton backButton;
		
		private JButton nextButton;
		
		private JButton reloadButtondumpAreaFact;
		
		private JList moduleList;
		
		private JList templateList;
		
		//UPGRADE_NOTE: The initialization of  'dumpAreaTemplate' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private JTextArea dumpAreaTemplate;
		
		//UPGRADE_NOTE: The initialization of  'dumpAreaFact' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private JTextArea dumpAreaFact;
		
		//UPGRADE_NOTE: The initialization of  'moduleListModel' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private DefaultListModel moduleListModel;
		
		//UPGRADE_NOTE: The initialization of  'templateListModel' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private DefaultListModel templateListModel;
		
		private StringChannel channel;
		
		//UPGRADE_NOTE: The initialization of  'factComponents' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private Map factComponents;
		
		public FactEditor(Rete engine):base(engine)
		{
			InitBlock();
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			setTitle("Assert new Fact");
			//UPGRADE_ISSUE: Constructor 'java.awt.CardLayout.CardLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCardLayout"'
			contentPanel = new JPanel(new CardLayout());
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(contentPanel, BorderLayout.CENTER);
			cancelButton = new JButton("Cancel", IconLoader.getImageIcon("cancel"));
			cancelButton.addActionListener(this);
			assertButton = new JButton("Assert Fact", IconLoader.getImageIcon("database_add"));
			assertButton.addActionListener(this);
			assertButton.setVisible(false);
			backButton = new JButton("Back", IconLoader.getImageIcon("resultset_previous"));
			backButton.addActionListener(this);
			backButton.setVisible(false);
			nextButton = new JButton("Next", IconLoader.getImageIcon("resultset_next"));
			nextButton.setHorizontalTextPosition(SwingConstants.LEFT);
			nextButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.CENTER, 5, 1));
			buttonPanel.add(cancelButton);
			buttonPanel.add(backButton);
			buttonPanel.add(nextButton);
			buttonPanel.add(assertButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(buttonPanel, BorderLayout.PAGE_END);
			
			dumpAreaTemplate.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpAreaTemplate.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			dumpAreaTemplate.setRows(5);
			dumpAreaFact.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpAreaFact.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			dumpAreaFact.setRows(5);
		}
		
		
		public override void  init()
		{
			// initialize the Panels
			initPreselectionPanel();
			initFactEditPanel();
			
			showCurrentStep();
			setVisible(true);
		}
		
		private void  initPreselectionPanel()
		{
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			GridBagLayout gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			GridBagConstraints c = new GridBagConstraints();
			JPanel preselectionPanel = new JPanel(gridbag);
			preselectionPanel.setBorder(BorderFactory.createTitledBorder("Module and Template Selection"));
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.BOTH;
			moduleList = new JList(moduleListModel);
			moduleList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
			moduleList.SelectionModel.addListSelectionListener(this);
			Collection modules = engine.WorkingMemory.Modules;
			Iterator itr = modules.iterator();
			while (itr.hasNext())
			{
				Module mod = (Module) itr.next();
				moduleListModel.addElement(mod.ModuleName);
			}
			JPanel modulePanel = new JPanel();
			modulePanel.setLayout(new BoxLayout(modulePanel, BoxLayout.Y_AXIS));
			modulePanel.add(new JLabel("Select a Module:"));
			modulePanel.add(new JScrollPane(moduleList));
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.weightx = 0.5;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = c.gridy = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.weighty = 1.0;
			// c.gridwidth = GridBagConstraints.RELATIVE;
			gridbag.setConstraints(modulePanel, c);
			preselectionPanel.add(modulePanel);
			templateList = new JList(templateListModel);
			templateList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
			templateList.SelectionModel.addListSelectionListener(this);
			initTemplateList();
			JPanel templatePanel = new JPanel();
			templatePanel.setLayout(new BoxLayout(templatePanel, BoxLayout.Y_AXIS));
			templatePanel.add(new JLabel("Select a Template:"));
			templatePanel.add(new JScrollPane(templateList));
			// c.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 1;
			gridbag.setConstraints(templatePanel, c);
			preselectionPanel.add(templatePanel);
			JPanel dumpAreaPanel = new JPanel();
			dumpAreaPanel.setLayout(new BoxLayout(dumpAreaPanel, BoxLayout.Y_AXIS));
			dumpAreaPanel.add(new JLabel("Template Definition:"));
			dumpAreaPanel.add(new JScrollPane(dumpAreaTemplate));
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.weightx = 0.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridwidth = 2;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.BOTH;
			gridbag.setConstraints(dumpAreaPanel, c);
			preselectionPanel.add(dumpAreaPanel);
			contentPanel.add(preselectionPanel, "preselection");
		}
		
		private void  initTemplateList()
		{
			templateListModel.clear();
			Module module = engine.WorkingMemory.findModule(System.String.valueOf(moduleList.SelectedValue));
			if (module != null)
			{
				Collection templates = module.Templates;
				Iterator itr = templates.iterator();
				while (itr.hasNext())
				{
					System.Object obj = itr.next();
					Template tmp = (Template) obj;
					if (!module.ModuleName.Equals("MAIN") || !tmp.Name.Equals("_initialFact"))
					{
						templateListModel.addElement(tmp.Name);
					}
				}
			}
		}
		
		private void  initFactEditPanel()
		{
			factComponents.clear();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			GridBagLayout gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			GridBagConstraints c = new GridBagConstraints();
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			JPanel factEditPanel = new JPanel(new BorderLayout());
			JPanel innerPanel = new JPanel(gridbag);
			factEditPanel.setBorder(BorderFactory.createTitledBorder("Set the Slots for the Fact"));
			if (templateList.SelectedIndex > - 1)
			{
				Module module = engine.WorkingMemory.findModule(System.String.valueOf(moduleList.SelectedValue));
				
				Template tmp = module.getTemplate(System.String.valueOf(templateList.SelectedValue));
				
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
				c.weightx = 1.0;
				Slot[] slots = tmp.AllSlots;
				for (int i = 0; i < slots.Length; ++i)
				{
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.gridx = 0;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.gridy = i;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.VERTICAL' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.fill = GridBagConstraints.VERTICAL;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.anchor = GridBagConstraints.EAST;
					JLabel label = new JLabel(slots[i].Name + ": ");
					gridbag.setConstraints(label, c);
					innerPanel.add(label);
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.gridx = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.fill = GridBagConstraints.BOTH;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
					c.anchor = GridBagConstraints.WEST;
					if (slots[i] is MultiSlot)
					{
						MultiSlotEditor multislotEditor = new MultiSlotEditor(this);
						JScrollPane scrollPane = new JScrollPane(multislotEditor.List);
						gridbag.setConstraints(scrollPane, c);
						innerPanel.add(scrollPane);
						factComponents.put(slots[i], multislotEditor.List);
					}
					else if (slots[i].ValueType == Constants.FACT_TYPE)
					{
						// TODO Fact-Selector
						
						JComboBox factBox = new JComboBox();
						factComponents.put(slots[i], factBox);
					}
					else
					{
						JTextField textField = new JTextField();
						gridbag.setConstraints(textField, c);
						innerPanel.add(textField);
						factComponents.put(slots[i], textField);
					}
				}
			}
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			factEditPanel.add(new JScrollPane(innerPanel), BorderLayout.CENTER);
			JPanel dumpAreaPanel = new JPanel();
			dumpAreaPanel.setLayout(new BoxLayout(dumpAreaPanel, BoxLayout.Y_AXIS));
			dumpAreaPanel.add(new JLabel("Fact Preview:"));
			dumpAreaPanel.add(new JScrollPane(dumpAreaFact));
			reloadButtondumpAreaFact = new JButton("Reload Fact Preview", IconLoader.getImageIcon("arrow_refresh"));
			reloadButtondumpAreaFact.addActionListener(this);
			dumpAreaPanel.add(reloadButtondumpAreaFact);
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.weightx = 0.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridwidth = 2;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.BOTH;
			gridbag.setConstraints(dumpAreaPanel, c);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.SOUTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			factEditPanel.add(dumpAreaPanel, BorderLayout.SOUTH);
			contentPanel.add("factEdit", factEditPanel);
		}
		
		private void  showCurrentStep()
		{
			switch (step)
			{
				
				case 0: 
					//UPGRADE_ISSUE: Class 'java.awt.CardLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCardLayout"'
					((CardLayout) contentPanel.Layout).show(contentPanel, "preselection");
					if (templateList.SelectedIndex > - 1)
					{
						nextButton.setEnabled(true);
					}
					else
					{
						nextButton.setEnabled(false);
					}
					nextButton.setVisible(true);
					backButton.setVisible(false);
					assertButton.setVisible(false);
					nextButton.requestFocus();
					break;
				
				case 1: 
					initFactEditPanel();
					//UPGRADE_ISSUE: Class 'java.awt.CardLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCardLayout"'
					((CardLayout) contentPanel.Layout).show(contentPanel, "factEdit");
					nextButton.setVisible(false);
					backButton.setVisible(true);
					assertButton.setVisible(true);
					assertButton.requestFocus();
					break;
				}
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == assertButton)
			{
				channel.executeCommand(getCurrentFactAssertionString(false));
				JOptionPane.showMessageDialog(this, "Assertion done.\nPlease check the log for Messages.");
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == backButton)
				{
					if (step > 0)
					{
						step--;
						showCurrentStep();
					}
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender == nextButton)
					{
						if (step < 1)
						{
							step++;
							showCurrentStep();
						}
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender == cancelButton)
						{
							close();
						}
						else
						{
							//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
							if (event_sender == reloadButtondumpAreaFact)
							{
								dumpAreaFact.setText(getCurrentFactAssertionString(true));
							}
						}
					}
				}
			}
		}
		
		public virtual void  valueChanged(ListSelectionEvent event_Renamed)
		{
			if (event_Renamed.Source == moduleList.SelectionModel)
			{
				initTemplateList();
			}
			else if (event_Renamed.Source == templateList.SelectionModel)
			{
				if (templateList.SelectedIndex > - 1)
				{
					nextButton.setEnabled(true);
					Module module = engine.WorkingMemory.findModule(System.String.valueOf(moduleList.SelectedValue));
					Template tmp = module.getTemplate(System.String.valueOf(templateList.SelectedValue));
					
					dumpAreaTemplate.setText("(deftemplate " + tmp.Name + "\n");
					Slot[] slots = tmp.AllSlots;
					for (int idx = 0; idx < slots.Length; idx++)
					{
						Slot slot = slots[idx];
						dumpAreaTemplate.append("    (");
						if (slot is MultiSlot)
						{
							dumpAreaTemplate.append("multislot");
						}
						else
						{
							dumpAreaTemplate.append("slot");
						}
						dumpAreaTemplate.append(" " + slot.Name + ")\n");
					}
					dumpAreaTemplate.append(")");
				}
				else
				{
					nextButton.setEnabled(false);
				}
			}
		}
		
		private System.String getCurrentFactAssertionString(bool print)
		{
			Module module = engine.WorkingMemory.findModule(System.String.valueOf(moduleList.SelectedValue));
			Template tmp = module.getTemplate(System.String.valueOf(templateList.SelectedValue));
			System.Text.StringBuilder res = new System.Text.StringBuilder("(assert (" + tmp.Name);
			JComponent currComponent;
			Iterator kitr = factComponents.keySet().iterator();
			while (kitr.hasNext())
			{
				Slot slot = (Slot) kitr.next();
				currComponent = (JComponent) factComponents.get(slot);
				if (print)
					res.Append("\n\t");
				res.Append("(" + slot.Name + " ");
				if (slot is MultiSlot)
				{
					System.Object[] values = ((DefaultListModel) ((JList) currComponent).Model).toArray();
					for (int i = 0; i < values.Length; ++i)
					{
						if (i > 0)
							res.Append(" ");
						//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
						res.Append("\"" + values[i].ToString() + "\"");
					}
				}
				else if (slot.ValueType == Constants.FACT_TYPE)
				{
					// TODO Fact-Selector
				}
				else
				{
					res.Append("\"" + ((JTextField) currComponent).Text + "\"");
				}
				res.Append(")");
			}
			if (print)
				res.Append("\n");
			res.Append("))");
			return res.ToString();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MultiSlotEditor' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class MultiSlotEditor : PopupMenuListener
		{
			private void  InitBlock(FactEditor enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				listModel = new DefaultListModel();
			}
			private FactEditor enclosingInstance;
			private JList List
			{
				get
				{
					return list;
				}
				
			}
			public FactEditor Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private JList list;
			
			//UPGRADE_NOTE: The initialization of  'listModel' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private DefaultListModel listModel;
			
			private JPopupMenu popupMenu;
			
			private JMenuItem addMenuItem;
			
			private JMenuItem editMenuItem;
			
			private JMenuItem deleteMenuItem;
			
			private MultiSlotEditor(FactEditor enclosingInstance)
			{
				InitBlock(enclosingInstance);
				popupMenu = new JPopupMenu();
				addMenuItem = new JMenuItem("add value", IconLoader.getImageIcon("add"));
				addMenuItem.addActionListener(this);
				editMenuItem = new JMenuItem("edit value", IconLoader.getImageIcon("pencil"));
				editMenuItem.addActionListener(this);
				deleteMenuItem = new JMenuItem("remove value", IconLoader.getImageIcon("delete"));
				deleteMenuItem.addActionListener(this);
				popupMenu.add(addMenuItem);
				popupMenu.add(editMenuItem);
				popupMenu.add(deleteMenuItem);
				popupMenu.addPopupMenuListener(this);
				list = new JList(listModel);
				list.setVisibleRowCount(4);
				//list.setComponentPopupMenu(popupMenu);
			}
			
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender == addMenuItem)
				{
					System.String value_Renamed = JOptionPane.showInputDialog("Enter the value:");
					listModel.addElement(value_Renamed);
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender == editMenuItem)
					{
						System.String value_Renamed = JOptionPane.showInputDialog("Enter the value:", list.SelectedValue);
						listModel.set(list.SelectedIndex, value_Renamed);
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender == deleteMenuItem)
						{
							int[] indices = list.SelectedIndices;
							// run backwards to delete the right indices
							for (int i = indices.Length - 1; i >= 0; --i)
							{
								listModel.remove(indices[i]);
							}
						}
					}
				}
			}
			
			public void  popupMenuCanceled(PopupMenuEvent arg0)
			{
				
			}
			
			public void  popupMenuWillBecomeInvisible(PopupMenuEvent arg0)
			{
				
			}
			
			public void  popupMenuWillBecomeVisible(PopupMenuEvent arg0)
			{
				addMenuItem.setVisible(true);
				if (list.SelectedIndices.length > 1)
				{
					editMenuItem.setVisible(false);
					deleteMenuItem.setVisible(true);
				}
				else if (list.SelectedIndices.length == 1)
				{
					editMenuItem.setVisible(true);
					deleteMenuItem.setVisible(true);
				}
				else
				{
					editMenuItem.setVisible(false);
					deleteMenuItem.setVisible(false);
				}
			}
		}
	}
}