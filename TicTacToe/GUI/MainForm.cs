using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe.GUI
{
	public partial class MainForm : Form
	{
		public static new int Width;
		public static new int Height;
		private Graphics graphics;
		private Core.Control control;

		public MainForm()
		{
			InitializeComponent();
			// Tạo đối tượng Graphic từ panel
			graphics = panelBoard.CreateGraphics();

			// Lấy kích thước panel để vẽ bàn cờ
			Width = panelBoard.Width;
			Height = panelBoard.Height;

			// Khởi tạo đối tượng Control
			control = new Core.Control();

			// Thêm sự kiện click menu
			playWithComputerToolStripMenuItem.Click += PlayWithComputerToolStripMenuItem_Click;
			playWithHumanToolStripMenuItem.Click += PlayWithHumanToolStripMenuItem_Click;
		}

		private void PlayWithHumanToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			control.PlayWithHuman(graphics);
			graphics.Clear(panelBoard.BackColor);
			Image image = new Bitmap(Properties.Resources.background_1);
			panelBoard.BackgroundImage = image;
		}

		private void PlayWithComputerToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			control.PlayWithComputer(graphics);
			graphics.Clear(panelBoard.BackColor);
			Image image = new Bitmap(Properties.Resources.background);
			panelBoard.BackgroundImage = image;

		}

		private void panelBoard_Paint(object sender, PaintEventArgs e)
		{
			if (control.IsReady)
			{
				control.DrawBoard(graphics);
				control.RedrawChessman(graphics);
			}
		}

		private void panelBoard_MouseClick(object sender, MouseEventArgs e)
		{
			if (control.IsReady)
			{
				// Chơi với người
				if (control.Mode == 1)
				{
					control.GoTo(graphics, e.Location.X, e.Location.Y);
					control.FindWinner(graphics);
				}
				// Chơi với máy
				else
				{
					control.GoTo(graphics, e.Location.X, e.Location.Y);
					if (!control.FindWinner(graphics))
					{
						control.TurnOfComputer(graphics);
						control.FindWinner(graphics);
					}
				}
			}
		}
	}
}
