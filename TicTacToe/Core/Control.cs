using System;
using System.Collections.Generic;
using System.Drawing;
using TicTacToe.GUI;

namespace TicTacToe.Core
{
	public class Control
	{
		public static Pen Pen;
		public static SolidBrush Black;
		public static SolidBrush White;
		public int Mode { get; set; }
		public bool IsReady { get; set; }

		private readonly Random random = new Random();
		private Board board;
		private Cell[,] array;
		private bool turnOfBlack;
		private Stack<Cell> gone;

		public Control()
		{
			board = new Board(MainForm.Height / Cell.Height, MainForm.Width / Cell.Width);
			Pen = new Pen(Color.DarkKhaki, 1);
			Black = new SolidBrush(Color.Black);
			White = new SolidBrush(Color.White);
			IsReady = false;
			array = new Cell[board.RowNumber, board.ColumnNumber];
		}

		public void DrawBoard(Graphics graphics)
		{
			board.DrawBoard(graphics);
		}

		public void Initialize()
		{
			for (int rowIndex = 0; rowIndex < board.RowNumber; rowIndex++)
			{
				for (int colIndex = 0; colIndex < board.ColumnNumber; colIndex++)
				{
					array[rowIndex, colIndex] = new Cell(rowIndex, colIndex, 0);
				}
			}
		}

		public void GoTo(Graphics graphics, int xPos, int yPos)
		{
			int row = yPos / Cell.Height;
			int column = xPos / Cell.Width;
			// Bỏ qua trường hợp nhấn vào đường kẻ
			if (yPos % Cell.Height != 0 && xPos % Cell.Width != 0)
			{
				// Nếu ô chưa đi
				if (!array[row, column].Gone)
				{
					// Quân đen đi
					if (turnOfBlack)
					{
						board.DrawChessman(graphics, column * Cell.Height, row * Cell.Width, turnOfBlack);
						array[row, column].Gone = true;
					}
				}
			}
		}
	}
}
