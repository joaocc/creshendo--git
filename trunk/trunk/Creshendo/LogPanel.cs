namespace org.jamocha.gui.tab
{
	using System;
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using JButton = javax.swing.JButton;
	using JComponent = javax.swing.JComponent;
	using JPanel = javax.swing.JPanel;
	using JScrollPane = javax.swing.JScrollPane;
	using JSplitPane = javax.swing.JSplitPane;
	using JTable = javax.swing.JTable;
	using JTextArea = javax.swing.JTextArea;
	using ListSelectionModel = javax.swing.ListSelectionModel;
	using ListSelectionEvent = javax.swing.event.ListSelectionEvent;
	using ListSelectionListener = javax.swing.event.ListSelectionListener;
	using AbstractTableModel = javax.swing.table.AbstractTableModel;
	using DefaultTableCellRenderer = javax.swing.table.DefaultTableCellRenderer;
	using TableCellRenderer = javax.swing.table.TableCellRenderer;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using InterestType = org.jamocha.messagerouter.InterestType;
	using MessageEvent = org.jamocha.messagerouter.MessageEvent;
	using StringChannel = org.jamocha.messagerouter.StringChannel;
	using Function = org.jamocha.rete.Function;
	
	public class LogPanel:AbstractJamochaPanel, ListSelectionListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassJTable' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassJTable : JTable
		{
			public AnonymousClassJTable(LogPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LogPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private LogPanel enclosingInstance;
			public LogPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			public virtual TableCellRenderer getCellRenderer(int row, int column)
			{
				return Enclosing_Instance.cellRenderer;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassThread:SupportClass.ThreadClass
		{
			public AnonymousClassThread(LogPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LogPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private LogPanel enclosingInstance;
			public LogPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: The equivalent of method 'java.lang.Thread.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			override public void  Run()
			{
				List msgEvents = new LinkedList();
				while (Enclosing_Instance.running)
				{
					Enclosing_Instance.logChannel.fillEventList(msgEvents);
					if (!msgEvents.isEmpty())
					{
						Enclosing_Instance.dataModel.addEvents(msgEvents);
						msgEvents.clear();
					}
					else
					{
						try
						{
							System.Threading.Thread.Sleep(new System.TimeSpan(10000 * 10));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
							// Can be ignored
						}
					}
				}
				Enclosing_Instance.gui.Engine.MessageRouter.closeChannel(Enclosing_Instance.logChannel);
			}
		}
		private void  InitBlock()
		{
			dataModel = new LogTableModel(this);
		}
		
		private const long serialVersionUID = 4811690181744862051L;
		
		private JSplitPane pane;
		
		private JTextArea detailView;
		
		private JTable logTable;
		
		private JButton clearButton;
		
		//UPGRADE_NOTE: The initialization of  'dataModel' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private LogTableModel dataModel;
		
		private LogTableCellRenderer cellRenderer;
		
		private StringChannel logChannel;
		
		private bool running = true;
		
		public LogPanel(JamochaGui gui):base(gui)
		{
			InitBlock();
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			logChannel = gui.Engine.MessageRouter.openChannel("gui_log", InterestType.ALL);
			detailView = new JTextArea();
			detailView.setEditable(false);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1075"'
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			detailView.setFont(new System.Drawing.Font("Courier", 12, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular));
			cellRenderer = new LogTableCellRenderer(this);
			logTable = new AnonymousClassJTable(this, dataModel);
			logTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
			logTable.SelectionModel.addListSelectionListener(this);
			pane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, new JScrollPane(logTable), new JScrollPane(detailView));
			pane.setDividerLocation(gui.Preferences.getInt("log.dividerlocation", 300));
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(pane, BorderLayout.CENTER);
			
			SupportClass.ThreadClass logThread = new AnonymousClassThread(this);
			logThread.Start();
			clearButton = new JButton("Clear Log", IconLoader.getImageIcon("monitor"));
			clearButton.addActionListener(this);
			JPanel buttonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			buttonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			buttonPanel.add(clearButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(buttonPanel, BorderLayout.PAGE_END);
		}
		
		public override void  close()
		{
			running = false;
			gui.Preferences.putInt("log.dividerlocation", pane.DividerLocation);
		}
		
		public override void  settingsChanged()
		{
			
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'LogMessageEvent' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class LogMessageEvent:MessageEvent
		{
			private void  InitBlock(LogPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				datetime = new System.Globalization.GregorianCalendar();
			}
			private LogPanel enclosingInstance;
			public virtual int SuperType
			{
				get
				{
					return superType;
				}
				
			}
			public virtual System.String DatetimeFormatted
			{
				get
				{
					System.Text.StringBuilder res = new System.Text.StringBuilder();
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.YEAR) + "/");
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(((SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.MONTH) + 1 > 9)?"":"0") + (SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.MONTH) + 1) + "/");
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(((SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.DAY_OF_MONTH) > 9)?"":"0") + SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.DAY_OF_MONTH) + " - ");
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(((SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.HOUR_OF_DAY) > 9)?"":"0") + SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.HOUR_OF_DAY) + ":");
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(((SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.MINUTE) > 9)?"":"0") + SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.MINUTE) + ":");
					//UPGRADE_TODO: field 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilCalendarget_int"'
					res.Append(((SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.SECOND) > 9)?"":"0") + SupportClass.CalendarManager.manager.Get(datetime, SupportClass.CalendarManager.SECOND));
					return res.ToString();
				}
				
			}
			public virtual System.String TypeFormatted
			{
				get
				{
					return typeFormatted;
				}
				
			}
			public LogPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			new private const long serialVersionUID = - 5690784906495393031L;
			
			//UPGRADE_NOTE: The initialization of  'datetime' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private System.Globalization.Calendar datetime;
			
			private System.String typeFormatted;
			
			private int superType;
			
			public const int TYPE_EVENT = 1;
			
			public const int TYPE_WARNING = 2;
			
			public const int TYPE_ERROR = 3;
			
			public LogMessageEvent(LogPanel enclosingInstance, MessageEvent event_Renamed):this(enclosingInstance, event_Renamed.Type, event_Renamed.Message, event_Renamed.ChannelId)
			{
				InitBlock(enclosingInstance);
			}
			
			public LogMessageEvent(LogPanel enclosingInstance, int type, System.Object message, System.String channelId):base(type, message, channelId)
			{
				InitBlock(enclosingInstance);
				switch (type)
				{
					
					case MessageEvent.ADD_NODE_ERROR: 
						typeFormatted = "ERROR: Error adding node";
						superType = 3;
						break;
					
					case MessageEvent.ADD_RULE_EVENT: 
						typeFormatted = "EVENT: added Rule";
						superType = 1;
						break;
					
					case MessageEvent.CLIPSPARSER_ERROR: 
						typeFormatted = "ERROR: Error in CLIPSParser";
						superType = 3;
						break;
					
					case MessageEvent.CLIPSPARSER_REINIT: 
						typeFormatted = "EVENT: CLIPSParser reinitialized";
						superType = 1;
						break;
					
					case MessageEvent.CLIPSPARSER_WARNING: 
						typeFormatted = "WARNING: CLIPSParser-Warning";
						superType = 2;
						break;
					
					case MessageEvent.COMMAND: 
						typeFormatted = "EVENT: incoming Command";
						superType = 1;
						break;
					
					case MessageEvent.ENGINE: 
						typeFormatted = "EVENT: Engine-Message";
						superType = 1;
						break;
					
					case MessageEvent.ERROR: 
						typeFormatted = "ERROR: unspecified Error";
						superType = 3;
						break;
					
					case MessageEvent.FUNCTION_INVALID: 
						typeFormatted = "WARNING: invalid Function";
						superType = 2;
						break;
					
					case MessageEvent.FUNCTION_NOT_FOUND: 
						typeFormatted = "WARNING: Function not found";
						superType = 2;
						break;
					
					case MessageEvent.INVALID_RULE: 
						typeFormatted = "WARNING: invalid Rule";
						superType = 2;
						break;
					
					case MessageEvent.PARSE_ERROR: 
						typeFormatted = "ERROR: Parse-Error";
						superType = 3;
						break;
					
					case MessageEvent.REMOVE_RULE_EVENT: 
						typeFormatted = "EVENT: Rule removed";
						superType = 1;
						break;
					
					case MessageEvent.RESULT: 
						typeFormatted = "EVENT: returned result";
						superType = 1;
						break;
					
					case MessageEvent.RULE_EXISTS: 
						typeFormatted = "EVENT: Rule exists";
						superType = 1;
						break;
					
					case MessageEvent.TEMPLATE_NOTFOUND: 
						typeFormatted = "WARNING: Template not found";
						superType = 2;
						break;
					
					default: 
						typeFormatted = "Unknown Messagetype";
						superType = 1;
						break;
					
				}
			}
			
			
			
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'LogTableCellRenderer' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class LogTableCellRenderer:DefaultTableCellRenderer
		{
			public LogTableCellRenderer(LogPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LogPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				colorError = System.Drawing.Color.RED;
				colorWarning = System.Drawing.Color.ORANGE;
				colorEvent = System.Drawing.Color.BLUE;
			}
			private LogPanel enclosingInstance;
			public LogPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = - 6649805279420707106L;
			
			//UPGRADE_NOTE: The initialization of  'colorError' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private System.Drawing.Color colorError;
			
			//UPGRADE_NOTE: The initialization of  'colorWarning' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private System.Drawing.Color colorWarning;
			
			//UPGRADE_NOTE: The initialization of  'colorEvent' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private System.Drawing.Color colorEvent;
			
			public System.Windows.Forms.Control getTableCellRendererComponent(JTable table, System.Object value_Renamed, bool isSelected, bool hasFocus, int row, int column)
			{
				JComponent returnComponent = (JComponent) base.getTableCellRendererComponent(table, value_Renamed, isSelected, hasFocus, row, column);
				switch (((LogTableModel) table.Model).getRow(row).SuperType)
				{
					
					case Enclosing_Instance.LogMessageEvent.TYPE_ERROR: 
						setForeground(colorError);
						break;
					
					case Enclosing_Instance.LogMessageEvent.TYPE_WARNING: 
						setForeground(colorWarning);
						break;
					
					default: 
						setForeground(colorEvent);
						break;
					
				}
				return returnComponent;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'LogTableModel' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private sealed class LogTableModel:AbstractTableModel
		{
			public LogTableModel(LogPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LogPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				events = new LinkedList();
			}
			private LogPanel enclosingInstance;
			public virtual int ColumnCount
			{
				get
				{
					return 3;
				}
				
			}
			public virtual int RowCount
			{
				get
				{
					return events.size();
				}
				
			}
			public LogPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			//UPGRADE_NOTE: The initialization of  'events' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
			private List events;
			
			private int maxEventCount = 1000;
			
			private void  addEvents(List events)
			{
				
				Enclosing_Instance.logTable.ColumnModel.getColumn(0).setPreferredWidth(180);
				Enclosing_Instance.logTable.ColumnModel.getColumn(1).setPreferredWidth(100);
				Enclosing_Instance.logTable.ColumnModel.getColumn(2).setPreferredWidth(Enclosing_Instance.logTable.Width - 280);
				for (int idx = 0; idx < events.size(); idx++)
				{
					MessageEvent event_Renamed = (MessageEvent) events.get(idx);
					this.events.add(new LogMessageEvent(event_Renamed));
				}
				while (this.events.size() > maxEventCount)
				{
					this.events.remove(0);
				}
				fireTableDataChanged();
			}
			
			private void  clearEvents()
			{
				events.clear();
				fireTableDataChanged();
			}
			
			public System.String getColumnName(int column)
			{
				switch (column)
				{
					
					case 0: 
						return "Date - Time";
					
					case 1: 
						return "Channel";
					
					case 2: 
						return "Message-Type";
					
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
				return typeof(System.String);
			}
			
			
			public LogMessageEvent getRow(int row)
			{
				return (LogMessageEvent) events.get(events.size() - (row + 1));
			}
			
			public System.Object getValueAt(int row, int column)
			{
				LogMessageEvent event_Renamed = getRow(row);
				if (event_Renamed != null)
				{
					switch (column)
					{
						
						case 0: 
							return event_Renamed.DatetimeFormatted;
						
						case 1: 
							return event_Renamed.ChannelId;
						
						case 2: 
							return event_Renamed.TypeFormatted;
						}
				}
				return null;
			}
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == clearButton)
			{
				dataModel.clearEvents();
				detailView.setText("");
			}
		}
		
		public virtual void  valueChanged(ListSelectionEvent arg0)
		{
			if (arg0.Source == logTable.SelectionModel)
			{
				System.Text.StringBuilder buffer = new System.Text.StringBuilder();
				if (logTable.SelectedRow > - 1)
				{
					LogMessageEvent event_Renamed = dataModel.getRow(logTable.SelectedRow);
					buffer.Append("Date-Time:    " + event_Renamed.DatetimeFormatted + "\nChannel:      " + event_Renamed.ChannelId + "\nMessage-Type: " + event_Renamed.TypeFormatted + "\n\nMessage:\n========\n");
					System.Object message = event_Renamed.Message;
					if (message is System.Exception)
					{
						System.Exception ex = (System.Exception) message;
						StackTraceElement[] str = ex.StackTrace;
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
						buffer.Append(ex.GetType().FullName + ": " + ex.Message);
						for (int idx = 0; idx < str.length; idx++)
						{
							StackTraceElement strelem = str[idx];
							buffer.Append("\n" + strelem);
						}
					}
					else if (message is Function)
					{
						buffer.Append("(" + ((Function) message).Name + ")");
					}
					else
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
						buffer.Append(message.ToString());
					}
				}
				detailView.setText(buffer.ToString());
			}
		}
	}
}