namespace org.jamocha
{
	using System;
	using JamochaGui = org.jamocha.gui.JamochaGui;
	using Rete = org.jamocha.rete.Rete;
	using Shell = org.jamocha.rete.Shell;
	
	public class Jamocha
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassThread:SupportClass.ThreadClass
		{
			public AnonymousClassThread(Jamocha enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jamocha enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jamocha enclosingInstance;
			public Jamocha Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: The equivalent of method 'java.lang.Thread.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			override public void  Run()
			{
				Enclosing_Instance.shell = new Shell(Enclosing_Instance.engine);
				Enclosing_Instance.shell.run();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread1' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		private class AnonymousClassThread1:SupportClass.ThreadClass
		{
			public AnonymousClassThread1(Jamocha enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jamocha enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jamocha enclosingInstance;
			public Jamocha Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: The equivalent of method 'java.lang.Thread.run' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
			override public void  Run()
			{
				Enclosing_Instance.jamochaGui.showGui();
			}
		}
		virtual public JamochaGui JamochaGui
		{
			get
			{
				return jamochaGui;
			}
			
		}
		virtual public Shell Shell
		{
			get
			{
				return shell;
			}
			
		}
		
		private JamochaGui jamochaGui;
		
		private Shell shell;
		
		private Rete engine;
		
		/// <param name="">args
		/// In args can be one or more of the following Strings: -shell:
		/// start the normal Shell with System.in and System.out -gui :
		/// start the graphical user interface for Jamocha with different
		/// tabs and nice, included Shell.
		/// 
		/// </param>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			bool guiStarted = false;
			bool shellStarted = false;
			Jamocha jamocha = new Jamocha(new Rete());
			if (null != args)
			{
				for (int i = 0; i < args.Length; ++i)
				{
					if (args[i].ToUpper().Equals("-gui".ToUpper()))
					{
						jamocha.startGui();
						guiStarted = true;
					}
					else if (args[i].ToUpper().Equals("-shell".ToUpper()))
					{
						jamocha.startShell();
						shellStarted = true;
					}
				}
			}
			// if no arguments were given or by another cause neither gui nor shell
			// were started, we show a usage guide.
			if (!shellStarted && !guiStarted)
			{
				jamocha.showUsage();
			}
			else if (!shellStarted)
			{
				jamocha.JamochaGui.ExitOnClose = true;
			}
		}
		
		internal Jamocha(Rete engine)
		{
			this.engine = engine;
		}
		
		public virtual void  startShell()
		{
			if (shell == null)
			{
				SupportClass.ThreadClass shellThread = new AnonymousClassThread(this);
				shellThread.Start();
			}
		}
		
		public virtual void  startGui()
		{
			if (jamochaGui == null)
			{
				jamochaGui = new JamochaGui(engine);
				SupportClass.ThreadClass guiThread = new AnonymousClassThread1(this);
				guiThread.Start();
			}
		}
		
		public virtual void  showUsage()
		{
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperty_javalangString"'
			System.String sep = System.getProperty("line.separator");
			System.Console.Out.WriteLine("You have to pass one or more of the following arguments:" + sep + sep + "-gui:   starts a graphical user interface." + sep + "-shell: starts a simple Shell.");
			System.Environment.Exit(0);
		}
		
		
	}
}