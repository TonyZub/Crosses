using System.Collections.Generic;
using System.Linq;
using Extensions;


namespace Crosses
{
	public sealed class FieldAnalyzer
	{
        #region Fields

        private readonly GameFieldController _fieldController;
        private readonly RoundController _roundController;

        private readonly Dictionary<CellType, CellData> _cellDatas = new Dictionary<CellType, CellData>()
        {
            { CellType.TopLeft, null},
            { CellType.TopCenter, null},
            { CellType.TopRight, null},
            { CellType.CenterLeft, null},
            { CellType.CenterCenter, null},
            { CellType.CenterRight, null},
            { CellType.BotLeft, null},
            { CellType.BotCenter, null},
            { CellType.BotRight, null}
        };

        private readonly Dictionary<CellType, CellType[]> _bestSecondMoves = new Dictionary<CellType, CellType[]>()
        {
            { CellType.TopLeft, new CellType[1]{ CellType.CenterCenter } },
            { CellType.TopCenter, new CellType[3]{ CellType.CenterCenter, CellType.TopLeft, CellType.TopRight }},
            { CellType.TopRight, new CellType[1]{ CellType.CenterCenter }},
            { CellType.CenterLeft, new CellType[3]{ CellType.CenterCenter, CellType.TopLeft, CellType.BotLeft }},
            { CellType.CenterCenter, new CellType[4]{ CellType.TopCenter, CellType.TopRight, CellType.BotLeft, CellType.BotRight }},
            { CellType.CenterRight, new CellType[3]{ CellType.CenterCenter, CellType.TopRight, CellType.BotRight }},
            { CellType.BotLeft, new CellType[1]{ CellType.CenterCenter }},
            { CellType.BotCenter, new CellType[3]{ CellType.CenterCenter, CellType.BotLeft, CellType.BotRight }},
            { CellType.BotRight, new CellType[1]{ CellType.CenterCenter }}
        };

        private static CellType[][] _winCombinations = new CellType[8][]
        {
            new CellType[3]{ CellType.TopLeft, CellType.TopCenter, CellType.TopRight},
            new CellType[3]{ CellType.CenterLeft, CellType.CenterCenter, CellType.CenterRight},
            new CellType[3]{ CellType.BotLeft, CellType.BotCenter, CellType.BotRight},
            new CellType[3]{ CellType.TopLeft, CellType.CenterLeft, CellType.BotLeft},
            new CellType[3]{ CellType.TopCenter, CellType.CenterCenter, CellType.BotCenter},
            new CellType[3]{ CellType.TopRight, CellType.CenterRight, CellType.BotRight},
            new CellType[3]{ CellType.TopLeft, CellType.CenterCenter, CellType.BotRight},
            new CellType[3]{ CellType.BotLeft, CellType.CenterCenter, CellType.TopRight}
        };

        private Dictionary<CellType, int> _cellPoints;

        private GameSides _checkSide;
        private GameSides _oppositeSide;

        #endregion


        #region Constructor

        public FieldAnalyzer(GameFieldController fieldController, RoundController roundController)
        {
            _fieldController = fieldController;
            _roundController = roundController;
        }

        #endregion


        #region Private Methods

        private void ClearCellsDictionary()
        {
            foreach (var key in _cellDatas.Keys)
            {
                _cellDatas[key] = null;
            }
        }

        private Dictionary<CellType, int> GetCellPoints()
        {
            _cellPoints = new Dictionary<CellType, int>();
            _checkSide = _roundController.IsPlayersTurn ? GameSides.Player : GameSides.Computer;
            _oppositeSide = _roundController.IsPlayersTurn ? GameSides.Computer : GameSides.Player;

            foreach (var cell in _fieldController.AvaliableCellDatas)
            {
                _cellPoints.Add(cell.CellType, 0);

            }

            return _cellPoints;
        }

        private CellType GetBestSecondMove(CellType firstMove)
        {
            return _bestSecondMoves[firstMove].RandomObject();
        }

        //private int GetRowPoints(CellData cell, out int sameMarksInRow, out int oppositeMarksInRow)
        //{
        //    CellData[] sameRowCells = _fieldController.CellDatasValues
        //}

        //private int GetColumnPoints(CellType cell, out int sameMarksInCol, out int oppositeMarksInCol)
        //{

        //}

        //private int GetMajorDiagonalPoints(CellType cell, out int sameMarksInDiag, out int oppositeMarksInDiag)
        //{

        //}

        //private void GetMinorDiagonalPoints(CellType cell, out int sameMarksInDiag, out int oppositeMarksInDiag)
        //{

        //}

        #endregion


        #region Public Methods

        public void GetBestMove(GameFieldController fieldController, GameSides whosTurn, int turnIndex, 
            out CellType cell)
        {
            cell = CellType.None;
            switch (turnIndex)
            {
                case 0:
                    cell = fieldController.AvaliableCellDatas.RandomObject().CellType;
                    break;
                case 1:
                    //cell = GetBestSecondMove(fieldController.MarkedCellTypes[0]);
                    break;
                default:
                    break;
            }
        }

        public static bool IsEndGame(GameFieldController fieldControoller, out CellType[] winCombination,
            out GameSides winner)
        {
            winCombination = null;
            winner = GameSides.None;
            var isEndGame = fieldControoller.AvaliableCellDatas.Length == 0;

            foreach (var combinaton in _winCombinations)
            {
                var firstCellMark = fieldControoller.GetMarkForCell(combinaton[0]);
                var secondCellMark = fieldControoller.GetMarkForCell(combinaton[1]);
                var thirdCellMark = fieldControoller.GetMarkForCell(combinaton[2]);

                if (firstCellMark == secondCellMark && secondCellMark == thirdCellMark && firstCellMark != GameSides.None)
                {
                    isEndGame = true;
                    winCombination = combinaton;
                    winner = firstCellMark;
                    break;
                }
            }

            return isEndGame;
        }

        #endregion
    }
}