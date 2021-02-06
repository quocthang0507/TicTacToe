using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe.GUI
{
	public partial class MainForm : Form
	{
		public static int Width;
		public static int Height;

		private Graphics graphics;
		private Core.Control control;

		public MainForm()
		{
			InitializeComponent();
		}

	}
}
