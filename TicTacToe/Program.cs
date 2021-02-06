using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
	/// <summary>
	/// https://github.com/microsoftarchive/msdn-code-gallery-community-s-z/tree/master/Tic%20Tac%20Toe%20Game%20in%20C%23
	/// </summary>
	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
