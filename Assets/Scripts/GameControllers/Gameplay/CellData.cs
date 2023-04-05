namespace Crosses
{
	public sealed class CellData
	{
        private readonly CellPlace _cellPlace;
        private readonly CellPlace[] _closeCells;
        private readonly CellPlace[] _remoteCells;
        private readonly int _row;
        private readonly int _column;
        private readonly bool _isOnMajorDiagonal;
        private readonly bool _isOnMinorDiagonal;

        public CellPlace CellPlace => _cellPlace;
        public CellPlace[] CloseCells => _closeCells;
        public CellPlace[] RemoteCells => _remoteCells;
        public int Row => _row;
        public int Column => _column;
        public bool IsOnMajorDiagonal => _isOnMajorDiagonal;
        public bool IsOnMinorDiagonal => _isOnMinorDiagonal;

        public CellData(CellPlace place, CellPlace[] closeCells, CellPlace[] remoteCells,
            int row, int col, bool isOnMajorDiag, bool isOnMinorDiag)
        {
            _cellPlace = place;
            _closeCells = closeCells;
            _remoteCells = remoteCells; 
            _row = row;
            _column = col;
            _isOnMajorDiagonal = isOnMajorDiag;
            _isOnMinorDiagonal = isOnMinorDiag;
        }
    }
}