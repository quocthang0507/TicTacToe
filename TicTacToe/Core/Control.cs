using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
		private Stack<Cell> history;

		public Control()
		{
			board = new Board(MainForm.Height / Cell.Height, MainForm.Width / Cell.Width);
			Pen = new Pen(Color.DarkKhaki, 1);
			Black = new SolidBrush(Color.Black);
			White = new SolidBrush(Color.White);
			IsReady = false;
			array = new Cell[board.RowNumber, board.ColumnNumber];
		}

		/// <summary>
		/// Vẽ bàn cờ
		/// </summary>
		/// <param name="graphics"></param>
		public void DrawBoard(Graphics graphics)
		{
			board.DrawBoard(graphics);
		}

		/// <summary>
		/// Khởi tạo bàn cờ
		/// </summary>
		public void Initialize()
		{
			for (int rowIndex = 0; rowIndex < board.RowNumber; rowIndex++)
			{
				for (int colIndex = 0; colIndex < board.ColumnNumber; colIndex++)
				{
					array[rowIndex, colIndex] = new Cell(rowIndex, colIndex, false);
				}
			}
		}

		/// <summary>
		/// Đi đến vị trí trên bàn cờ
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		public void GoTo(Graphics graphics, int xPos, int yPos)
		{
			int row = yPos / Cell.Height;
			int column = xPos / Cell.Width;
			// Bỏ qua trường hợp nhấn vào đường kẻ
			if (yPos % Cell.Height != 0 && xPos % Cell.Width != 0)
			{
				// Nếu ô chưa đi
				if (!array[row, column].HadGone)
				{
					// Quân đen đi
					if (turnOfBlack)
					{
						board.DrawChessman(graphics, column * Cell.Height, row * Cell.Width, turnOfBlack);
						array[row, column].HadGone = true;

						// Mời quân trắng đi
						turnOfBlack = false;
					}
					// Quân trắng đi
					else
					{
						board.DrawChessman(graphics, column * Cell.Height, row * Cell.Width, turnOfBlack);
						array[row, column].HadGone = true;

						turnOfBlack = true;
					}
					Cell cell = array[row, column].Clone();
					history.Push(cell);
				}
			}
		}

		/// <summary>
		/// Vẽ lại quân cờ
		/// </summary>
		/// <param name="graphics"></param>
		public void RedrawChessman(Graphics graphics)
		{
			if (history.Count != 0)
			{
				foreach (var cell in history)
				{
					board.DrawChessman(graphics, cell.ColIndex * Cell.Width, cell.RowIndex * Cell.Height, cell.HadGone);
				}
			}
		}

		/// <summary>
		/// Chơi với người
		/// </summary>
		/// <param name="graphics"></param>
		public void PlayWithHuman(Graphics graphics)
		{
			Mode = 1;
			turnOfBlack = Convert.ToBoolean(random.Next(0, 2));
			if (turnOfBlack)
			{
				MessageBox.Show("Quân đen đi trước");
			}
			else
			{
				MessageBox.Show("Quân trắng đi trước");
			}
			IsReady = true;
			Initialize();
			history = new Stack<Cell>();
			DrawBoard(graphics);
		}

		/// <summary>
		/// Chơi với máy tính
		/// </summary>
		/// <param name="graphics"></param>
		public void PlayWithComputer(Graphics graphics)
		{
			Mode = 2;
			turnOfBlack = Convert.ToBoolean(random.Next(0, 2));
			if (turnOfBlack)
			{
				MessageBox.Show("Quân đen đi trước");
			}
			else
			{
				MessageBox.Show("Quân trắng đi trước");
			}
			IsReady = true;
			Initialize();
			history = new Stack<Cell>();
			DrawBoard(graphics);
			TurnOfComputer(graphics);
		}

		/// <summary>
		/// Lượt đi của máy
		/// </summary>
		/// <param name="graphics"></param>
		public void TurnOfComputer(Graphics graphics)
		{
			int MaxPoint = 0;
			int DefensePoint = 0;
			int AttackPoint = 0;
			Cell cell = new Cell();

			if (turnOfBlack)
			{
				if (history.Count == 0)
				{
					// Vị trí đánh của máy tính lượt đầu tiên sẽ lấy ngẫu nhiên từ trung tâm với bán kính 3 ô
					GoTo(graphics, random.Next((board.ColumnNumber / 2 - 3) * Cell.Width + 1, (board.ColumnNumber / 2 + 3) * Cell.Width + 1),
						random.Next((board.RowNumber / 2 - 3) * Cell.Height, (board.RowNumber / 2 + 3) * Cell.Height));
				}
				else
				{
					// Thuật toán MinMax tìm điểm cao nhất để đánh
					for (int i = 0; i < board.RowNumber; i++)
					{
						for (int j = 0; j < board.ColumnNumber; j++)
						{
							// Nếu ô tại [i, j] chưa đi và không bị cắt tỉa thì dùng MinMax
							if (!array[i, j].HadGone && !Prune(array[i, j]))
							{
								int centroid;
								AttackPoint = TraversalAttack_Horizontal(i, j) + TravelsalAttack_Vertical(i, j) + TraversalAttack_PriDiagonal(i, j) + TraversalAttack_SecDiagonal(i, j);
								DefensePoint = TraversalDefense_Horizontal(i, j) + TravelsalDefense_Vertical(i, j) + TraversalDefense_PriDiagonal(i, j) + TraversalDefense_SecDiagonal(i, j);

								centroid = DefensePoint > AttackPoint ? DefensePoint : AttackPoint;

								if (MaxPoint < centroid)
								{
									MaxPoint = centroid;
									cell = new Cell(array[i, j].RowIndex, array[i, j].ColIndex, array[i, j].HadGone);
								}
							}
						}
					}
					GoTo(graphics, cell.ColIndex * Cell.Width + 1, cell.RowIndex * Cell.Height + 1);
				}
			}
		}

		#region Alpha–beta pruning
		/// <summary>
		/// Duyệt phòng thủ theo đường chéo phụ
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalDefense_SecDiagonal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt phòng thủ theo đường chéo chính
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalDefense_PriDiagonal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt phòng thủ theo chiều dọc
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TravelsalDefense_Vertical(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt phòng thủ theo chiều ngang
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalDefense_Horizontal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt tấn công theo đường chéo phụ
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalAttack_SecDiagonal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt tấn công theo đường chéo chính
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalAttack_PriDiagonal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt tấn công theo chiều dọc
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TravelsalAttack_Vertical(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Duyệt tấn công theo chiều ngang
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private int TraversalAttack_Horizontal(int i, int j)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Cắt tỉa Alpha–beta pruning
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool Prune(Cell cell)
		{
			if (HorizontallyPrune(cell) && VerticalPrune(cell) && PriDiangonallyPrune(cell) && SecDiangonallyPrune(cell))
				return true;
			return false;
		}

		/// <summary>
		/// Tỉa theo đường chéo phụ
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool SecDiangonallyPrune(Cell cell)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tỉa theo đường chéo chính
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool PriDiangonallyPrune(Cell cell)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tỉa theo chiều dọc
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool VerticalPrune(Cell cell)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tỉa theo chiều ngang
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool HorizontallyPrune(Cell cell)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
