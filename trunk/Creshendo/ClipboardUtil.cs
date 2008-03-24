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
	/// <summary> 
	/// A Class for Clipboard access.
	/// 
	/// </summary>
	/// <author>  Karl-Heinz Krempels <krempels@cs.rwth-aachen.de>
	/// </author>
	/// <author>  Alexander Wilden <october.rust@gmx.de>
	/// </author>
	/// <version>  0.01
	/// 
	/// </version>
	//UPGRADE_ISSUE: Interface 'java.awt.datatransfer.ClipboardOwner' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtdatatransferClipboardOwner"'
	public class ClipboardUtil : ClipboardOwner
	{
		/// <summary> Returns the single ClipboardUtil instance.
		/// 
		/// </summary>
		/// <returns> The Clipboard singleton object.
		/// 
		/// </returns>
		public static ClipboardUtil Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ClipboardUtil();
				return _instance;
			}
			
		}
		/// <summary> Get the String residing on the clipboard.
		/// 
		/// </summary>
		/// <returns> any text found on the Clipboard. If none found, return null.
		/// 
		/// </returns>
		/// <summary> Place a String on the clipboard and make this class the owner of the
		/// Clipboard's contents.
		/// </summary>
		virtual public System.String ClipboardContents
		{
			get
			{
				System.String result = "";
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getSystemClipboard' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit"'
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit"'
				System.Windows.Forms.Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();
				System.Windows.Forms.IDataObject contents = System.Windows.Forms.Clipboard.GetDataObject();
				//UPGRADE_ISSUE: Method 'java.awt.datatransfer.Transferable.isDataFlavorSupported' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtdatatransferTransferableisDataFlavorSupported_javaawtdatatransferDataFlavor"'
				//UPGRADE_TODO: Field 'java.awt.datatransfer.DataFlavor.stringFlavor' was converted to 'System.Windows.Forms.DataFormats.StringFormat' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				bool hasTransferableText = (contents != null) && contents.isDataFlavorSupported(System.Windows.Forms.DataFormats.StringFormat);
				if (hasTransferableText)
				{
					try
					{
						//UPGRADE_TODO: Field 'java.awt.datatransfer.DataFlavor.stringFlavor' was converted to 'System.Windows.Forms.DataFormats.StringFormat' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
						result = (System.String) contents.GetData(System.Windows.Forms.DataFormats.StringFormat.ToString());
					}
					catch (System.Exception ex)
					{
						SupportClass.WriteStackTrace(ex, Console.Error);
					}
					catch (System.IO.IOException ex)
					{
						SupportClass.WriteStackTrace(ex, Console.Error);
					}
				}
				return result;
			}
			
			set
			{
				System.Windows.Forms.DataObject stringSelection = new System.Windows.Forms.DataObject(value);
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getSystemClipboard' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit"'
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit"'
				System.Windows.Forms.Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();
				System.Windows.Forms.Clipboard.SetDataObject(stringSelection);
			}
			
		}
		
		/// <summary> The singleton ClipboardUtil.
		/// </summary>
		private static ClipboardUtil _instance = null;
		
		/// <summary> A private Constructur so that we can use singletons.
		/// 
		/// </summary>
		private ClipboardUtil()
		{
		}
		
		
		//UPGRADE_TODO: The equivalent of method 'java.awt.datatransfer.ClipboardOwner.lostOwnership' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
		/// <summary> Called on lost ownership.
		/// </summary>
		public void  lostOwnership(System.Windows.Forms.Clipboard aClipboard, System.Windows.Forms.IDataObject aContents)
		{
			// do nothing
		}
		
		
	}
}