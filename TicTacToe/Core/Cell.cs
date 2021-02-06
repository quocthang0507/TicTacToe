namespace TicTacToe.Core
{
	public class Cell
	{
		public const int Width = 30;
		public const int Height = 30;
		public int Row { get; set; }
		public int Column { get; set; }
		public bool Gone { get; set; }

		public Cell()
		{

		}

		public Cell(int rowIndex, int colIndex, bool gone)
		{
			this.Row = rowIndex;
			this.Column = colIndex;
			this.Gone = gone;
		}

	}
}
