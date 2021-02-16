using System;
using System.Windows.Forms;
using TicTacToe.GUI;

namespace TicTacToe
{
	/// <summary>
	/// https://hoangphongdhhp.blogspot.com/2016/07/game-co-caro-viet-bang-c.html
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
