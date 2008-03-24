/// <summary> Copyright 2007 Nikolaus Koemm
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
	using Collection = java.util.Collection;
	using Collections = java.util.Collections;
	using List = java.util.List;
	using JButton = javax.swing.JButton;
	using JPanel = javax.swing.JPanel;
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
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	using Function = org.jamocha.rete.Function;
	/// <summary> This Panel shows all functions currently in the Jamocha engine.
	/// 
	/// </summary>
	/// <author>  Nikolaus Koemm
	/// 
	/// </author>
	public class FunctionsPanel:AbstractJamochaPanel, ListSelectionListener
	{
		
		private const long serialVersionUID = 23;
		
		private JTextArea dumpAreaFunction;
		
		private JSplitPane pane;
		
		private JTable functionsTable;
		
		private FunctionsTableModel dataModel;
		
		private StringChannel editorChannel;
		
		private JButton reloadButton;
		
		public FunctionsPanel(JamochaGui gui):base(gui)
		{
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			
			dataModel = new FunctionsTableModel(this);
			TableSorter sorter = new TableSorter(new TableMap());
			((TableMap) sorter.Model).setModel(dataModel);
			functionsTable = new JTable(sorter);
			sorter.addMouseListenerToHeaderInTable(functionsTable);
			
			functionsTable.setShowHorizontalLines(false);
			functionsTable.setRowSelectionAllowed(true);
			functionsTable.TableHeader.setReorderingAllowed(false);
			functionsTable.TableHeader.setToolTipText("Click to sort ascending. Click while pressing the shift-key down to sort descending");
			functionsTable.SelectionModel.addListSelectionListener(this);
			dumpAreaFunction = new JTextArea();
			dumpAreaFunction.setLineWrap(true);
			dumpAreaFunction.setWrapStyleWord(true);
			dumpAreaFunction.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			dumpAreaFunction.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			
			pane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, new JScrollPane(functionsTable), new JScrollPane(dumpAreaFunction));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(pane, BorderLayout.CENTER);
			pane.setDividerLocation(gui.Preferences.getInt("functions.dividerlocation", 300));
			
			reloadButton = new JButton("Reload Functions", IconLoader.getImageIcon("arrow_refresh"));
			reloadButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			buttonPanel.add(reloadButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(buttonPanel, BorderLayout.PAGE_END);
			
			
			initFunctionsList();
		}
		
		
		private void  initFunctionsList()
		{
			Collection c = gui.Engine.AllFunctions;
			Function[] func = (Function[]) c.toArray(new Function[0]);
			List funcs = new ArrayList();
			bool larger = false;
			funcs.add(0, func[0]);
			for (int idx = 1; idx <= func.Length - 1; idx++)
			{
				int bound = funcs.size();
				larger = true;
				for (int indx = 0; indx < bound; indx++)
				{
					int cmpvalue = func[idx].Name.CompareTo(((Function) funcs.get(indx)).Name);
					if (cmpvalue < 0)
					{
						funcs.add(indx, func[idx]);
						indx = bound;
						larger = false;
					}
					else if (cmpvalue == 0)
					{
						indx = bound;
						larger = false;
					}
				}
				if (larger)
				{
					funcs.add(func[idx]);
				}
			}
			dataModel.setFunctions(funcs);
			functionsTable.ColumnModel.getColumn(0).setPreferredWidth(50);
		}
		
		
		public override void  setFocus()
		{
			base.setFocus();
			initFunctionsList();
		}
		
		public override void  close()
		{
			if (editorChannel != null)
				gui.Engine.MessageRouter.closeChannel(editorChannel);
			gui.Preferences.putInt("functions.dividerlocation", pane.DividerLocation);
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender.equals(reloadButton))
			{
				initFunctionsList();
			}
		}
		
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FunctionsTableModel' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class FunctionsTableModel:AbstractTableModel
		{
			public FunctionsTableModel(FunctionsPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FunctionsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				funclist = new ArrayList();
			}
			private FunctionsPanel enclosingInstance;
			private List Functions
			{
				set
				{
					this.funclist = value;
					fireTableDataChanged();
				}
				
			}
			public virtual int ColumnCount
			{
				get
				{
					return 1;
				}
				
			}
			public virtual int RowCount
			{
				get
				{
					return funclist.size();
				}
				
			}
			public FunctionsPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			//UPGRADE_NOTE: The initialization of  'funclist' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private List funclist;
			
			
			
			public System.String getColumnName(int column)
			{
				switch (column)
				{
					
					case 0: 
						return "Functions";
					
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
				else
					return typeof(System.Type);
			}
			
			
			public Function getRow(int row)
			{
				return (Function) funclist.get(row);
			}
			
			public System.Object getValueAt(int row, int column)
			{
				System.String functionname = getRow(row).Name;
				return functionname;
			}
		}
		
		
		
		public virtual void  valueChanged(ListSelectionEvent arg0)
		{
			if (arg0.Source == functionsTable.SelectionModel)
			{
				System.Text.StringBuilder buffer = new System.Text.StringBuilder();
				if (functionsTable.SelectedColumnCount == 1 && functionsTable.SelectedRow > - 1)
				{
					Function function = dataModel.getRow(functionsTable.SelectedRow);
					if (function != null)
					{
						buffer.Append(function.toPPString(null, 0));
						buffer.Append("\n");
					}
				}
				dumpAreaFunction.setText(buffer.ToString());
			}
		}
	}
}