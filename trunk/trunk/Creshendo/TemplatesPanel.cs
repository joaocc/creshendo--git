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
	using Collection = java.util.Collection;
	using Iterator = java.util.Iterator;
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using JButton = javax.swing.JButton;
	using JMenuItem = javax.swing.JMenuItem;
	using JPanel = javax.swing.JPanel;
	using JPopupMenu = javax.swing.JPopupMenu;
	using JScrollPane = javax.swing.JScrollPane;
	using JSplitPane = javax.swing.JSplitPane;
	using JTable = javax.swing.JTable;
	using JTextArea = javax.swing.JTextArea;
	using ListSelectionEvent = javax.swing.event.ListSelectionEvent;
	using ListSelectionListener = javax.swing.event.ListSelectionListener;
	using AbstractTableModel = javax.swing.table.AbstractTableModel;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using TableMap = org.jamocha.gui.TableMap;
	using TableSorter = org.jamocha.gui.TableSorter;
	using TemplateEditor = org.jamocha.gui.editor.TemplateEditor;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using ConversionUtils = org.jamocha.rete.ConversionUtils;
	using Module = org.jamocha.rete.Module;
	using MultiSlot = org.jamocha.rete.MultiSlot;
	using Slot = org.jamocha.rete.Slot;
	using Template = org.jamocha.rete.Template;
	/// <summary> This Panel shows all Templates of all Modules currently in the Engine. You
	/// can add new Templates or delete old ones.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// </author>
	/// <version>  0.01
	/// 
	/// </version>
	public class TemplatesPanel:AbstractJamochaPanel, ListSelectionListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter
		{
			public AnonymousClassMouseAdapter(TemplatesPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(TemplatesPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TemplatesPanel enclosingInstance;
			public TemplatesPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				int[] selCols = Enclosing_Instance.templatesTable.SelectedRows;
				for (int i = 0; i < selCols.Length; ++i)
				{
					System.String modName = (System.String) Enclosing_Instance.dataModel.getValueAt(selCols[i], 0);
					Enclosing_Instance.gui.Engine.WorkingMemory.findModule(modName).removeTemplate(Enclosing_Instance.dataModel.getRow(selCols[i]).Template, Enclosing_Instance.gui.Engine, Enclosing_Instance.gui.Engine.WorkingMemory);
				}
				Enclosing_Instance.initTemplatesList();
			}
		}
		
		private const long serialVersionUID = - 5732131176258158968L;
		
		private JSplitPane pane;
		
		private JTable templatesTable;
		
		private TemplatesTableModel dataModel;
		
		private JButton reloadButton;
		
		private JButton createNewButton;
		
		private JTextArea dumpArea;
		
		public TemplatesPanel(JamochaGui gui):base(gui)
		{
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			
			dataModel = new TemplatesTableModel(this);
			TableSorter sorter = new TableSorter(new TableMap());
			((TableMap) sorter.Model).setModel(dataModel);
			templatesTable = new JTable(sorter);
			sorter.addMouseListenerToHeaderInTable(templatesTable);
			
			templatesTable.setShowHorizontalLines(true);
			templatesTable.setRowSelectionAllowed(true);
			templatesTable.TableHeader.setReorderingAllowed(false);
			templatesTable.TableHeader.setToolTipText("Click to sort ascending. Click while pressing the shift-key down to sort descending");
			templatesTable.SelectionModel.addListSelectionListener(this);
			dumpArea = new JTextArea();
			dumpArea.setLineWrap(true);
			dumpArea.setWrapStyleWord(true);
			dumpArea.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpArea.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			
			pane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, new JScrollPane(templatesTable), new JScrollPane(dumpArea));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(pane, BorderLayout.CENTER);
			pane.setDividerLocation(gui.Preferences.getInt("facts.dividerlocation", 300));
			reloadButton = new JButton("Reload Templates", IconLoader.getImageIcon("arrow_refresh"));
			reloadButton.addActionListener(this);
			createNewButton = new JButton("Create new Template", IconLoader.getImageIcon("brick_add"));
			createNewButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			buttonPanel.add(reloadButton);
			buttonPanel.add(createNewButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(buttonPanel, BorderLayout.PAGE_END);
			
			initPopupMenu();
		}
		
		private void  initTemplatesList()
		{
			dataModel.clear();
			Collection modules = gui.Engine.WorkingMemory.Modules;
			// dataModel.setTemplates(modules);
			Iterator itr = modules.iterator();
			while (itr.hasNext())
			{
				Module module = (Module) itr.next();
				Collection templates = module.Templates;
				dataModel.addTemplates(templates, module);
			}
			templatesTable.ColumnModel.getColumn(0).setPreferredWidth(100);
			templatesTable.ColumnModel.getColumn(1).setPreferredWidth(templatesTable.Width - 100);
		}
		
		private void  initPopupMenu()
		{
			JPopupMenu menu = new JPopupMenu();
			JMenuItem removeItem = new JMenuItem("Remove selected Template(s)", IconLoader.getImageIcon("brick_delete"));
			removeItem.addMouseListener(new AnonymousClassMouseAdapter(this));
			menu.add(removeItem);
			// templatesTable.setComponentPopupMenu(menu);
		}
		
		public override void  setFocus()
		{
			base.setFocus();
			initTemplatesList();
		}
		
		public override void  close()
		{
			gui.Preferences.putInt("templates.dividerlocation", pane.DividerLocation);
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender.equals(reloadButton))
			{
				initTemplatesList();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender.equals(createNewButton))
				{
					TemplateEditor editor = new TemplateEditor(gui.Engine);
					editor.setStringChannel(gui.StringChannel);
					editor.init();
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'TemplatesTableModel' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class TemplatesTableModel:AbstractTableModel
		{
			public TemplatesTableModel(TemplatesPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(TemplatesPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				templates = new LinkedList();
			}
			private TemplatesPanel enclosingInstance;
			public virtual int ColumnCount
			{
				get
				{
					return 2;
				}
				
			}
			public virtual int RowCount
			{
				get
				{
					return templates.size();
				}
				
			}
			public TemplatesPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			//UPGRADE_NOTE: The initialization of  'templates' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private List templates;
			
			private void  clear()
			{
				templates = new LinkedList();
				fireTableDataChanged();
			}
			
			private void  addTemplates(Collection templates, Module module)
			{
				Iterator itr = templates.iterator();
				while (itr.hasNext())
				{
					Template template = (Template) itr.next();
					ExtTemplate exttemp = new ExtTemplate(template, module);
					this.templates.add(exttemp);
				}
				fireTableDataChanged();
			}
			
			public System.String getColumnName(int column)
			{
				switch (column)
				{
					
					case 0: 
						return "Module";
					
					case 1: 
						return "Template";
					
					default: 
						return null;
					
				}
			}
			
			
			public bool isCellEditable(int row, int col)
			{
				return false;
			}
			
			public System.Type getColumnClass(int aColumn)
			{
				if (aColumn == 0)
					return typeof(System.String);
				else if (aColumn == 1)
					return typeof(System.String);
				else
					return typeof(System.Type);
			}
			
			
			public ExtTemplate getRow(int row)
			{
				return (ExtTemplate) templates.get(row);
			}
			
			public System.Object getValueAt(int row, int column)
			{
				ExtTemplate template = getRow(row);
				switch (column)
				{
					
					case 0: 
						return template.Module.ModuleName;
					
					case 1: 
						return template.Template.Name;
					}
				return null;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ExtTemplate' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class ExtTemplate
		{
			private void  InitBlock(TemplatesPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TemplatesPanel enclosingInstance;
			private Template Template
			{
				get
				{
					return template;
				}
				
			}
			private Module Module
			{
				get
				{
					return module;
				}
				
			}
			public TemplatesPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private Template template;
			
			private Module module;
			
			private ExtTemplate(TemplatesPanel enclosingInstance, Template template, Module module)
			{
				InitBlock(enclosingInstance);
				this.template = template;
				this.module = module;
			}
			
			
		}
		
		public virtual void  valueChanged(ListSelectionEvent arg0)
		{
			if (arg0.Source == templatesTable.SelectionModel)
			{
				System.Text.StringBuilder buffer = new System.Text.StringBuilder();
				if (templatesTable.SelectedColumnCount == 1 && templatesTable.SelectedRow > - 1)
				{
					ExtTemplate template = dataModel.getRow(templatesTable.SelectedRow);
					if (template != null)
					{
						buffer.Append("(" + template.Module.ModuleName + "::" + template.Template.Name);
						Slot[] slots = template.Template.AllSlots;
						for (int idx = 0; idx < slots.Length; idx++)
						{
							Slot slot = slots[idx];
							buffer.Append("\n    (");
							if (slot is MultiSlot)
								buffer.Append("multislot " + slot.Name + ")");
							else
							{
								buffer.Append("slot " + slot.Name + " (type " + ConversionUtils.getTypeName(slot.ValueType) + ") )");
							}
						}
						buffer.Append("\n)");
					}
				}
				dumpArea.setText(buffer.ToString());
			}
		}
	}
}