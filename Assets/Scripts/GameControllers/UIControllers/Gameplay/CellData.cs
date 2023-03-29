namespace Crosses
{
	public sealed class CellData
	{
        private readonly int _row;
        private readonly int _column;
        private readonly bool _isOnMajorDiagonal;
        private readonly bool _isOnMinorDiagonal;
        private readonly GameSides _whosMark;
        private readonly CellType _cellType;
        private readonly CellMarks _cellMark;

        public int Row => _row;
        public int Column => _column;
        public bool IsOnMajorDiagonal => _isOnMajorDiagonal;
        public bool IsOnMinorDiagonal => _isOnMinorDiagonal;
        public bool IsPlayersMark => WhosMark == GameSides.Player;
        public GameSides WhosMark => _whosMark;
        public CellType CellType => _cellType;
        public CellMarks CellMark => _cellMark;

        public CellData(GameCellModel model, GameSides whosMark, CellMarks mark)
        {
            _row = model.Row;
            _column = model.Column;
            _isOnMajorDiagonal = model.IsOnMajorDiagonal;
            _isOnMinorDiagonal = model.IsOnMinorDiagonal;
            _whosMark = whosMark;
            _cellType = model.CellType;
            _cellMark = mark;
        }

        public CellData(CellData data, CellMarks mark)
        {
            _row = data.Row;
            _column = data.Column;
            _isOnMajorDiagonal = data.IsOnMajorDiagonal;
            _isOnMinorDiagonal = data.IsOnMinorDiagonal;
            _whosMark = data.WhosMark;
            _cellType = data.CellType;
            _cellMark = mark;
        }
    }
}