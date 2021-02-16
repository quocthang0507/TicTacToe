using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.GUI;

namespace TicTacToe.Core
{
	public class Control
	{
		public int Mode { get; set; }
		public bool IsReady { get; set; }
		public static Pen Pen { get; set; }

		private readonly Random random = new Random();
		private readonly Board board;
		private readonly Cell[,] array;
		private bool turnOfX;
		private Stack<Cell> history;
		private readonly int[] ArrayOfAttackPoints = new int[7] { 0, 4, 25, 246, 7300, 6561, 59049 };
		private readonly int[] ArrayOfDefensePoints = new int[7] { 0, 3, 24, 243, 2197, 19773, 177957 };

		public Control()
		{
			board = new Board(MainForm.HeightOfPanel / Cell.Height, MainForm.WidthOfPanel / Cell.Width);
			Pen = new Pen(Color.DarkKhaki, 1);
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
					array[rowIndex, colIndex] = new Cell(rowIndex, colIndex, Gone.None);
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
				if (array[row, column].HadGone == Gone.None)
				{
					// Quân x đi
					if (turnOfX)
					{
						board.DrawChessman(graphics, column * Cell.Height, row * Cell.Width, Gone.X);
						array[row, column].HadGone = Gone.X;

						// Mời quân o đi
						turnOfX = false;
					}
					// Quân o đi
					else
					{
						board.DrawChessman(graphics, column * Cell.Height, row * Cell.Width, Gone.O);
						array[row, column].HadGone = Gone.O;

						turnOfX = true;
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
				foreach (Cell cell in history)
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
			turnOfX = Convert.ToBoolean(random.Next(0, 2));
			if (turnOfX)
			{
				MessageBox.Show("X đi trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("O đi trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			turnOfX = Convert.ToBoolean(random.Next(0, 2));
			if (turnOfX)
			{
				MessageBox.Show("X đi trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("O đi trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			int DefensePoint;
			int AttackPoint;
			Cell cell = new Cell();

			if (turnOfX)
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
							if (array[i, j].HadGone == Gone.None && !Prune(array[i, j]))
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
			// Duyệt từ trên xuống
			if (cell.RowIndex <= board.RowNumber - 5 && cell.ColIndex <= board.ColumnNumber - 5)
				for (int i = 1; i <= 4; i++)
					if (array[cell.RowIndex + i, cell.ColIndex + i].HadGone != Gone.None)
						return false;
			// Duyệt từ dưới lên
			if (cell.ColIndex >= 4 && cell.RowIndex >= 4)
				for (int i = 1; i <= 4; i++)
					if (array[cell.RowIndex - i, cell.ColIndex - i].HadGone != Gone.None)
						return false;
			return true;
		}

		/// <summary>
		/// Tỉa theo đường chéo chính
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool PriDiangonallyPrune(Cell cell)
		{
			// Duyệt từ trên xuống
			if (cell.RowIndex <= board.RowNumber - 5 && cell.ColIndex >= 4)
				for (int i = 1; i <= 4; i++)
					if (array[cell.RowIndex + i, cell.ColIndex - i].HadGone != Gone.None)
						return false;
			// Duyệt từ dưới lên
			if (cell.ColIndex <= board.ColumnNumber - 5 && cell.RowIndex >= 4)
				for (int i = 1; i <= 4; i++)
					if (array[cell.RowIndex - i, cell.ColIndex + i].HadGone != Gone.None)
						return false;
			return true;
		}

		/// <summary>
		/// Tỉa theo chiều dọc
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool VerticalPrune(Cell cell)
		{
			// Duyệt phía dưới
			if (cell.RowIndex <= board.RowNumber - 5)
				for (int i = 1; i <= 4; i++)
					// Nếu ô đã đi thì không cắt tỉa
					if (array[cell.RowIndex + i, cell.ColIndex].HadGone != Gone.None)
						return false;
			// Duyệt phía trên
			if (cell.RowIndex >= 4)
				for (int i = 1; i <= 4; i++)
					// Nếu ô đã đi thì không cắt tỉa
					if (array[cell.RowIndex - i, cell.ColIndex].HadGone != Gone.None)
						return false;
			return true;
		}

		/// <summary>
		/// Tỉa theo chiều ngang
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private bool HorizontallyPrune(Cell cell)
		{
			// Duyệt phải
			if (cell.ColIndex <= board.ColumnNumber - 5)
				for (int i = 1; i <= 4; i++)
					// Nếu ô đã đi thì không cắt tỉa
					if (array[cell.RowIndex, cell.ColIndex + i].HadGone != Gone.None)
						return false;
			// Duyệt trái
			if (cell.ColIndex >= 4)
				for (int i = 1; i <= 4; i++)
					// Nếu ô đã đi thì không cắt tỉa
					if (array[cell.RowIndex, cell.ColIndex - i].HadGone != Gone.None)
						return false;
			return true;
		}

		#endregion

		#region AI

		#region Defense
		/// <summary>
		/// Duyệt phòng thủ theo đường chéo phụ
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalDefense_SecDiagonal(int row, int column)
		{
			int DefensePoint = 0;
			int LeftOurArmies = 0;
			int RightOurArmies = 0;
			int Enemies = 0;
			int TopSpaceCells = 0;
			int BottomSpaceCells = 0;
			bool ok = false;

			for (int i = 1; i <= 4 && row > 4 && column < board.ColumnNumber - 5; i++)
			{
				if (array[row - i, column + i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row - i, column + i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					RightOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					TopSpaceCells++;
				}
			}

			if (Enemies == 3 && TopSpaceCells == 1 && ok)
				DefensePoint -= 200;

			ok = false;

			// Đi xuống
			for (int i = 1; i <= 4 && row < board.RowNumber - 5 && column > 4; i++)
			{
				// Gặp quân địch
				if (array[row + i, column - i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row + i, column - i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					LeftOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					BottomSpaceCells++;
				}
			}

			if (Enemies == 3 && BottomSpaceCells == 1 && ok)
				DefensePoint -= 200;

			if (RightOurArmies > 0 && LeftOurArmies > 0 && (BottomSpaceCells + TopSpaceCells + Enemies) < 4)
				return 0;

			DefensePoint -= ArrayOfAttackPoints[RightOurArmies + RightOurArmies];
			DefensePoint += ArrayOfDefensePoints[Enemies];

			return DefensePoint;
		}

		/// <summary>
		/// Duyệt phòng thủ theo đường chéo chính
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalDefense_PriDiagonal(int row, int column)
		{
			int DefensePoint = 0;
			int LeftOurArmies = 0;
			int RightOurArmies = 0;
			int Enemies = 0;
			int TopSpaceCells = 0;
			int BottomSpaceCells = 0;
			bool ok = false;

			for (int i = 1; i <= 4 && row < board.RowNumber - 5 && column < board.ColumnNumber - 5; i++)
			{
				if (array[row + i, column + i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row + i, column + i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					RightOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					TopSpaceCells++;
				}
			}

			if (Enemies == 3 && TopSpaceCells == 1 && ok)
				DefensePoint -= 200;

			ok = false;

			// Đi xuống
			for (int i = 1; i <= 4 && row > 4 && column > 4; i++)
			{
				// Gặp quân địch
				if (array[row - i, column - i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row - i, column - i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					LeftOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					BottomSpaceCells++;
				}
			}

			if (Enemies == 3 && BottomSpaceCells == 1 && ok)
				DefensePoint -= 200;

			if (RightOurArmies > 0 && LeftOurArmies > 0 && (BottomSpaceCells + TopSpaceCells + Enemies) < 4)
				return 0;

			DefensePoint -= ArrayOfAttackPoints[RightOurArmies + RightOurArmies];
			DefensePoint += ArrayOfDefensePoints[Enemies];

			return DefensePoint;
		}

		/// <summary>
		/// Duyệt phòng thủ theo chiều dọc
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TravelsalDefense_Vertical(int row, int column)
		{
			int DefensePoint = 0;
			int LeftOurArmies = 0;
			int RightOurArmies = 0;
			int Enemies = 0;
			int TopSpaceCells = 0;
			int BottomSpaceCells = 0;
			bool ok = false;

			for (int i = 1; i <= 4 && row > 4; i++)
			{
				if (array[row - i, column].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row - i, column].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					RightOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					TopSpaceCells++;
				}
			}

			if (Enemies == 3 && TopSpaceCells == 1 && ok)
				DefensePoint -= 200;

			ok = false;

			// Đi xuống
			for (int i = 1; i <= 4 && row < board.RowNumber - 5; i++)
			{
				// Gặp quân địch
				if (array[row + i, column].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row + i, column].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					LeftOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					BottomSpaceCells++;
				}
			}

			if (Enemies == 3 && BottomSpaceCells == 1 && ok)
				DefensePoint -= 200;

			if (RightOurArmies > 0 && LeftOurArmies > 0 && (BottomSpaceCells + TopSpaceCells + Enemies) < 4)
				return 0;

			DefensePoint -= ArrayOfAttackPoints[RightOurArmies + RightOurArmies];
			DefensePoint += ArrayOfDefensePoints[Enemies];

			return DefensePoint;
		}

		/// <summary>
		/// Duyệt phòng thủ theo chiều ngang
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalDefense_Horizontal(int row, int column)
		{
			int DefensePoint = 0;
			int LeftOurArmies = 0;
			int RightOutArmies = 0;
			int Enemies = 0;
			int RightSpaceCells = 0;
			int LeftSpaceCells = 0;
			bool ok = false;

			for (int i = 1; i <= 4 && column < board.ColumnNumber - 5; i++)
			{
				if (array[row, column + i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row, column + i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					LeftOurArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					RightSpaceCells++;
				}
			}

			if (Enemies == 3 && RightSpaceCells == 1 && ok)
				DefensePoint -= 200;

			ok = false;

			for (int i = 1; i <= 4 && column > 4; i++)
			{
				if (array[row, column - i].HadGone == Gone.O)
				{
					if (i == 1)
						DefensePoint += 9;
					Enemies++;
				}
				else if (array[row, column - i].HadGone == Gone.X)
				{
					if (i == 4)
						DefensePoint -= 170;
					RightOutArmies++;
					break;
				}
				else
				{
					if (i == 1)
						ok = true;
					LeftSpaceCells++;
				}
			}

			if (Enemies == 3 && LeftSpaceCells == 1 && ok)
				DefensePoint -= 200;

			if (RightOutArmies > 0 && LeftOurArmies > 0 && (LeftSpaceCells + RightSpaceCells + Enemies) < 4)
				return 0;

			DefensePoint -= ArrayOfAttackPoints[RightOutArmies + RightOutArmies];
			DefensePoint += ArrayOfDefensePoints[Enemies];

			return DefensePoint;
		}

		#endregion
		#region Attack

		/// <summary>
		/// Duyệt tấn công theo đường chéo phụ (chéo ngược)
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalAttack_SecDiagonal(int row, int column)
		{
			int AttackPoint = 0;
			int Our = 0;
			int TopDiagonalEnemies = 0;
			int BottomDiagonalEnemies = 0;
			int SpaceCells = 0;

			// Đường chéo ngược lên
			for (int i = 1; i <= 4 && column < board.ColumnNumber - 5 && row > 4; i++)
			{
				if (array[row - i, column + i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row - i, column + i].HadGone == Gone.O)
				{
					TopDiagonalEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Đường chéo ngược xuống
			for (int i = 1; i <= 4 && column > 4 && row < board.RowNumber - 5; i++)
			{
				if (array[row + i, column - i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row + i, column - i].HadGone == Gone.O)
				{
					BottomDiagonalEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bị chặn 2 đầu khoảng trống không đủ tạo thành 5 nước
			if (TopDiagonalEnemies > 0 && BottomDiagonalEnemies > 0 && SpaceCells < 4)
				return 0;

			AttackPoint -= ArrayOfDefensePoints[TopDiagonalEnemies + BottomDiagonalEnemies];
			AttackPoint += ArrayOfAttackPoints[Our];
			return AttackPoint;
		}

		/// <summary>
		/// Duyệt tấn công theo đường chéo chính (chéo xuôi)
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalAttack_PriDiagonal(int row, int column)
		{
			int AttackPoint = 1;
			int Our = 0;
			int TopDiagonalEnemies = 0;
			int BottomDiagonalEnemies = 0;
			int SpaceCells = 0;

			// Đường chéo xuôi xuống
			for (int i = 1; i <= 4 && column < board.ColumnNumber - 5 && row < board.RowNumber - 5; i++)
			{
				if (array[row + i, column + i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row + i, column + i].HadGone == Gone.O)
				{
					TopDiagonalEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Đường chéo xuôi lên
			for (int i = 1; i <= 4 && row > 4 && column > 4; i++)
			{
				if (array[row - i, column - i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row - i, column - i].HadGone == Gone.O)
				{
					BottomDiagonalEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bị chặn 2 đầu khoảng trống không đủ tạo thành 5 nước
			if (TopDiagonalEnemies > 0 && BottomDiagonalEnemies > 0 && SpaceCells < 4)
				return 0;

			AttackPoint -= ArrayOfDefensePoints[TopDiagonalEnemies + BottomDiagonalEnemies];
			AttackPoint += ArrayOfAttackPoints[Our];
			return AttackPoint;
		}

		/// <summary>
		/// Duyệt tấn công theo chiều dọc
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TravelsalAttack_Vertical(int row, int column)
		{
			int AttackPoint = 0;
			int Our = 0;
			int TopEnemies = 0;
			int BottomEnemies = 0;
			int SpaceCells = 0;

			// Bên trên
			for (int i = 1; i <= 4 && row > 4; i++)
			{
				if (array[row - i, column].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row - i, column].HadGone == Gone.O)
				{
					TopEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bên dưới
			for (int i = 1; i <= 4 && row < board.RowNumber - 5; i++)
			{
				if (array[row + i, column].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row + i, column].HadGone == Gone.O)
				{
					BottomEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bị chặn 2 đầu khoảng trống không đủ tạo thành 5 nước
			if (TopEnemies > 0 && BottomEnemies > 0 && SpaceCells < 4)
				return 0;

			AttackPoint -= ArrayOfDefensePoints[TopEnemies + BottomEnemies];
			AttackPoint += ArrayOfAttackPoints[Our];
			return AttackPoint;
		}

		/// <summary>
		/// Duyệt tấn công theo chiều ngang
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private int TraversalAttack_Horizontal(int row, int column)
		{
			int AttackPoint = 0;
			int Our = 0;
			int RightEnemies = 0;
			int LeftEnemies = 0;
			int SpaceCells = 0;

			// Bên phải
			for (int i = 1; i <= 4 && column < board.ColumnNumber - 5; i++)
			{
				if (array[row, column + i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;
				}
				else if (array[row, column + i].HadGone == Gone.O)
				{
					RightEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bên trái
			for (int i = 1; i <= 4 && column > 4; i++)
			{
				if (array[row, column - i].HadGone == Gone.X)
				{
					if (i == 1)
						AttackPoint += 37;
					Our++;
					SpaceCells++;

				}
				else if (array[row, column - i].HadGone == Gone.O)
				{
					LeftEnemies++;
					break;
				}
				else SpaceCells++;
			}

			// Bị chặn 2 đầu khoảng trống không đủ tạo thành 5 nước
			if (RightEnemies > 0 && LeftEnemies > 0 && SpaceCells < 4)
				return 0;

			AttackPoint -= ArrayOfDefensePoints[RightEnemies + LeftEnemies];
			AttackPoint += ArrayOfAttackPoints[Our];
			return AttackPoint;
		}

		#endregion

		#region FindWinner
		public bool FindWinner(Graphics graphics)
		{
			if (history.Count != 0)
				foreach (Cell cell in history)
				{
					if (RightTraversal_Horizontal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| LeftTraversal_Horizontal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| TopTraversal_Vertical(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| BottomTraversal_Vertical(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| TopTraversal_PriDiagonal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| BottomTraversal_PriDiagonal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| TopTraversal_SecDiagonal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone)
						|| BottomTraversal_SecDiagonal(graphics, cell.RowIndex, cell.ColIndex, cell.HadGone))
					{
						EndGame(cell);
						return true;
					}
				}
			return false;
		}

		private static void DrawWinningLine(Graphics graphics, int x1, int y1, int x2, int y2)
		{
			graphics.DrawLine(new Pen(Color.Blue, 3f), x1, y1, x2, y2);
		}

		private void EndGame(Cell cell)
		{
			// Chơi với người
			if (Mode == 1)
			{
				if (cell.HadGone == Gone.X)
					MessageBox.Show("Quân X thắng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					MessageBox.Show("Quân O thắng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			// Chơi với máy
			else
			{
				if (cell.HadGone == Gone.X)
				{
					MessageBox.Show("Máy thắng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Media.PlayFailingSound();
				}
				else
				{
					MessageBox.Show("Người chơi thắng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Media.PlayWinningSound();
				}
			}
			IsReady = false;
		}

		private bool RightTraversal_Horizontal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex > board.ColumnNumber - 5)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex, colIndex + i].HadGone != hadGone)
					return false;
			}
			DrawWinningLine(graphics, colIndex * Cell.Width, rowIndex * Cell.Height + Cell.Height / 2, (colIndex + 5) * Cell.Width, rowIndex * Cell.Height + Cell.Height / 2);
			return true;
		}

		private bool LeftTraversal_Horizontal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (colIndex < 4)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex, colIndex - i].HadGone != hadGone)
					return false;
			}
			DrawWinningLine(graphics, (colIndex + 1) * Cell.Width, rowIndex * Cell.Height + Cell.Height / 2, (colIndex - 4) * Cell.Width, rowIndex * Cell.Height + Cell.Height / 2);
			return true;
		}

		private bool TopTraversal_Vertical(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex < 4 || colIndex < 4)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex - i, colIndex - i].HadGone != hadGone)
					return false;
			}
			DrawWinningLine(graphics, colIndex * Cell.Width + Cell.Width / 2, rowIndex * Cell.Height, colIndex * Cell.Width + Cell.Width / 2, (rowIndex + 5) * Cell.Height);
			return true;
		}

		private bool BottomTraversal_Vertical(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex > board.RowNumber - 5)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex + i, colIndex].HadGone != hadGone)
					return false;
			}
			DrawWinningLine(graphics, colIndex * Cell.Width + Cell.Width / 2, rowIndex * Cell.Height, colIndex * Cell.Width + Cell.Width / 2, (rowIndex + 5) * Cell.Height);
			return true;
		}

		private bool TopTraversal_PriDiagonal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex < 4 || colIndex < 4)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex - i, colIndex - i].HadGone != hadGone)
				{
					return false;
				}
			}
			DrawWinningLine(graphics, (colIndex + 1) * Cell.Width, (rowIndex + 1) * Cell.Height, (colIndex - 4) * Cell.Width, (rowIndex - 4) * Cell.Height);
			return true;
		}

		private bool BottomTraversal_PriDiagonal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex > board.RowNumber - 5 || colIndex > board.ColumnNumber - 5)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex + i, colIndex + i].HadGone != hadGone)
				{
					return false;
				}
			}
			DrawWinningLine(graphics, colIndex * Cell.Width, rowIndex * Cell.Height, (colIndex + 5) * Cell.Width, (rowIndex + 5) * Cell.Height);
			return true;
		}

		private bool TopTraversal_SecDiagonal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex < 4 || colIndex > board.ColumnNumber - 5)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex - i, colIndex + i].HadGone != hadGone)
				{
					return false;
				}
			}
			DrawWinningLine(graphics, colIndex * Cell.Width, (rowIndex + 1) * Cell.Height, (colIndex + 5) * Cell.Width, (rowIndex - 4) * Cell.Height);
			return true;
		}

		private bool BottomTraversal_SecDiagonal(Graphics graphics, int rowIndex, int colIndex, Gone hadGone)
		{
			if (rowIndex > board.RowNumber - 5 || colIndex < 4)
				return false;
			for (int i = 1; i <= 4; i++)
			{
				if (array[rowIndex + i, colIndex - i].HadGone != hadGone)
				{
					return false;
				}
			}
			DrawWinningLine(graphics, (colIndex + 1) * Cell.Width, rowIndex * Cell.Height, (colIndex - 4) * Cell.Width, (rowIndex + 5) * Cell.Height);
			return true;
		}
		#endregion
		#endregion
	}
}
