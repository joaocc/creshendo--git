namespace org.jamocha.gui.editor
{
	using System;
	using JFrame = javax.swing.JFrame;
	using Rete = org.jamocha.rete.Rete;
	
	public abstract class AbstractJamochaEditor:JFrame
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassWindowAdapter' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassWindowAdapter
		{
			public AnonymousClassWindowAdapter(AbstractJamochaEditor enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AbstractJamochaEditor enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AbstractJamochaEditor enclosingInstance;
			public AbstractJamochaEditor Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs event_Renamed)
			{
				Enclosing_Instance.close();
			}
		}
		
		protected internal Rete engine;
		
		public AbstractJamochaEditor(Rete engine)
		{
			this.engine = engine;
			this.setTitle("Jamochaeditor");
			this.setSize(600, 400);
			this.setSize(new System.Drawing.Size(600, 400));
			//this.setLocationByPlatform(true);
			this.setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
			this.addWindowListener(new AnonymousClassWindowAdapter(this));
		}
		
		public virtual void  close()
		{
			this.setVisible(false);
			this.dispose();
		}
		
		public abstract void  init();
	}
}