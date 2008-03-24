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
	using AbstractTableModel = javax.swing.table.AbstractTableModel;
	using TableModel = javax.swing.table.TableModel;
	using TableModelListener = javax.swing.event.TableModelListener;
	using TableModelEvent = javax.swing.event.TableModelEvent;
	/// <summary> This is a supporting class for the TableSorter.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class TableMap:AbstractTableModel, TableModelListener
	{
		virtual public TableModel Model
		{
			get
			{
				return model;
			}
			
			set
			{
				this.model = value;
				value.addTableModelListener(this);
			}
			
		}
		virtual public int RowCount
		{
			get
			{
				return (model == null)?0:model.RowCount;
			}
			
		}
		virtual public int ColumnCount
		{
			get
			{
				return (model == null)?0:model.ColumnCount;
			}
			
		}
		
		private const long serialVersionUID = 4007086145879578418L;
		
		protected internal TableModel model;
		
		
		
		public virtual System.Object getValueAt(int aRow, int aColumn)
		{
			return model.getValueAt(aRow, aColumn);
		}
		
		public virtual void  setValueAt(System.Object aValue, int aRow, int aColumn)
		{
			model.setValueAt(aValue, aRow, aColumn);
		}
		
		
		
		public virtual System.String getColumnName(int aColumn)
		{
			return model.getColumnName(aColumn);
		}
		
		public virtual System.Type getColumnClass(int aColumn)
		{
			return model.getColumnClass(aColumn);
		}
		
		public virtual bool isCellEditable(int row, int column)
		{
			return model.isCellEditable(row, column);
		}
		
		public virtual void  tableChanged(TableModelEvent e)
		{
			fireTableChanged(e);
		}
	}
}