using System.Drawing;

namespace TicTacToe.Core
{
	public class Board
	{
		public int RowNumber { get; set; }
		public int ColumnNumber { get; set; }

		private Image O = new Bitmap(Properties.Resources.o);
		private Image X = new Bitmap(Properties.Resources.x);

		public Board()
		{
			RowNumber = 0;
			ColumnNumber = 0;
		}

		public Board(int rowNumber, int columnNumber)
		{
			RowNumber = rowNumber;
			ColumnNumber = columnNumber;
		}

		public void DrawBoard(Graphics graphics)
		{
			for (int colIndex = 0; colIndex < ColumnNumber; colIndex++)
			{
				graphics.DrawLine(Control.Pen, colIndex * Cell.Width, 0, colIndex * Cell.Width, RowNumber * Cell.Height);
			}
			for (int rowIndex = 0; rowIndex < RowNumber; rowIndex++)
			{
				graphics.DrawLine(Control.Pen, 0, rowIndex * Cell.Height, ColumnNumber * Cell.Width, rowIndex * Cell.Height);
			}
		}

		public void DrawChessman(Graphics graphics, int x, int y, bool gone)
		{
			// Black chessman
			if (gone)
			{
				graphics.DrawImage(O, x, y);
			}
			else
			{
				graphics.DrawImage(X, x + 2, y + 2);
			}
		}
	}
}
