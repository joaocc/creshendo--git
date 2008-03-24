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
	using TableModel = javax.swing.table.TableModel;
	using TableModelEvent = javax.swing.event.TableModelEvent;
	using JTable = javax.swing.JTable;
	using JTableHeader = javax.swing.table.JTableHeader;
	using TableColumnModel = javax.swing.table.TableColumnModel;
	/// <summary> This class enables a JTable do be sortable.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class TableSorter:TableMap
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter
		{
			public AnonymousClassMouseAdapter(JTable tableView, org.jamocha.gui.TableSorter sorter, TableSorter enclosingInstance)
			{
				InitBlock(tableView, sorter, enclosingInstance);
			}
			private void  InitBlock(JTable tableView, org.jamocha.gui.TableSorter sorter, TableSorter enclosingInstance)
			{
				this.tableView = tableView;
				this.sorter = sorter;
				this.enclosingInstance = enclosingInstance;
				columnModel = tableView.ColumnModel;
			}
			//UPGRADE_NOTE: Final variable tableView was copied into class AnonymousClassMouseAdapter. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1023"'
			private JTable tableView;
			//UPGRADE_NOTE: Final variable sorter was copied into class AnonymousClassMouseAdapter. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1023"'
			private org.jamocha.gui.TableSorter sorter;
			private TableSorter enclosingInstance;
			public TableSorter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: Class java.awt.event.MouseAdapter was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
			public void  mouseClicked(System.Object event_sender, System.EventArgs e)
			{
				TableColumnModel columnModel;
				int viewColumn = columnModel.getColumnIndexAtX(e.X);
				int column = tableView.convertColumnIndexToModel(viewColumn);
				if (e.Clicks == 1 && column != - 1)
				{
					//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getModifiers' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
					//UPGRADE_ISSUE: Field 'java.awt.event.InputEvent.SHIFT_MASK' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
					int shiftPressed = e.getModifiers() & InputEvent.SHIFT_MASK;
					bool ascending = (shiftPressed == 0);
					sorter.sortByColumn(column, ascending);
				}
			}
		}
		private void  InitBlock()
		{
			sortingColumns = new System.Collections.ArrayList();
		}
		virtual public TableModel Model
		{
			set
			{
				base.setModel(value);
				reallocateIndexes();
			}
			
		}
		
		new private const long serialVersionUID = - 1884558901554019827L;
		
		private int[] indexes;
		
		//UPGRADE_NOTE: The initialization of  'sortingColumns' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private System.Collections.ArrayList sortingColumns;
		
		private bool ascending = true;
		
		private int compares;
		
		public TableSorter()
		{
			InitBlock();
			indexes = new int[0];
		}
		
		public TableSorter(TableModel model)
		{
			InitBlock();
			setModel(model);
		}
		
		
		public virtual int compareRowsByColumn(int row1, int row2, int column)
		{
			System.Type type = model.getColumnClass(column);
			TableModel data = model;
			
			System.Object o1 = data.getValueAt(row1, column);
			System.Object o2 = data.getValueAt(row2, column);
			
			if (o1 == null && o2 == null)
			{
				return 0;
			}
			else if (o1 == null)
			{
				return - 1;
			}
			else if (o2 == null)
			{
				return 1;
			}
			
			//UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
			if (type == typeof(java.lang.Number))
			{
				//UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
				Number n1 = (Number) data.getValueAt(row1, column);
				//UPGRADE_ISSUE: Method 'java.lang.Number.doubleValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
				double d1 = n1.doubleValue();
				//UPGRADE_ISSUE: Class 'java.lang.Number' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
				Number n2 = (Number) data.getValueAt(row2, column);
				//UPGRADE_ISSUE: Method 'java.lang.Number.doubleValue' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangNumber"'
				double d2 = n2.doubleValue();
				
				if (d1 < d2)
				{
					return - 1;
				}
				else if (d1 > d2)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			else if (type == typeof(System.DateTime))
			{
				System.DateTime d1 = (System.DateTime) data.getValueAt(row1, column);
				long n1 = ((d1.Ticks - 621355968000000000) / 10000) - (long) System.TimeZone.CurrentTimeZone.GetUtcOffset(d1).TotalMilliseconds;
				System.DateTime d2 = (System.DateTime) data.getValueAt(row2, column);
				long n2 = ((d2.Ticks - 621355968000000000) / 10000) - (long) System.TimeZone.CurrentTimeZone.GetUtcOffset(d2).TotalMilliseconds;
				
				if (n1 < n2)
				{
					return - 1;
				}
				else if (n1 > n2)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			else if (type == typeof(System.String))
			{
				System.String s1 = (System.String) data.getValueAt(row1, column);
				System.String s2 = (System.String) data.getValueAt(row2, column);
				int result = s1.CompareTo(s2);
				
				if (result < 0)
				{
					return - 1;
				}
				else if (result > 0)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			else if (type == (System.Object) typeof(System.Boolean))
			{
				System.Boolean bool1 = (System.Boolean) data.getValueAt(row1, column);
				bool b1 = bool1;
				System.Boolean bool2 = (System.Boolean) data.getValueAt(row2, column);
				bool b2 = bool2;
				
				if (b1 == b2)
				{
					return 0;
				}
				else if (b1)
				{
					return 1;
				}
				else
				{
					return - 1;
				}
			}
			else
			{
				System.Object v1 = data.getValueAt(row1, column);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
				System.String s1 = v1.ToString();
				System.Object v2 = data.getValueAt(row2, column);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
				System.String s2 = v2.ToString();
				int result = s1.CompareTo(s2);
				
				if (result < 0)
				{
					return - 1;
				}
				else if (result > 0)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}
		
		public virtual int compare(int row1, int row2)
		{
			compares++;
			for (int level = 0; level < sortingColumns.Count; level++)
			{
				System.Int32 column = (System.Int32) sortingColumns[level];
				int result = compareRowsByColumn(row1, row2, column);
				if (result != 0)
				{
					return ascending?result:- result;
				}
			}
			return 0;
		}
		
		public virtual void  reallocateIndexes()
		{
			int rowCount = model.RowCount;
			
			indexes = new int[rowCount];
			
			for (int row = 0; row < rowCount; row++)
			{
				indexes[row] = row;
			}
		}
		
		public virtual void  tableChanged(TableModelEvent e)
		{
			reallocateIndexes();
			
			base.tableChanged(e);
		}
		
		public virtual void  checkModel()
		{
			if (indexes.Length != model.RowCount)
			{
				System.Console.Error.WriteLine("Sorter not informed of a change in model.");
			}
		}
		
		public virtual void  sort(System.Object sender)
		{
			checkModel();
			
			compares = 0;
			//UPGRADE_NOTE: Variable 'generated_var' was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1011"'
			int[] generated_var = new int[indexes.Length];
			indexes.CopyTo(generated_var, 0);
			shuttlesort(generated_var, indexes, 0, indexes.Length);
		}
		
		public virtual void  n2sort()
		{
			for (int i = 0; i < RowCount; i++)
			{
				for (int j = i + 1; j < RowCount; j++)
				{
					if (compare(indexes[i], indexes[j]) == - 1)
					{
						swap(i, j);
					}
				}
			}
		}
		
		public virtual void  shuttlesort(int[] from, int[] to, int low, int high)
		{
			if (high - low < 2)
			{
				return ;
			}
			int middle = (low + high) / 2;
			shuttlesort(to, from, low, middle);
			shuttlesort(to, from, middle, high);
			
			int p = low;
			int q = middle;
			
			if (high - low >= 4 && compare(from[middle - 1], from[middle]) <= 0)
			{
				for (int i = low; i < high; i++)
				{
					to[i] = from[i];
				}
				return ;
			}
			
			for (int i = low; i < high; i++)
			{
				if (q >= high || (p < middle && compare(from[p], from[q]) <= 0))
				{
					to[i] = from[p++];
				}
				else
				{
					to[i] = from[q++];
				}
			}
		}
		
		public virtual void  swap(int i, int j)
		{
			int tmp = indexes[i];
			indexes[i] = indexes[j];
			indexes[j] = tmp;
		}
		
		public override System.Object getValueAt(int aRow, int aColumn)
		{
			checkModel();
			return model.getValueAt(indexes[aRow], aColumn);
		}
		
		public override void  setValueAt(System.Object aValue, int aRow, int aColumn)
		{
			checkModel();
			model.setValueAt(aValue, indexes[aRow], aColumn);
		}
		
		public virtual void  sortByColumn(int column)
		{
			sortByColumn(column, true);
		}
		
		public virtual void  sortByColumn(int column, bool ascending)
		{
			this.ascending = ascending;
			System.Collections.ArrayList temp_arraylist;
			temp_arraylist = sortingColumns;
			temp_arraylist.RemoveRange(0, temp_arraylist.Count);
			sortingColumns.Add(column);
			sort(this);
			base.tableChanged(new TableModelEvent(this));
		}
		
		public virtual void  addMouseListenerToHeaderInTable(JTable table)
		{
			//UPGRADE_NOTE: Final was removed from the declaration of 'sorter '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
			TableSorter sorter = this;
			//UPGRADE_NOTE: Final was removed from the declaration of 'tableView '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
			JTable tableView = table;
			tableView.setColumnSelectionAllowed(false);
			MouseAdapter listMouseListener = new AnonymousClassMouseAdapter(tableView, sorter, this);
			JTableHeader th = tableView.TableHeader;
			th.addMouseListener(listMouseListener);
		}
	}
}