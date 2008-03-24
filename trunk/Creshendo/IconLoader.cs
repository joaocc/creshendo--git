namespace org.jamocha.gui.icons
{
	using System;
	using HashMap = java.util.HashMap;
	
	using ImageIcon = javax.swing.ImageIcon;
	
	public class IconLoader
	{
		
		//UPGRADE_NOTE: The initialization of  '_iconCache' was moved to static method 'org.jamocha.gui.icons.IconLoader'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static Map _iconCache;
		
		public static ImageIcon getImageIcon(System.String name)
		{
			return getImageIcon(name, typeof(IconLoader), "png");
		}
		
		public static ImageIcon getImageIcon(System.String name, System.Type clazz)
		{
			return getImageIcon(name, clazz, "png");
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getImageIcon'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027"'
		public static ImageIcon getImageIcon(System.String name, System.Type clazz, System.String extension)
		{
			lock (typeof(org.jamocha.gui.icons.IconLoader))
			{
				ImageIcon icon = (ImageIcon) _iconCache.get(name);
				if (null != icon)
				{
					return icon;
				}
				//UPGRADE_ISSUE: Method 'java.lang.Class.getResource' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangClassgetResource_javalangString"'
				System.Uri url = clazz.getResource("images/" + name + "." + extension);
				if (url != null)
				{
					icon = new ImageIcon(url);
					_iconCache.put(name, icon);
				}
				return icon;
			}
		}
		static IconLoader()
		{
			_iconCache = new HashMap();
		}
	}
}