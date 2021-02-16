namespace TicTacToe.Core
{
	public class Cell
	{
		/// <summary>
		/// Chiều rộng của ô
		/// </summary>
		public const int Width = 30;
		/// <summary>
		/// Chiều cao của ô
		/// </summary>
		public const int Height = 30;
		/// <summary>
		/// Thứ tự của ô theo dòng
		/// </summary>
		public int RowIndex { get; set; }
		/// <summary>
		/// Thứ tự của ô theo cột
		/// </summary>
		public int ColIndex { get; set; }
		/// <summary>
		/// Ô hiện tại đã đi chưa? Nếu rồi thì thuộc về quân nào? True là đen, false là trắng
		/// </summary>
		public bool? HadGone { get; set; }

		public Cell()
		{

		}

		public Cell(int rowIndex, int colIndex, bool? gone)
		{
			this.RowIndex = rowIndex;
			this.ColIndex = colIndex;
			this.HadGone = gone;
		}

		public Cell Clone()
		{
			return (Cell)MemberwiseClone();
		}

	}
}
