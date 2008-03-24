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
	using LinkedList = java.util.LinkedList;
	using List = java.util.List;
	using BorderFactory = javax.swing.BorderFactory;
	using JButton = javax.swing.JButton;
	using JMenuItem = javax.swing.JMenuItem;
	using JPanel = javax.swing.JPanel;
	using JPopupMenu = javax.swing.JPopupMenu;
	using JScrollPane = javax.swing.JScrollPane;
	using JTextArea = javax.swing.JTextArea;
	using SwingUtilities = javax.swing.SwingUtilities;
	using Timer = javax.swing.Timer;
	using BadLocationException = javax.swing.text.BadLocationException;
	using ClipboardUtil = org.jamocha.gui.ClipboardUtil;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using IconLoader = org.jamocha.gui.icons.IconLoader;
	using MessageEvent = org.jamocha.messagerouter.MessageEvent;
	using StreamChannel = org.jamocha.messagerouter.StreamChannel;
	using Constants = org.jamocha.rete.Constants;
	/// <summary> This class provides a panel with a command line interface to Jamocha.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public class ShellPanel:AbstractJamochaPanel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassThread:SupportClass.ThreadClass
		{
			public AnonymousClassThread(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: The equivalent of method 'java.lang.Thread.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'run'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
			/// <summary> Simply runs the ChannelListener and lets it process Events.
			/// </summary>
			override public void  Run()
			{
				lock (this)
				{
					List msgEvents = new ArrayList();
					bool printPrompt = false;
					
					while (Enclosing_Instance.running)
					{
						Enclosing_Instance.channel.fillEventList(msgEvents);
						if (!msgEvents.isEmpty())
						{
							Enclosing_Instance.stopTimer();
							System.Text.StringBuilder buffer = new System.Text.StringBuilder();
							for (int idx = 0; idx < msgEvents.size(); idx++)
							{
								MessageEvent event_Renamed = (MessageEvent) msgEvents.get(idx);
								if (event_Renamed.Type == MessageEvent.PARSE_ERROR || event_Renamed.Type == MessageEvent.ERROR || event_Renamed.Type == MessageEvent.RESULT)
								{
									
									printPrompt = true;
									Enclosing_Instance.lastIncompleteCommand = new System.Text.StringBuilder();
								}
								if (event_Renamed.Type == MessageEvent.ERROR)
								{
									//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
									buffer.Append(exceptionToString((System.Exception) event_Renamed.Message).Trim() + System.getProperty("line.separator"));
								}
								//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
								if (event_Renamed.Type != MessageEvent.COMMAND && !event_Renamed.Message.ToString().Equals("") && !event_Renamed.Message.Equals(Constants.NIL_SYMBOL))
								{
									//UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
									//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
									buffer.Append(event_Renamed.Message.ToString().Trim() + System.getProperty("line.separator"));
								}
							}
							msgEvents.clear();
							Enclosing_Instance.hideCursor();
							Enclosing_Instance.printMessage(buffer.ToString().Trim(), true);
							if (printPrompt)
							{
								Enclosing_Instance.printPrompt();
								Enclosing_Instance.moveCursorTo(Enclosing_Instance.lastPromptIndex);
							}
							Enclosing_Instance.showCursor();
							printPrompt = false;
							Enclosing_Instance.startTimer();
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
					try
					{
						Enclosing_Instance.outWriter.Close();
					}
					catch (System.IO.IOException e)
					{
						// we silently ignore it
						SupportClass.WriteStackTrace(e, Console.Error);
					}
					Enclosing_Instance.gui.Engine.MessageRouter.closeChannel(Enclosing_Instance.channel);
				}
			}
			
			/// <summary> Converts an Exception to a String namely turns the StackTrace to
			/// a String.
			/// 
			/// </summary>
			/// <param name="">exception
			/// The Exception
			/// </param>
			/// <returns> A nice String representation of the Exception
			/// 
			/// </returns>
			private System.String exceptionToString(System.Exception exception)
			{
				System.Text.StringBuilder res = new System.Text.StringBuilder();
				StackTraceElement[] str = exception.StackTrace;
				for (int i = 0; i < str.length; ++i)
				{
					//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
					res.Append(str[i] + System.getProperty("line.separator"));
				}
				return res.ToString();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassKeyAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassKeyAdapter
		{
			public AnonymousClassKeyAdapter(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: Class java.awt.event.KeyAdapter was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
			public void  keyPressed(System.Object event_sender, System.Windows.Forms.KeyPressEventArgs e)
			{
				Enclosing_Instance.keyEventQueue.add(e);
				//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.consume' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
				e.consume();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread1' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassThread1:SupportClass.ThreadClass
		{
			public AnonymousClassThread1(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: The equivalent of method 'java.lang.Thread.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'run'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
			override public void  Run()
			{
				lock (this)
				{
					while (Enclosing_Instance.running)
					{
						if (!Enclosing_Instance.keyEventQueue.isEmpty())
						{
							System.Windows.Forms.KeyEventArgs e = (System.Windows.Forms.KeyEventArgs) Enclosing_Instance.keyEventQueue.remove(0);
							int delta = 1;
							//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyChar' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawteventKeyEventgetKeyCode"'
							switch ((int) e.KeyChar)
							{
								
								case (int) System.Windows.Forms.Keys.Down: 
								case System.Windows.Forms.KeyEventArgs.VK_KP_DOWN: 
									delta = - 1;
									goto case (int) System.Windows.Forms.Keys.Up;
								
								case (int) System.Windows.Forms.Keys.Up: 
								case System.Windows.Forms.KeyEventArgs.VK_KP_UP: 
									Enclosing_Instance.stopTimer();
									Enclosing_Instance.hideCursor();
									// Here we walk through the history
									Enclosing_Instance.history_offset += delta;
									if (Enclosing_Instance.history_offset <= 0)
									{
										Enclosing_Instance.history_offset = 0;
										if (Enclosing_Instance.lastPromptIndex < Enclosing_Instance.Offset)
										{
											Enclosing_Instance.removeLine();
										}
									}
									else
									{
										if (Enclosing_Instance.history_offset > Enclosing_Instance.history.size())
										{
											Enclosing_Instance.history_offset = Enclosing_Instance.history.size();
										}
										if (Enclosing_Instance.lastPromptIndex < Enclosing_Instance.Offset)
										{
											Enclosing_Instance.removeLine();
										}
										int index = Enclosing_Instance.history.size() - Enclosing_Instance.history_offset;
										if (index >= 0 && Enclosing_Instance.history.size() > 0)
										{
											System.String tmp = (System.String) Enclosing_Instance.history.get(index);
											Enclosing_Instance.printMessage(tmp, false);
										}
									}
									Enclosing_Instance.moveCursorToEnd();
									Enclosing_Instance.startTimer();
									break;
								
								case (int) System.Windows.Forms.Keys.Enter: 
									Enclosing_Instance.stopTimer();
									Enclosing_Instance.hideCursor();
									if (Enclosing_Instance.lastPromptIndex < Enclosing_Instance.Offset)
									{
										System.String currLine = "";
										try
										{
											try
											{
												currLine = Enclosing_Instance.outputArea.getText(Enclosing_Instance.lastPromptIndex, Enclosing_Instance.Offset - Enclosing_Instance.lastPromptIndex);
											}
											catch (BadLocationException e1)
											{
												e1.printStackTrace();
											}
											//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
											Enclosing_Instance.lastIncompleteCommand.Append(currLine + System.getProperty("line.separator"));
											if (currLine.Length > 0)
											{
												Enclosing_Instance.addToHistory(currLine);
												Enclosing_Instance.outWriter.Write(currLine);
												Enclosing_Instance.outWriter.Flush();
											}
										}
										catch (System.IO.IOException e1)
										{
											SupportClass.WriteStackTrace(e1, Console.Error);
										}
									}
									Enclosing_Instance.printMessage("", true);
									Enclosing_Instance.moveCursorToEnd();
									Enclosing_Instance.startTimer();
									break;
									// delete a char on the left side of the cursor
								
								case (int) System.Windows.Forms.Keys.Back: 
									Enclosing_Instance.stopTimer();
									Enclosing_Instance.hideCursor();
									if (Enclosing_Instance.cursorPosition > Enclosing_Instance.lastPromptIndex)
									{
										Enclosing_Instance.removeCharLeft();
									}
									Enclosing_Instance.showCursor();
									Enclosing_Instance.startTimer();
									break;
									// delete a char on the right side of the cursor
								
								case (int) System.Windows.Forms.Keys.Delete: 
									Enclosing_Instance.stopTimer();
									Enclosing_Instance.hideCursor();
									if (Enclosing_Instance.cursorPosition < Enclosing_Instance.Offset)
									{
										Enclosing_Instance.removeCharRight();
									}
									Enclosing_Instance.showCursor();
									Enclosing_Instance.startTimer();
									break;
									// Moving the Cursor in the current line
								
								case (int) System.Windows.Forms.Keys.Right: 
								case System.Windows.Forms.KeyEventArgs.VK_KP_RIGHT: 
									//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.isShiftDown' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
									if (!e.isShiftDown())
									{
										Enclosing_Instance.stopTimer();
										Enclosing_Instance.hideCursor();
										if (Enclosing_Instance.cursorPosition < Enclosing_Instance.Offset)
										{
											Enclosing_Instance.moveCursorTo(Enclosing_Instance.cursorPosition + 1);
										}
										Enclosing_Instance.showCursor();
										Enclosing_Instance.startTimer();
									}
									break;
								
								
								case (int) System.Windows.Forms.Keys.Left: 
								case System.Windows.Forms.KeyEventArgs.VK_KP_LEFT: 
									//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.isShiftDown' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
									if (!e.isShiftDown())
									{
										Enclosing_Instance.stopTimer();
										Enclosing_Instance.hideCursor();
										if (Enclosing_Instance.cursorPosition > Enclosing_Instance.lastPromptIndex)
										{
											Enclosing_Instance.moveCursorTo(Enclosing_Instance.cursorPosition - 1);
										}
										Enclosing_Instance.showCursor();
										Enclosing_Instance.startTimer();
									}
									break;
									// ignore special keys
								
								case (int) System.Windows.Forms.Keys.Alt: 
								case (int) System.Windows.Forms.Keys.Control: 
								//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.VK_META' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventKeyEventVK_META_f"'
								case KeyEvent.VK_META: 
								case (int) System.Windows.Forms.Keys.Shift: 
									break;
								
								default: 
									//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.isControlDown' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
									//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.isMetaDown' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawteventInputEvent"'
									if (!e.isControlDown() && !e.isMetaDown())
									{
										// simple character
										Enclosing_Instance.printMessage(e.KeyChar.ToString(), false);
									}
									else
									{
										// paste from clipboard
										//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyChar' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawteventKeyEventgetKeyCode"'
										if (e.KeyChar == 'v' || (int) e.KeyChar == (int) System.Windows.Forms.Keys.V)
										{
											System.String clipContent = ClipboardUtil.Instance.ClipboardContents;
											if (clipContent != null)
											{
												Enclosing_Instance.printMessage(clipContent, false);
											}
										}
										// copy to clipboard
										else
										{
											//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyChar' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawteventKeyEventgetKeyCode"'
											if (e.KeyChar == 'c' || (int) e.KeyChar == (int) System.Windows.Forms.Keys.C)
											{
												ClipboardUtil.Instance.setClipboardContents(Enclosing_Instance.outputArea.SelectedText);
											}
										}
									}
									break;
								
							}
							//e.consume();
							Enclosing_Instance.startTimer();
						}
						else
						{
							try
							{
								System.Threading.Thread.Sleep(new System.TimeSpan(10000 * 20));
							}
							catch (System.Threading.ThreadInterruptedException e)
							{
								// ignored
							}
						}
					}
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter
		{
			public AnonymousClassMouseAdapter(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				ClipboardUtil.Instance.setClipboardContents(Enclosing_Instance.outputArea.SelectedText);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter1' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter1
		{
			public AnonymousClassMouseAdapter1(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				System.String clipContent = ClipboardUtil.Instance.ClipboardContents;
				if (clipContent != null)
				{
					Enclosing_Instance.printMessage(clipContent, false);
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter2' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter2
		{
			public AnonymousClassMouseAdapter2(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				Enclosing_Instance.outputArea.setSelectionStart(Enclosing_Instance.lastPromptIndex);
				Enclosing_Instance.outputArea.setSelectionEnd(Enclosing_Instance.Offset);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter3' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassMouseAdapter3
		{
			public AnonymousClassMouseAdapter3(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs event_Renamed)
			{
				Enclosing_Instance.outputArea.setSelectionStart(0);
				Enclosing_Instance.outputArea.setSelectionEnd(Enclosing_Instance.Offset);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassRunnable : IThreadRunnable
		{
			public AnonymousClassRunnable(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: The equivalent of method 'java.lang.Runnable.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			public void  Run()
			{
				Enclosing_Instance.showCursor();
				Enclosing_Instance.startTimer();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable1' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassRunnable1 : IThreadRunnable
		{
			public AnonymousClassRunnable1(ShellPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ShellPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ShellPanel enclosingInstance;
			public ShellPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: The equivalent of method 'java.lang.Runnable.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			public void  Run()
			{
				Enclosing_Instance.stopTimer();
				Enclosing_Instance.hideCursor();
			}
		}
		private void  InitBlock()
		{
			keyEventQueue = new ArrayList();
			history = new LinkedList();
			cursorTimer = new Timer(400, this);
			lastIncompleteCommand = new System.Text.StringBuilder();
		}
		/// <summary> Returns the offset of the downmost line in the outputArea.
		/// 
		/// </summary>
		/// <returns> The total Offset.
		/// 
		/// </returns>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getOffset'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		private int Offset
		{
			get
			{
				lock (this)
				{
					try
					{
						return outputArea.getLineEndOffset(outputArea.LineCount - 1);
					}
					catch (BadLocationException e1)
					{
						// This should never happen as we have at least one line
					}
					return 0;
				}
			}
			
		}
		
		private const long serialVersionUID = 1777454004380892575L;
		
		/// <summary> Flag for the ChannelListener and the eventThread to know if they should
		/// go on working.
		/// </summary>
		private bool running = true;
		
		/// <summary> The Queue for incoming KeyEvents. We process them in an own Thread to
		/// prevent strange, concurrent behaviours.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'keyEventQueue' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private List keyEventQueue;
		
		/// <summary> The Area to display our keypresses and results from the engine.
		/// </summary>
		private JTextArea outputArea;
		
		/// <summary> The scrollpane for the outputArea.
		/// </summary>
		private JScrollPane scrollPane;
		
		/// <summary> The Button that clears the console window.
		/// </summary>
		private JButton clearButton;
		
		/// <summary> The current, temporary offset inside the history.
		/// </summary>
		private int history_offset = 0;
		
		/// <summary> The maximum size of the history. The boundary is to prevent memory
		/// problems. A history of less than zero means unbounded history.
		/// </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'history_max_size '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
		private int history_max_size = 100;
		
		/// <summary> A history limited to history_max_size entries.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'history' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private List history;
		
		/// <summary> The last position of the Prompt or in case of a new line the position of
		/// the beginning of the new line.
		/// </summary>
		private int lastPromptIndex = 0;
		
		/// <summary> The Symbol for the blinking Cursor.
		/// </summary>
		private const System.String SHELL_CURSOR = "_";
		
		/// <summary> The current position of the Cursor inside of the outputArea.
		/// </summary>
		private int cursorPosition = 0;
		
		/// <summary> The String "below" the Cursor at the current Position.
		/// </summary>
		private System.String cursorSubString = "";
		
		/// <summary> The Timer that makes the Cursor blinking. Set a smaller delay (first
		/// parameter) to let it blink faster.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'cursorTimer' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private Timer cursorTimer;
		
		/// <summary> A Flag indicating if the Shell currently shows the Cursor or the
		/// cursorSubString.
		/// </summary>
		private bool cursorShowing = false;
		
		private int lastScrollBarPosition = 0;
		
		/// <summary> The Shells channel to the Rete engine.
		/// </summary>
		private StreamChannel channel;
		
		/// <summary> The Writer we use to write to the channel via a PipedIn(Out)putStream.
		/// </summary>
		private System.IO.StreamWriter outWriter;
		
		/// <summary> A Buffer for the last command, that wasn't send to the engine in total.
		/// </summary>
		//UPGRADE_NOTE: The initialization of  'lastIncompleteCommand' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private System.Text.StringBuilder lastIncompleteCommand;
		
		/// <summary> The Thread listening for results from the Rete engine on our channel.
		/// </summary>
		private SupportClass.ThreadClass channelListener;
		
		/// <summary> The main constructor for a ShellPanel.
		/// 
		/// </summary>
		/// <param name="">engine
		/// The Jamocha engine that should be used with this GUI.
		/// 
		/// </param>
		public ShellPanel(JamochaGui gui):base(gui)
		{
			InitBlock();
			// GUI construction
			// create the output area
			outputArea = new JTextArea();
			outputArea.setEditable(false);
			outputArea.setLineWrap(true);
			outputArea.setWrapStyleWord(true);
			outputArea.addFocusListener(this);
			// set the font and the colors
			settingsChanged();
			this.addFocusListener(this);
			// create a scroll pane to embedd the output area
			scrollPane = new JScrollPane(outputArea, JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED, JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
			scrollPane.VerticalScrollBar.addAdjustmentListener(this);
			// Assemble the GUI
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			setLayout(new BorderLayout());
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout.CENTER' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtBorderLayout"'
			add(scrollPane, BorderLayout.CENTER);
			
			// create the button that clears the output area
			clearButton = new JButton("Clear Shell", IconLoader.getImageIcon("application_osx_terminal"));
			clearButton.addActionListener(this);
			JPanel clearButtonPanel = new JPanel();
			//UPGRADE_ISSUE: Constructor 'java.awt.FlowLayout.FlowLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			//UPGRADE_ISSUE: Field 'java.awt.FlowLayout.RIGHT' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFlowLayout"'
			clearButtonPanel.setLayout(new FlowLayout(FlowLayout.RIGHT, 5, 1));
			clearButtonPanel.add(clearButton);
			//UPGRADE_ISSUE: Field 'java.awt.BorderLayout' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000"'
			add(clearButtonPanel, BorderLayout.PAGE_END);
			
			// initialize the channel to the engine
			//UPGRADE_ISSUE: Constructor 'java.io.PipedOutputStream.PipedOutputStream' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaioPipedOutputStreamPipedOutputStream"'
			System.IO.StreamWriter outStream = new PipedOutputStream();
			//UPGRADE_ISSUE: Constructor 'java.io.PipedInputStream.PipedInputStream' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaioPipedInputStreamPipedInputStream"'
			System.IO.StreamReader inStream = new PipedInputStream();
			outWriter = new System.IO.StreamWriter(outStream);
			try
			{
				//UPGRADE_ISSUE: Method 'java.io.PipedInputStream.connect' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaioPipedInputStreamconnect_javaioPipedOutputStream"'
				inStream.connect(outStream);
			}
			catch (System.IO.IOException e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				System.Environment.Exit(1);
			}
			channel = gui.Engine.MessageRouter.openChannel("JamochaGui", inStream);
			
			printPrompt();
			moveCursorToEnd();
			startTimer();
			
			// initialize the channellistener for outputs from the engine
			initChannelListener();
			
			// initialize the keylistener for key events
			initKeyListener();
			
			// initialize the mouselistener for the context menu
			initPopupMenu();
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'printPrompt'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Prints the prompt in the outputArea.
		/// 
		/// </summary>
		private void  printPrompt()
		{
			lock (this)
			{
				outputArea.append(Constants.SHELL_PROMPT);
				lastPromptIndex = Offset;
				outputArea.setCaretPosition(outputArea.Document.Length);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'printMessage'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Prints a messge at the current cursorPosition in the outputArea.
		/// 
		/// </summary>
		/// <param name="">message
		/// The message to print.
		/// </param>
		/// <param name="">lineBreak
		/// If true a linebreak is added at the end.
		/// 
		/// </param>
		private void  printMessage(System.String message, bool lineBreak)
		{
			lock (this)
			{
				if (lineBreak)
				{
					//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
					message += System.getProperty("line.separator");
				}
				outputArea.insert(message, cursorPosition);
				
				cursorPosition = cursorPosition + message.Length;
				if (lineBreak)
				{
					lastPromptIndex = Offset;
				}
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'moveCursorToEnd'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Moves the Cursor to the End of the outputArea.
		/// 
		/// </summary>
		private void  moveCursorToEnd()
		{
			lock (this)
			{
				moveCursorTo(Offset);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'moveCursorTo'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Moves the Cursor to the specified Position.
		/// 
		/// </summary>
		/// <param name="">newPosition
		/// The new Position for the Cursor.s
		/// 
		/// </param>
		private void  moveCursorTo(int newPosition)
		{
			lock (this)
			{
				hideCursor();
				cursorPosition = newPosition;
				showCursor();
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'showCursor'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Shows the Cursor if it was previously hiding and otherwise does nothing.
		/// 
		/// </summary>
		private void  showCursor()
		{
			lock (this)
			{
				if (!cursorShowing)
				{
					int currOffset = Offset;
					if (currOffset >= (cursorPosition + SHELL_CURSOR.Length))
					{
						try
						{
							cursorSubString = outputArea.getText(cursorPosition, SHELL_CURSOR.Length);
						}
						catch (BadLocationException e)
						{
							// Shouldn't happen
							e.printStackTrace();
						}
						outputArea.replaceRange(SHELL_CURSOR, cursorPosition, cursorPosition + cursorSubString.Length);
					}
					else
					{
						int offset = currOffset - cursorPosition;
						if (offset < 0)
							offset = 0;
						try
						{
							cursorSubString = outputArea.getText(cursorPosition, offset);
						}
						catch (BadLocationException e)
						{
							// Shouldn't happen
							e.printStackTrace();
						}
						outputArea.replaceRange(SHELL_CURSOR, cursorPosition, cursorPosition + offset);
					}
				}
				cursorShowing = true;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'hideCursor'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Hide the Cursor if it was previously visible and otherwise does nothing.
		/// 
		/// </summary>
		private void  hideCursor()
		{
			lock (this)
			{
				if (cursorShowing)
				{
					try
					{
						outputArea.replaceRange(cursorSubString, cursorPosition, cursorPosition + SHELL_CURSOR.Length);
					}
					catch (System.Exception e)
					{
						SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
				cursorShowing = false;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'removeCharLeft'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Removes a Character on the left side of the Cursor.
		/// 
		/// </summary>
		private void  removeCharLeft()
		{
			lock (this)
			{
				outputArea.replaceRange("", cursorPosition - 1, cursorPosition);
				cursorPosition--;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'removeCharRight'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Removes a Character on the right side (below) of the Cursor.
		/// 
		/// </summary>
		private void  removeCharRight()
		{
			lock (this)
			{
				outputArea.replaceRange("", cursorPosition, cursorPosition + 1);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'removeLine'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Removes the current line completely.
		/// 
		/// </summary>
		private void  removeLine()
		{
			lock (this)
			{
				outputArea.replaceRange("", lastPromptIndex, Offset);
				cursorPosition = Offset;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'startTimer'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Starts the cursorTimer.
		/// 
		/// </summary>
		private void  startTimer()
		{
			lock (this)
			{
				if (!cursorTimer.isRunning())
					cursorTimer.start();
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'stopTimer'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Pauses the cursorTimer.
		/// 
		/// </summary>
		private void  stopTimer()
		{
			lock (this)
			{
				cursorTimer.stop();
			}
		}
		
		/// <summary> Initializes and starts the ChannelListener that waits for results or
		/// errors from the Jamocha engine.
		/// 
		/// </summary>
		private void  initChannelListener()
		{
			channelListener = new AnonymousClassThread(this);
			channelListener.Start();
		}
		
		/// <summary> Initializes the KeyListener to catch all Keypresses.
		/// 
		/// </summary>
		private void  initKeyListener()
		{
			KeyAdapter adapter = new AnonymousClassKeyAdapter(this);
			addKeyListener(adapter);
			outputArea.addKeyListener(adapter);
			
			SupportClass.ThreadClass eventThread = new AnonymousClassThread1(this);
			eventThread.Start();
		}
		
		/// <summary> A function that safely adds single lines to the history. If a String
		/// contains more than one line it will be splitted line by line.
		/// 
		/// If the history size is greater than history_max_size we remove elements
		/// that are old and too much.
		/// 
		/// </summary>
		/// <param name="">historyString
		/// The String that should be added to the history.
		/// 
		/// </param>
		private void  addToHistory(System.String historyString)
		{
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
			System.String[] lines = historyString.split(System.getProperty("line.separator"));
			for (int i = 0; i < lines.Length; ++i)
			{
				if (!lines[i].Equals(""))
				{
					history.add(lines[i]);
				}
			}
			// remove items as long as there are too much of them
			// and a valid history_max_size is set
			while (history.size() > history_max_size && history_max_size >= 0)
			{
				history.remove(0);
			}
			// reset the history index after a command
			history_offset = 0;
		}
		
		/// <summary> initializing the contextmenu.
		/// 
		/// </summary>
		private void  initPopupMenu()
		{
			
			JPopupMenu menu = new JPopupMenu();
			JMenuItem copyMenu = new JMenuItem("Copy", IconLoader.getImageIcon("page_copy"));
			copyMenu.addMouseListener(new AnonymousClassMouseAdapter(this));
			JMenuItem pasteMenu = new JMenuItem("Paste", IconLoader.getImageIcon("paste_plain"));
			pasteMenu.addMouseListener(new AnonymousClassMouseAdapter1(this));
			JMenuItem selectCommandMenu = new JMenuItem("Select current line");
			selectCommandMenu.addMouseListener(new AnonymousClassMouseAdapter2(this));
			JMenuItem selectAllMenu = new JMenuItem("Select all");
			selectAllMenu.addMouseListener(new AnonymousClassMouseAdapter3(this));
			menu.add(copyMenu);
			menu.add(pasteMenu);
			menu.addSeparator();
			menu.add(selectCommandMenu);
			menu.add(selectAllMenu);
			// outputArea.setComponentPopupMenu(menu);
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'clearArea'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		/// <summary> Clears the outputArea and sets a new prompt. If parts of a command are
		/// already in the channel these commandparts will be printed again.
		/// 
		/// </summary>
		private void  clearArea()
		{
			lock (this)
			{
				stopTimer();
				hideCursor();
				outputArea.setText("");
				lastPromptIndex = 0;
				cursorPosition = 0;
				lastScrollBarPosition = 0;
				if (lastIncompleteCommand.Length > 0)
				{
					printPrompt();
					cursorPosition = Offset;
					printMessage(lastIncompleteCommand.ToString().Trim(), true);
				}
				else
					printPrompt();
				moveCursorToEnd();
				setFocus();
				startTimer();
			}
		}
		
		
		/// <summary> Sets the focus of this panel and by this sets the focus to the outputArea
		/// so that the user doesn't have to click on it before he can start typing.
		/// 
		/// </summary>
		public override void  setFocus()
		{
			base.setFocus();
		}
		
		/// <summary> Close this Panel.
		/// 
		/// </summary>
		public override void  close()
		{
			stopTimer();
			running = false;
		}
		
		public override void  settingsChanged()
		{
			//UPGRADE_TODO: Method 'java.awt.Font.Plain' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtFontPLAIN_f"'
			outputArea.setFont(new Font(gui.Preferences.get("shell.font", "Courier"), gui.Preferences.getInt("shell.fontstyle", (int) System.Drawing.FontStyle.Regular), gui.Preferences.getInt("shell.fontsize", 12)));
			outputArea.setBackground(new Color(gui.Preferences.getInt("shell.backgroundcolor", System.Drawing.Color.BLACK.RGB)));
			outputArea.setForeground(new Color(gui.Preferences.getInt("shell.fontcolor", System.Drawing.Color.WHITE.RGB)));
			outputArea.setBorder(BorderFactory.createLineBorder(outputArea.Background, 2));
		}
		
		/// <summary> Catches events for Buttons and the Timer in this Panel.
		/// </summary>
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender.equals(clearButton))
			{
				clearArea();
			}
			else
			{
				//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
				if (event_sender.equals(cursorTimer))
				{
					if (cursorShowing)
					{
						hideCursor();
					}
					else
					{
						showCursor();
					}
				}
			}
		}
		
		public virtual void  focusGained(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == this || event_sender == outputArea)
			{
				SwingUtilities.invokeLater(new AnonymousClassRunnable(this));
			}
		}
		
		public virtual void  focusLost(System.Object event_sender, System.EventArgs event_Renamed)
		{
			//UPGRADE_NOTE: The method 'java.util.EventObject.getSource' needs to be in a event handling method in order to be properly converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1171"'
			if (event_sender == this || event_sender == outputArea)
			{
				SwingUtilities.invokeLater(new AnonymousClassRunnable1(this));
			}
		}
		
		public virtual void  adjustmentValueChanged(System.Object event_sender, System.Windows.Forms.ScrollEventArgs event_Renamed)
		{
			if (event_Renamed.NewValue < lastScrollBarPosition)
				stopTimer();
			else
			{
				lastScrollBarPosition = event_Renamed.NewValue;
				startTimer();
			}
		}
	}
}