using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extensions;


namespace Crosses
{
	public sealed class FieldAnalyzer
	{
        #region Constants

        private static int[][] _markPoints = new int[3][]
        {
            new int[3] { 1, 30, 100},
            new int[3] { 10, 10, 0},
            new int[3] { 50, 0, 0},
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

        private static Dictionary<CellType, CellType[]> _bestSecondMoves = new Dictionary<CellType, CellType[]>()
        {
            { CellType.TopLeft, new CellType[1]{ CellType.CenterCenter } },
            { CellType.TopCenter, new CellType[3]{ CellType.CenterCenter, CellType.TopLeft, CellType.TopRight }},
            { CellType.TopRight, new CellType[1]{ CellType.CenterCenter }},
            { CellType.CenterLeft, new CellType[3]{ CellType.CenterCenter, CellType.TopLeft, CellType.BotLeft }},
            { CellType.CenterCenter, new CellType[4]{ CellType.TopLeft, CellType.TopRight, CellType.BotLeft, CellType.BotRight }},
            { CellType.CenterRight, new CellType[3]{ CellType.CenterCenter, CellType.TopRight, CellType.BotRight }},
            { CellType.BotLeft, new CellType[1]{ CellType.CenterCenter }},
            { CellType.BotCenter, new CellType[3]{ CellType.CenterCenter, CellType.BotLeft, CellType.BotRight }},
            { CellType.BotRight, new CellType[1]{ CellType.CenterCenter }}
        };

        private static Dictionary<string, CellType[]> _exceptionsForComputer = new Dictionary<string, CellType[]>()
        {
            {
                "001020100",
                new CellType[4] {
                    CellType.CenterLeft, CellType.TopCenter, CellType.BotCenter, CellType.CenterRight
                }
            },
            {
                "100020001",
                new CellType[4] {
                    CellType.CenterLeft, CellType.TopCenter, CellType.BotCenter, CellType.CenterRight
                }
            },
        };

        private static Dictionary<string, CellType[]> _exceptionsForPlayer = new Dictionary<string, CellType[]>()
        {
            {
                "002010200",
                new CellType[4] {
                    CellType.CenterLeft, CellType.TopCenter, CellType.BotCenter, CellType.CenterRight
                }
            },
            {
                "200010002",
                new CellType[4] {
                    CellType.CenterLeft, CellType.TopCenter, CellType.BotCenter, CellType.CenterRight
                }
            },
        };

        #endregion


        #region Fields

        private readonly GameFieldController _fieldController;

        private Dictionary<CellData, int> _cellPoints;
        private GameSides _checkSide;
        private GameSides _oppositeSide;

        #endregion


        #region Constructor

        public FieldAnalyzer(GameFieldController fieldController)
        {
            _fieldController = fieldController;
        }

        #endregion


        #region Private Methods

        private void GetCellPoints(out int maxPoint)
        {
            _cellPoints = new Dictionary<CellData, int>();
            foreach (var cell in _fieldController.AvaliableCellDatas)
            {
                _cellPoints.Add(cell, 0);
                int sameMarksInRow = GetMarksInRow(cell, _checkSide);
                int sameMarksInCol = GetMarksInCol(cell, _checkSide);
                int sameMarksOnMajorDiag = GetMarksInMajorDiagonal(cell, _checkSide);
                int sameMarksOnMinorDiag = GetMarksInMinorDiagonal(cell, _checkSide);
                int oppositeMarksInRow = GetMarksInRow(cell, _oppositeSide);
                int oppositeMarksInCol = GetMarksInCol(cell, _oppositeSide);
                int oppositeMarksOnMajorDiag = GetMarksInMajorDiagonal(cell, _oppositeSide);
                int oppositeMarksOnMinorDiag = GetMarksInMinorDiagonal(cell, _oppositeSide);
                _cellPoints[cell] += GetMarkPointsForLine(sameMarksInRow, oppositeMarksInRow);
                _cellPoints[cell] += GetMarkPointsForLine(sameMarksInCol, oppositeMarksInCol);
                _cellPoints[cell] += GetMarkPointsForLine(sameMarksOnMajorDiag, oppositeMarksOnMajorDiag);
                _cellPoints[cell] += GetMarkPointsForLine(sameMarksOnMinorDiag, oppositeMarksOnMinorDiag);
            }
            maxPoint = Mathf.Max(_cellPoints.Values.ToArray());
        }

        private int GetMarksInRow(CellData cell, GameSides side)
        {
            return _fieldController.AllCellDatas.Where(x => x.Row == cell.Row && _fieldController.
                GetMarkForCell(x) == side).Count();
        }

        private int GetMarksInCol(CellData cell, GameSides side)
        {
            return _fieldController.AllCellDatas.Where(x => x.Column == cell.Column && _fieldController.
                GetMarkForCell(x) == side).Count();
        }

        private int GetMarksInMajorDiagonal(CellData cell, GameSides side)
        {
            return cell.IsOnMajorDiagonal ? _fieldController.AllCellDatas.Where(x => x.IsOnMajorDiagonal && 
                _fieldController.GetMarkForCell(x) == side).Count() : 0;
        }

        private int GetMarksInMinorDiagonal(CellData cell, GameSides side)
        {
            return cell.IsOnMinorDiagonal ? _fieldController.AllCellDatas.Where(x => x.IsOnMinorDiagonal &&
                _fieldController.GetMarkForCell(x) == side).Count() : 0;
        }

        private int GetMarkPointsForLine(int sameMarks, int oppositeMarks)
        {
            return _markPoints[oppositeMarks][sameMarks];
        }

        private CellData GetBestSecondMove(CellType firstMove)
        {
            var randomType = _bestSecondMoves[firstMove].RandomObject();
            return _fieldController.AllCellDatas.First(x => x.CellType == randomType);
        }

        private bool IsInComputerExceptions(out CellType bestMove)
        {
            bestMove = CellType.None;
            if (_exceptionsForComputer.TryGetValue(_fieldController.GameSidesString, out CellType[] moves))
            {
                bestMove = moves.RandomObject();
                return true;
            }
            return false;
        }

        private bool IsInPlayerExceptions(out CellType bestMove)
        {
            bestMove = CellType.None;
            if (_exceptionsForPlayer.TryGetValue(_fieldController.GameSidesString, out CellType[] moves))
            {
                bestMove = moves.RandomObject();
                return true;
            }
            return false;
        }

        private CellData GetBestMove()
        {
            switch (_checkSide)
            {
                case GameSides.Computer:
                    if (IsInComputerExceptions(out CellType bestMoveForComputer))
                    {
                        return _fieldController.GetCellByType(bestMoveForComputer);
                    }
                    break;
                case GameSides.Player:
                    if(IsInPlayerExceptions(out CellType bestMoveForPlayer))
                    {
                        return _fieldController.GetCellByType(bestMoveForPlayer);
                    }
                    break;
                default:
                    break;
            }
            GetCellPoints(out int maxPoints);
            return _cellPoints.Where(x => x.Value == maxPoints).ToArray().RandomObject().Key;
        }

        #endregion


        #region Public Methods

        public CellData GetBestMove(GameSides whosTurn, int turnIndex)
        {
            _checkSide = whosTurn;
            _oppositeSide = _checkSide == GameSides.Player ? GameSides.Computer : GameSides.Player;
            switch (turnIndex)
            {
                case 0:
                    return _fieldController.AvaliableCellDatas.RandomObject();
                case 1:
                    return GetBestSecondMove(_fieldController.MarkedCellDatas[0].CellType);
                default:
                    return GetBestMove();
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