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
	using ArrayList = java.util.ArrayList;
	using Collections = java.util.Collections;
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
	using FactEditor = org.jamocha.gui.editor.FactEditor;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using Constants = org.jamocha.rete.Constants;
	using Fact = org.jamocha.rete.Fact;
	using MultiSlot = org.jamocha.rete.MultiSlot;
	using Slot = org.jamocha.rete.Slot;
	using RetractException = org.jamocha.rete.exception.RetractException;
	/// <summary> This Panel shows all facts currently in the Jamocha engine. You can assert
	/// new facts or retract existing ones.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class FactsPanel:AbstractJamochaPanel, ListSelectionListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter
		{
			public AnonymousClassMouseAdapter(FactsPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FactsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FactsPanel enclosingInstance;
			public FactsPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				int[] selCols = Enclosing_Instance.factsTable.SelectedRows;
				for (int i = 0; i < selCols.Length; ++i)
				{
					System.Int64 value_Renamed = (System.Int64) Enclosing_Instance.dataModel.getValueAt(selCols[i], 0);
					try
					{
						Enclosing_Instance.gui.Engine.retractById((long) value_Renamed);
					}
					catch (System.FormatException e)
					{
						// ignore it
					}
					catch (RetractException e)
					{
						SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
				Enclosing_Instance.initFactsList();
			}
		}
		
		private const long serialVersionUID = - 5732131176258158968L;
		
		private JSplitPane pane;
		
		private JTable factsTable;
		
		private FactsTableModel dataModel;
		
		private JButton reloadButton;
		
		private JButton assertButton;
		
		private JTextArea dumpArea;
		
		public FactsPanel(JamochaGui gui):base(gui)
		{
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			
			dataModel = new FactsTableModel(this);
			TableSorter sorter = new TableSorter(new TableMap());
			((TableMap) sorter.Model).setModel(dataModel);
			factsTable = new JTable(sorter);
			sorter.addMouseListenerToHeaderInTable(factsTable);
			
			factsTable.setShowHorizontalLines(true);
			factsTable.setRowSelectionAllowed(true);
			factsTable.TableHeader.setReorderingAllowed(false);
			factsTable.TableHeader.setToolTipText("Click to sort ascending. Click while pressing the shift-key down to sort descending");
			factsTable.SelectionModel.addListSelectionListener(this);
			dumpArea = new JTextArea();
			dumpArea.setLineWrap(true);
			dumpArea.setWrapStyleWord(true);
			dumpArea.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpArea.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			
			pane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, new JScrollPane(factsTable), new JScrollPane(dumpArea));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(pane, BorderLayout.CENTER);
			pane.setDividerLocation(gui.Preferences.getInt("facts.dividerlocation", 300));
			reloadButton = new JButton("Reload Facts", IconLoader.getImageIcon("database_refresh"));
			reloadButton.addActionListener(this);
			assertButton = new JButton("Assert Fact", IconLoader.getImageIcon("database_add"));
			assertButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			buttonPanel.add(reloadButton);
			buttonPanel.add(assertButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(buttonPanel, BorderLayout.PAGE_END);
			
			initPopupMenu();
		}
		
		private void  initFactsList()
		{
			List facts = gui.Engine.AllFacts;
			dataModel.setFacts(facts);
			factsTable.ColumnModel.getColumn(0).setPreferredWidth(50);
			factsTable.ColumnModel.getColumn(1).setPreferredWidth(factsTable.Width - 50);
		}
		
		private void  initPopupMenu()
		{
			JPopupMenu menu = new JPopupMenu();
			JMenuItem retractItem = new JMenuItem("Retract selected Fact(s)", IconLoader.getImageIcon("database_delete"));
			retractItem.addMouseListener(new AnonymousClassMouseAdapter(this));
			menu.add(retractItem);
			// factsTable.setComponentPopupMenu(menu);
		}
		
		public override void  setFocus()
		{
			base.setFocus();
			initFactsList();
		}
		
		public override void  close()
		{
			gui.Preferences.putInt("facts.dividerlocation", pane.DividerLocation);
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender.equals(reloadButton))
			{
				initFactsList();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender.equals(assertButton))
				{
					FactEditor editor = new FactEditor(gui.Engine);
					editor.setStringChannel(gui.StringChannel);
					editor.init();
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FactsTableModel' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class FactsTableModel:AbstractTableModel
		{
			public FactsTableModel(FactsPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FactsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				facts = new ArrayList();
			}
			private FactsPanel enclosingInstance;
			private List Facts
			{
				set
				{
					this.facts = value;
					fireTableDataChanged();
				}
				
			}
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
					return facts.size();
				}
				
			}
			public FactsPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			//UPGRADE_NOTE: The initialization of  'facts' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private List facts;
			
			
			public System.String getColumnName(int column)
			{
				switch (column)
				{
					
					case 0: 
						return "ID";
					
					case 1: 
						return "Fact";
					
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
				{
					//UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
					return typeof(java.lang.Number);
				}
				else if (aColumn == 1)
					return typeof(System.String);
				else
					return typeof(System.Type);
			}
			
			
			public Fact getRow(int row)
			{
				return (Fact) facts.get(row);
			}
			
			public System.Object getValueAt(int row, int column)
			{
				Fact fact = getRow(row);
				switch (column)
				{
					
					case 0: 
						return fact.FactId;
					
					case 1: 
						return fact.toFactString();
					}
				return null;
			}
		}
		
		public virtual void  valueChanged(ListSelectionEvent arg0)
		{
			if (arg0.Source == factsTable.SelectionModel)
			{
				System.Text.StringBuilder buffer = new System.Text.StringBuilder();
				if (factsTable.SelectedColumnCount == 1 && factsTable.SelectedRow > - 1)
				{
					Fact fact = dataModel.getRow(factsTable.SelectedRow);
					if (fact != null)
					{
						buffer.Append("f-" + fact.FactId + "(" + fact.Deftemplate.Name);
						Slot[] slots = fact.Deftemplate.AllSlots;
						for (int idx = 0; idx < slots.Length; idx++)
						{
							Slot slot = slots[idx];
							buffer.Append("\n    (" + slot.Name + " ");
							if (slot.ValueType == Constants.ARRAY_TYPE)
							{
								MultiSlot ms = (MultiSlot) slot;
								for (int i = 0; i < ((System.Object[]) ms.Value).Length; ++i)
								{
									if (i > 0)
										buffer.Append(" ");
									//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
									buffer.Append(((System.Object[]) ms.Value)[i].ToString());
								}
							}
							else
							{
								//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
								System.String value_Renamed = fact.getSlotValue(slot.Id).ToString();
								if (!value_Renamed.Equals(""))
									buffer.Append(value_Renamed);
							}
							buffer.Append(")");
						}
						buffer.Append("\n)");
					}
				}
				dumpArea.setText(buffer.ToString());
			}
		}
	}
}