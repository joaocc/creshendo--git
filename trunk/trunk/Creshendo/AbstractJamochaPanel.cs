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
	using JPanel = javax.swing.JPanel;
	using SwingUtilities = javax.swing.SwingUtilities;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	/// <summary> This is an abstract panel that covers all common functions of the panels in
	/// jamocha. Every panel in the tabbedPane should inherit from this class.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// 
	/// </author>
	public abstract class AbstractJamochaPanel:JPanel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassRunnable : IThreadRunnable
		{
			public AnonymousClassRunnable(AbstractJamochaPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AbstractJamochaPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AbstractJamochaPanel enclosingInstance;
			public AbstractJamochaPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: The equivalent of method 'java.lang.Runnable.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			public void  Run()
			{
				Enclosing_Instance.requestFocus();
			}
		}
		
		/// <summary> The JamochaGui Object. We need it to get the engine or other future
		/// purposes.
		/// </summary>
		protected internal JamochaGui gui;
		
		/// <summary> The constructor expecting a JamochaGui as argument.
		/// 
		/// </summary>
		/// <param name="">gui
		/// The active JamocheGui.
		/// 
		/// </param>
		public AbstractJamochaPanel(JamochaGui gui)
		{
			this.gui = gui;
		}
		
		/// <summary> This function is called whenever this Panel gains the focus in the
		/// tabbedPane. A non abstract implementation of this class should override
		/// it and do whatever has to be done when gaining focus. The Shell for
		/// example sets the focus to the textarea and not to itself.
		/// 
		/// </summary>
		public virtual void  setFocus()
		{
			SwingUtilities.invokeLater(new AnonymousClassRunnable(this));
		}
		
		/// <summary> This function is called when the gui is closed and must be implemented by
		/// every class that extends this class. Here all the necessary cleanup
		/// should be done when closing the gui.
		/// 
		/// </summary>
		public abstract void  close();
		
		/// <summary> This function will be called by the gui whenever settings are changed in
		/// the SettingsPanel.
		/// 
		/// </summary>
		public abstract void  settingsChanged();
	}
}