namespace Crosses
{
	public sealed class CellData
	{
        private readonly CellType _cellType;
        private readonly int _row;
        private readonly int _column;
        private readonly bool _isOnMajorDiagonal;
        private readonly bool _isOnMinorDiagonal;

        public CellType CellType => _cellType;
        public int Row => _row;
        public int Column => _column;
        public bool IsOnMajorDiagonal => _isOnMajorDiagonal;
        public bool IsOnMinorDiagonal => _isOnMinorDiagonal;

        public CellData(CellType type, int row, int col, bool isOnMajorDiag, bool isOnMinorDiag)
        {
            _cellType = type;
            _row = row;
            _column = col;
            _isOnMajorDiagonal = isOnMajorDiag;
            _isOnMinorDiagonal = isOnMinorDiag;
        }
    }
}