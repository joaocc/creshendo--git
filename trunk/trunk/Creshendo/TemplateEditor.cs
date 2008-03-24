namespace org.jamocha.gui.editor
{
	using System;
	
	
	
	
	using BorderFactory = javax.swing.BorderFactory;
	using BoxLayout = javax.swing.BoxLayout;
	using ImageIcon = javax.swing.ImageIcon;
	using JButton = javax.swing.JButton;
	using JComboBox = javax.swing.JComboBox;
	using JLabel = javax.swing.JLabel;
	using JOptionPane = javax.swing.JOptionPane;
	using JPanel = javax.swing.JPanel;
	using JScrollPane = javax.swing.JScrollPane;
	using JTextArea = javax.swing.JTextArea;
	using JTextField = javax.swing.JTextField;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	using Constants = org.jamocha.rete.Constants;
	using Module = org.jamocha.rete.Module;
	using Rete = org.jamocha.rete.Rete;
	
	public class TemplateEditor:AbstractJamochaEditor
	{
		private void  InitBlock()
		{
			dumpAreaTemplate = new JTextArea();
			rows = new LinkedList();
		}
		virtual public StringChannel StringChannel
		{
			set
			{
				this.channel = value;
			}
			
		}
		private JComboBox NewTypesCombo
		{
			get
			{
				System.String[] types = new System.String[]{"STRING", "LONG", "DOUBLE", "OBJECT", "MULTISLOT"};
				JComboBox box = new JComboBox(types);
				return box;
			}
			
		}
		
		private const long serialVersionUID = 6037731034903564707L;
		
		private JPanel contentPanel;
		
		private JPanel templatePanel;
		
		private JButton addSlotButton;
		
		private JButton cancelButton;
		
		private JButton assertButton;
		
		private JTextField nameField;
		
		private JComboBox moduleBox;
		
		private JButton reloadButtonDumpAreaTemplate;
		
		//UPGRADE_NOTE: The initialization of  'dumpAreaTemplate' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private JTextArea dumpAreaTemplate;
		
		private StringChannel channel;
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		private GridBagLayout gridbag;
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		private GridBagConstraints gridbagConstraints;
		
		//UPGRADE_NOTE: The initialization of  'rows' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private List rows;
		
		public TemplateEditor(Rete engine):base(engine)
		{
			InitBlock();
			setSize(600, 500);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			setTitle("Create new Template");
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			contentPanel = new JPanel(new BorderLayout());
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(contentPanel, BorderLayout.CENTER);
			cancelButton = new JButton("Cancel", IconLoader.getImageIcon("cancel"));
			cancelButton.addActionListener(this);
			assertButton = new JButton("Create Template", IconLoader.getImageIcon("brick_add"));
			assertButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.CENTER, 5, 1));
			buttonPanel.add(cancelButton);
			buttonPanel.add(assertButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.SOUTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(buttonPanel, BorderLayout.SOUTH);
			
			nameField = new JTextField(15);
			Collection modules = engine.WorkingMemory.Modules;
			System.String[] moduleNames = new System.String[modules.size()];
			int i = 0;
			Iterator itr = modules.iterator();
			while (itr.hasNext())
			{
				System.Object obj = itr.next();
				moduleNames[i++] = ((Module) obj).ModuleName;
			}
			moduleBox = new JComboBox(moduleNames);
			
			
			addSlotButton = new JButton("Add Slot", IconLoader.getImageIcon("add"));
			addSlotButton.addActionListener(this);
			//UPGRADE_ISSUE: Field 'java.awt.Component.RIGHT_ALIGNMENT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtComponentRIGHT_ALIGNMENT_f"'
			addSlotButton.setAlignmentX(Component.RIGHT_ALIGNMENT);
			
			JPanel topPanel = new JPanel();
			topPanel.setBorder(BorderFactory.createTitledBorder("General Template Settings"));
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.LEFT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			topPanel.setLayout(new FlowLayout(FlowLayout.LEFT, 20, 1));
			//UPGRADE_ISSUE: Constructor 'java.awt.GridLayout.GridLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridLayout"'
			JPanel innerTopPanel = new JPanel(new GridLayout(2, 2));
			innerTopPanel.add(new JLabel("Template-name:"));
			innerTopPanel.add(nameField);
			innerTopPanel.add(new JLabel("Template-Module:"));
			innerTopPanel.add(moduleBox);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			topPanel.add(innerTopPanel, BorderLayout.WEST);
			topPanel.add(addSlotButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.NORTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(topPanel, BorderLayout.NORTH);
			
			dumpAreaTemplate.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpAreaTemplate.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			dumpAreaTemplate.setRows(5);
			
			JPanel dumpAreaPanel = new JPanel();
			dumpAreaPanel.setLayout(new BoxLayout(dumpAreaPanel, BoxLayout.Y_AXIS));
			dumpAreaPanel.setBorder(BorderFactory.createTitledBorder("Template Preview"));
			dumpAreaPanel.add(new JScrollPane(dumpAreaTemplate));
			reloadButtonDumpAreaTemplate = new JButton("Reload Template Preview", IconLoader.getImageIcon("arrow_refresh"));
			reloadButtonDumpAreaTemplate.addActionListener(this);
			dumpAreaPanel.add(reloadButtonDumpAreaTemplate);
			
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.SOUTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			contentPanel.add(dumpAreaPanel, BorderLayout.SOUTH);
		}
		
		
		public override void  init()
		{
			// initialize the Panels
			templatePanel = new JPanel();
			templatePanel.setBorder(BorderFactory.createTitledBorder("Set the Slots for the Template"));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			contentPanel.add(new JScrollPane(templatePanel), BorderLayout.CENTER);
			initTemplatePanel();
			setVisible(true);
		}
		
		private void  initTemplatePanel()
		{
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
			gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.weightx = 1.0;
			templatePanel.setLayout(gridbag);
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.anchor = GridBagConstraints.WEST;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.gridy = 0;
			templatePanel.add(new JLabel());
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.gridx = 1;
			templatePanel.add(new JLabel());
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.gridy = 2;
			templatePanel.add(new JLabel("Type:"));
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			gridbagConstraints.gridy = 3;
			templatePanel.add(new JLabel("Name:"));
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == assertButton)
			{
				channel.executeCommand(getCurrentDeftemplateString(false));
				JOptionPane.showMessageDialog(this, "Template created.\nPlease check the log for Messages.");
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
					if (event_sender == reloadButtonDumpAreaTemplate)
					{
						dumpAreaTemplate.setText(getCurrentDeftemplateString(true));
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender == addSlotButton)
						{
							EditorRow row = new EditorRow(new DeleteButton(IconLoader.getImageIcon("delete"), rows.size()), new JLabel("Slot " + (rows.size() + 1)), NewTypesCombo, new JTextField());
							row.deleteButton.addActionListener(this);
							addRemoveButton(templatePanel, row.deleteButton, gridbag, gridbagConstraints, (rows.size() + 1));
							addLabel(templatePanel, row.rowLabel, gridbag, gridbagConstraints, (rows.size() + 1));
							addTypesCombo(templatePanel, row.typeBox, gridbag, gridbagConstraints, (rows.size() + 1));
							addNameField(templatePanel, row.nameField, gridbag, gridbagConstraints, (rows.size() + 1));
							rows.add(row);
							templatePanel.revalidate();
						}
						else
						{
							//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
							if (event_sender is DeleteButton)
							{
								//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
								DeleteButton deleteButton = (DeleteButton) event_sender;
								rows.remove(deleteButton.Row);
								templatePanel.removeAll();
								initTemplatePanel();
								for (int i = 0; i < rows.size(); ++i)
								{
									EditorRow editorRow = (EditorRow) rows.get(i);
									editorRow.deleteButton.Row = i;
									editorRow.rowLabel.setText("Slot " + (i + 1));
									addRemoveButton(templatePanel, editorRow.deleteButton, gridbag, gridbagConstraints, i + 1);
									addLabel(templatePanel, editorRow.rowLabel, gridbag, gridbagConstraints, i + 1);
									addTypesCombo(templatePanel, editorRow.typeBox, gridbag, gridbagConstraints, i + 1);
									addNameField(templatePanel, editorRow.nameField, gridbag, gridbagConstraints, i + 1);
								}
								templatePanel.repaint();
								templatePanel.revalidate();
							}
						}
					}
				}
			}
		}
		
		private System.String getCurrentDeftemplateString(bool print)
		{
			System.Text.StringBuilder res = new System.Text.StringBuilder("(deftemplate " + moduleBox.SelectedItem + "::" + nameField.Text);
			if (print)
			{
				res.Append("\n");
			}
			Iterator itr = rows.iterator();
			while (itr.hasNext())
			{
				EditorRow row = (EditorRow) itr.next();
				res.Append("    (");
				if (row.typeBox.SelectedItem.toString().equals("MULTISLOT"))
				{
					res.Append("multislot " + row.nameField.Text + ")");
				}
				else
				{
					res.Append("slot " + row.nameField.Text);
					if (print)
						res.Append("\n");
					res.Append("        (type " + row.typeBox.SelectedItem.toString() + ")");
					if (print)
						res.Append("\n");
					res.Append("    )");
				}
				if (print)
					res.Append("\n");
			}
			res.Append(")");
			return res.ToString();
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		private void  addRemoveButton(JPanel parent, JButton button, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.NONE' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.NONE;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.EAST;
			gridbag.setConstraints(button, c);
			parent.add(button);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		private void  addLabel(JPanel parent, JLabel label, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.NONE' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.NONE;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.EAST;
			gridbag.setConstraints(label, c);
			parent.add(label);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		private void  addTypesCombo(JPanel parent, JComboBox combo, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 2;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.NONE' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.NONE;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.WEST;
			gridbag.setConstraints(combo, c);
			parent.add(combo);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagLayout"'
		//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
		private void  addNameField(JPanel parent, JTextField field, GridBagLayout gridbag, GridBagConstraints c, int row)
		{
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridx = 3;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.gridy = row;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGridBagConstraints"'
			c.anchor = GridBagConstraints.WEST;
			field.setColumns(30);
			gridbag.setConstraints(field, c);
			parent.add(field);
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'EditorRow' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class EditorRow
		{
			private void  InitBlock(TemplateEditor enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TemplateEditor enclosingInstance;
			public TemplateEditor Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private DeleteButton deleteButton;
			
			private JLabel rowLabel;
			
			private JComboBox typeBox;
			
			private JTextField nameField;
			
			private EditorRow(TemplateEditor enclosingInstance, DeleteButton deleteButton, JLabel rowLabel, JComboBox typeBox, JTextField nameField)
			{
				InitBlock(enclosingInstance);
				this.deleteButton = deleteButton;
				this.rowLabel = rowLabel;
				this.typeBox = typeBox;
				this.nameField = nameField;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DeleteButton' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class DeleteButton:JButton
		{
			private void  InitBlock(TemplateEditor enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TemplateEditor enclosingInstance;
			private int Row
			{
				get
				{
					return row;
				}
				
				set
				{
					this.row = value;
				}
				
			}
			public TemplateEditor Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			private int row;
			
			private DeleteButton(TemplateEditor enclosingInstance, ImageIcon icon, int row):base(icon)
			{
				InitBlock(enclosingInstance);
				this.row = row;
			}
			
			
		}
	}
}