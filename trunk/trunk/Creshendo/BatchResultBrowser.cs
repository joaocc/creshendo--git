namespace org.jamocha.gui
{
	using System;
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using Map = java.util.Map;
	using BorderFactory = javax.swing.BorderFactory;
	using DefaultComboBoxModel = javax.swing.DefaultComboBoxModel;
	using JButton = javax.swing.JButton;
	using JComboBox = javax.swing.JComboBox;
	using JFrame = javax.swing.JFrame;
	using JPanel = javax.swing.JPanel;
	using JScrollPane = javax.swing.JScrollPane;
	using JTextArea = javax.swing.JTextArea;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	
	public class BatchResultBrowser:JFrame
	{
		virtual internal Map Results
		{
			set
			{
				this.batchResults = value;
				resultsBoxModel.setItems(value.keySet().toArray());
			}
			
		}
		
		private const long serialVersionUID = 1L;
		
		private JPanel topPanel;
		
		private JButton batchResultsButton;
		
		private JButton removeButton;
		
		private JButton reloadButton;
		
		private JButton closeButton;
		
		private JTextArea aboutArea;
		
		private JComboBox resultsBox;
		
		private ResultBoxModel resultsBoxModel;
		
		private Map batchResults;
		
		internal BatchResultBrowser(JButton batchResultsButton)
		{
			this.batchResultsButton = batchResultsButton;
			topPanel = new JPanel();
			setSize(500, 400);
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			resultsBoxModel = new ResultBoxModel(this);
			resultsBox = new JComboBox(resultsBoxModel);
			removeButton = new JButton(IconLoader.getImageIcon("delete"));
			removeButton.addActionListener(this);
			removeButton.setToolTipText("Remove this batch result");
			reloadButton = new JButton(IconLoader.getImageIcon("arrow_refresh"));
			reloadButton.addActionListener(this);
			reloadButton.setToolTipText("Reload the list of available batch results");
			
			topPanel.add(resultsBox);
			topPanel.add(removeButton);
			topPanel.add(reloadButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.NORTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(topPanel, BorderLayout.NORTH);
			aboutArea = new JTextArea();
			aboutArea.setBorder(BorderFactory.createEmptyBorder());
			aboutArea.setLineWrap(true);
			aboutArea.setWrapStyleWord(true);
			aboutArea.setEditable(false);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(new JScrollPane(aboutArea, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_NEVER), BorderLayout.CENTER);
			resultsBox.addActionListener(this);
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			JPanel closePanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
			closeButton = new JButton("close");
			closeButton.addActionListener(this);
			closePanel.add(closeButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.SOUTH' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(closePanel, BorderLayout.SOUTH);
			batchResultsButton.setIcon(IconLoader.getImageIcon("lorry"));
		}
		
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender.equals(closeButton))
			{
				dispose();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender.equals(resultsBox))
				{
					System.Object item = resultsBox.SelectedItem;
					if (item != null)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
						aboutArea.setText((System.String) batchResults.get(item.ToString()));
					}
					batchResultsButton.setIcon(IconLoader.getImageIcon("lorry"));
				}
				else
				{
					//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
					if (event_sender.equals(removeButton))
					{
						System.Object item = resultsBox.SelectedItem;
						if (item != null)
						{
							resultsBoxModel.removeItem(item);
							batchResults.remove(item);
							aboutArea.setText("");
							resultsBox.setSelectedIndex(- 1);
							// if we removed the last result we hide the indicator button
							if (batchResults.isEmpty())
							{
								batchResultsButton.setVisible(false);
							}
						}
					}
					else
					{
						//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
						if (event_sender.equals(reloadButton))
						{
							resultsBoxModel.setItems(batchResults.keySet().toArray());
							aboutArea.setText("");
							resultsBox.setSelectedIndex(- 1);
						}
					}
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ResultBoxModel' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class ResultBoxModel:DefaultComboBoxModel
		{
			public ResultBoxModel(BatchResultBrowser enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(BatchResultBrowser enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private BatchResultBrowser enclosingInstance;
			private System.Object[] Items
			{
				set
				{
					this.items = value;
					if (value != null)
						fireContentsChanged(this, 0, value.Length);
					else
						fireContentsChanged(this, 0, 0);
				}
				
			}
			virtual public int Size
			{
				get
				{
					if (items == null)
						return 0;
					return items.Length;
				}
				
			}
			public BatchResultBrowser Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private const long serialVersionUID = 1L;
			
			private System.Object[] items;
			
			
			private void  removeItem(System.Object item)
			{
				if (items != null)
				{
					List temp = new LinkedList();
					List litems = (List) item;
					for (int idx = 0; idx < litems.size(); idx++)
					{
						System.Object tmpItem = litems.get(idx);
						if (!tmpItem.Equals(item))
						{
							temp.add(tmpItem);
						}
					}
					items = temp.toArray();
					fireContentsChanged(this, 0, items.Length);
				}
			}
			
			public virtual System.Object getElementAt(int index)
			{
				if (items != null)
				{
					if (index > - 1 && index < items.Length)
					{
						return items[index];
					}
				}
				return null;
			}
			
		}
	}
}