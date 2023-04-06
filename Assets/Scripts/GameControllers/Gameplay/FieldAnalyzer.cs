using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;


namespace Crosses
{
	public sealed class FieldAnalyzer
	{
        #region Constants

        private const int EXTREME_POINTS = 1000;

        public static readonly Dictionary<WinCombinations, CellPlace[]> EndCombinations = new Dictionary<WinCombinations, CellPlace[]>()
        {
            { WinCombinations.TopRow, new CellPlace[3]{ CellPlace.TopLeft, CellPlace.TopCenter, CellPlace.TopRight} },
            { WinCombinations.CenterRow, new CellPlace[3]{ CellPlace.CenterLeft, CellPlace.CenterCenter, CellPlace.CenterRight} },
            { WinCombinations.BotRow, new CellPlace[3]{ CellPlace.BotLeft, CellPlace.BotCenter, CellPlace.BotRight} },
            { WinCombinations.LeftColumn, new CellPlace[3]{ CellPlace.TopLeft, CellPlace.CenterLeft, CellPlace.BotLeft} },
            { WinCombinations.CenterColumn, new CellPlace[3]{ CellPlace.TopCenter, CellPlace.CenterCenter, CellPlace.BotCenter} },
            { WinCombinations.RightColumn, new CellPlace[3]{ CellPlace.TopRight, CellPlace.CenterRight, CellPlace.BotRight} },
            { WinCombinations.MajorDiagonal, new CellPlace[3]{ CellPlace.TopLeft, CellPlace.CenterCenter, CellPlace.BotRight} },
            { WinCombinations.MinorDiagonal, new CellPlace[3]{ CellPlace.BotLeft, CellPlace.CenterCenter, CellPlace.TopRight} },
        };

        private static readonly Dictionary<string, CellPlace[]> _bestSecondMoves = new Dictionary<string, CellPlace[]>()
        {
            { "100000000", new CellPlace[1]{ CellPlace.CenterCenter } },
            { "010000000", new CellPlace[3]{ CellPlace.CenterCenter, CellPlace.TopLeft, CellPlace.TopRight }},
            { "001000000", new CellPlace[1]{ CellPlace.CenterCenter }},
            { "000100000", new CellPlace[3]{ CellPlace.CenterCenter, CellPlace.TopLeft, CellPlace.BotLeft }},
            { "000010000", new CellPlace[4]{ CellPlace.TopLeft, CellPlace.TopRight, CellPlace.BotLeft, CellPlace.BotRight }},
            { "000001000", new CellPlace[3]{ CellPlace.CenterCenter, CellPlace.TopRight, CellPlace.BotRight }},
            { "000000100", new CellPlace[1]{ CellPlace.CenterCenter }},
            { "000000010", new CellPlace[3]{ CellPlace.CenterCenter, CellPlace.BotLeft, CellPlace.BotRight }},
            { "000000001", new CellPlace[1]{ CellPlace.CenterCenter }}
        };

        private static readonly Dictionary<string, CellPlace[]> _bestThirdMovesTemplates = new Dictionary<string, CellPlace[]>()
        {
            { "210000000", new CellPlace[3]{ CellPlace.CenterLeft, CellPlace.CenterCenter, CellPlace.BotLeft } },
            { "201000000", new CellPlace[3]{ CellPlace.CenterLeft, CellPlace.CenterCenter, CellPlace.BotRight } },
            { "200100000", new CellPlace[3]{ CellPlace.TopCenter, CellPlace.TopRight, CellPlace.CenterCenter } },
            { "200010000", new CellPlace[1]{ CellPlace.BotRight } },
            { "200001000", new CellPlace[3]{ CellPlace.TopRight, CellPlace.CenterCenter, CellPlace.BotLeft } },
            { "200000100", new CellPlace[3]{ CellPlace.TopCenter, CellPlace.TopRight, CellPlace.BotRight } },
            { "200000010", new CellPlace[3]{ CellPlace.TopRight, CellPlace.CenterCenter, CellPlace.BotLeft } },
            { "200000001", new CellPlace[2]{ CellPlace.TopRight, CellPlace.BotLeft } },

            { "120000000", new CellPlace[1]{ CellPlace.CenterCenter } },
            { "021000000", new CellPlace[1]{ CellPlace.CenterCenter } },
            { "020100000", new CellPlace[2]{ CellPlace.TopLeft, CellPlace.CenterCenter } },
            { "020010000", new CellPlace[6]{ CellPlace.TopLeft, CellPlace.TopRight, CellPlace.CenterLeft, CellPlace.CenterRight, CellPlace.BotLeft, CellPlace.BotRight } },
            { "020001000", new CellPlace[2]{ CellPlace.TopRight, CellPlace.CenterCenter } },
            { "020000100", new CellPlace[1]{ CellPlace.TopLeft } },
            { "020000010", new CellPlace[1]{ CellPlace.CenterCenter } },
            { "020000001", new CellPlace[1]{ CellPlace.TopRight } },

            { "100020000", new CellPlace[7]{ CellPlace.TopCenter, CellPlace.TopRight, CellPlace.CenterLeft, CellPlace.CenterRight, CellPlace.BotLeft, CellPlace.BotCenter, CellPlace.BotRight } },
            { "010020000", new CellPlace[6]{ CellPlace.TopLeft, CellPlace.TopRight, CellPlace.CenterLeft, CellPlace.CenterRight, CellPlace.BotLeft, CellPlace.BotRight } },
        };

        private class FieldAnalysis
        {
            public readonly string Composition;
            public readonly Field Field;
            public readonly GameSides CheckSide;
            public readonly GameSides OppositeSide;
            public readonly bool HasCheckPrewin;
            public readonly bool HasOppositePrewin;
            public readonly CellPlace PrewinContinuePlace;
            public readonly CellPlace PrewinPreventionPlace;

            public FieldAnalysis(string composition, GameSides checkSide, GameSides oppositeSide)
            {
                Composition = composition;
                CheckSide = checkSide;
                OppositeSide = oppositeSide;
                Field = new Field(Composition);
                int[][] marksAmounts = GetSideMarksCountsArray(Field, checkSide, oppositeSide);
                var marksAmountsForCheckSide = marksAmounts[0];
                var marksAmountsForOppositeSide = marksAmounts[1];
                HasCheckPrewin = HasPrewin(marksAmountsForCheckSide, marksAmountsForOppositeSide, out WinCombinations[] checkPrewinCombinations);
                HasOppositePrewin = HasPrewin(marksAmountsForOppositeSide, marksAmountsForCheckSide, out WinCombinations[] oppositePrewinCombinations);
                if (HasCheckPrewin)
                {
                    PrewinContinuePlace = GetRestPlaceForWin(Field, checkPrewinCombinations[0]);
                }
                if (HasOppositePrewin)
                {
                    PrewinPreventionPlace = GetRestPlaceForWin(Field, oppositePrewinCombinations[0]);
                }
            }
        }

        private class MoveAnalysis
        {
            public readonly CellPlace CellPlace;
            public readonly GameSides WhosTurnWas;
            public readonly GameSides WhosNextTurn;
            public readonly GameSides CheckSide;
            public readonly GameSides OppositeSide;

            public readonly string Composition;
            public readonly Field Field;
            public readonly int Turn;

            public readonly int[] MarksAmountsForCheckSide;
            public readonly int[] MarksAmountsForOppositeSide;

            public readonly int AmountOfPrewinsForCheckSide;
            public readonly int AmountOfPrewinsForOppositeSide;

            public readonly bool CheckSideHasPrewin;
            public readonly bool OppositeSideHasPrewin;
            public readonly bool IsCheckSideWon;
            public readonly bool IsOppositeSideWon;

            public readonly WinCombinations[] CheckSidePrewins;
            public readonly WinCombinations[] OppositeSidePrewins;

            public readonly MoveAnalysis PreviousNode;
            public readonly List<MoveAnalysis> NextNodes;

            public int Points => (WhosNextTurn == CheckSide ? (AmountOfPrewinsForCheckSide * AmountOfPrewinsForCheckSide ) :
                (-(AmountOfPrewinsForOppositeSide * AmountOfPrewinsForOppositeSide) * EXTREME_POINTS));
            public bool IsEnd => IsCheckSideWon || IsOppositeSideWon || NextNodes.Count == 0;

            public MoveAnalysis(MoveAnalysis previousNode, CellPlace cellPlace, string previousComposition, 
                GameSides whosTurnWas, GameSides whosNextTurn, GameSides checkSide, GameSides oppositeSide, int turn)
            {
                PreviousNode = previousNode;
                CellPlace = cellPlace;
                WhosTurnWas = whosTurnWas;
                WhosNextTurn = whosNextTurn;
                CheckSide = checkSide;
                OppositeSide = oppositeSide;
                Turn = turn;
                Composition = FieldComposition.MakeCompositionWithAddedPlace(previousComposition, CellPlace, WhosTurnWas);
                Field = new Field(Composition);
                int[][] marksAmounts = GetSideMarksCountsArray(Field, checkSide, oppositeSide);
                MarksAmountsForCheckSide = marksAmounts[0];
                MarksAmountsForOppositeSide = marksAmounts[1];
                CheckSideHasPrewin = HasPrewin(MarksAmountsForCheckSide, MarksAmountsForOppositeSide, out WinCombinations[] CheckSidePrewins);;
                OppositeSideHasPrewin = HasPrewin(MarksAmountsForOppositeSide, MarksAmountsForCheckSide, out WinCombinations[] OppositeSidePrewins);
                IsCheckSideWon = HasWin(MarksAmountsForCheckSide, MarksAmountsForOppositeSide, out WinCombinations[] checkWinCombinations);
                IsOppositeSideWon = HasWin(MarksAmountsForOppositeSide, MarksAmountsForCheckSide, out WinCombinations[] oppositeWinCombinations);
                AmountOfPrewinsForCheckSide = CheckSidePrewins.Length;
                AmountOfPrewinsForOppositeSide = OppositeSidePrewins.Length;

                NextNodes = new List<MoveAnalysis>();
                if (IsCheckSideWon || IsOppositeSideWon) return;
                if (WhosNextTurn == CheckSide && OppositeSideHasPrewin)
                {
                    NextNodes.Add(new MoveAnalysis(this, GetRestPlaceForWin(Field, OppositeSidePrewins[0]),
                        Composition, WhosNextTurn, WhosTurnWas, CheckSide, OppositeSide, Turn++));
                    return;
                }
                else if (WhosNextTurn == OppositeSide && CheckSideHasPrewin)
                {
                    NextNodes.Add(new MoveAnalysis(this, GetRestPlaceForWin(Field, CheckSidePrewins[0]),
                        Composition, WhosNextTurn, WhosTurnWas, CheckSide, OppositeSide, Turn++));
                    return;
                }
                foreach (var nextMove in Field.AvaliableCellDatas)
                {
                    NextNodes.Add(new MoveAnalysis(this, nextMove.CellPlace, Composition, WhosNextTurn,
                        WhosTurnWas, CheckSide, OppositeSide, Turn++));
                }
            }

            public float GetAveragePoints()
            {
                if (NextNodes.Count == 1)
                {
                    return (Points + NextNodes[0].GetAveragePoints()) / 2;
                }
                else if (NextNodes.Count > 0)
                {
                    return (Points + NextNodes.Select(x => x.GetAveragePoints()).Aggregate((a, b) => a + b) /
                        NextNodes.Count) / 2;
                }
                else
                {
                    return Points;
                }
            }
        }

        #endregion


        #region Fields

        private readonly GameFieldController _fieldController;
        private readonly Dictionary<string, CellPlace[]> _bestThirdMoves;

        #endregion


        #region Constructor

        public FieldAnalyzer(GameFieldController fieldController)
        {
            _fieldController = fieldController;
            _bestThirdMoves = GetFullBestThirdMovesDict();
        }

        #endregion


        #region Private Methods

        private Dictionary<string, CellPlace[]> GetFullBestThirdMovesDict()
        {
            var dict = new Dictionary<string, CellPlace[]>();

            foreach (var template in _bestThirdMovesTemplates)
            {
                var key = template.Key;
                var value = template.Value;
                dict.Add(key, value);
                for (int i = 0; i < 3; i++)
                {
                    FieldComposition.RotateCombinationRight(ref key);
                    value = FieldComposition.RotatePlacesRight(value);
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        private CellData GetBestSecondMove(string fieldComposition)
        {
            var place = _bestSecondMoves[fieldComposition].RandomObject();
            return _fieldController.AllCellDatas.First(x => x.CellPlace == place);
        }

        private CellData GetBestThirdMove(string fieldComposition)
        {
            var place = _bestThirdMoves[fieldComposition].RandomObject();
            return _fieldController.AllCellDatas.First(x => x.CellPlace == place);
        }

        private CellData GetBestMove(GameSides whosTurn)
        {
            var oppositeTurn = whosTurn == GameSides.Player ? GameSides.Computer : GameSides.Player;
            return _fieldController.AllCellDatas[(int)GetBestCellPlace(whosTurn, oppositeTurn)];
        }

        private CellPlace GetBestCellPlace(GameSides whosTurn, GameSides oppositeTurn)
        {
            var fieldAnalysis = new FieldAnalysis(_fieldController.Field.Composition, whosTurn, oppositeTurn);
            if (fieldAnalysis.HasCheckPrewin)
            {
                return fieldAnalysis.PrewinContinuePlace;
            }
                
            if (fieldAnalysis.HasOppositePrewin)
            {
                return fieldAnalysis.PrewinPreventionPlace;
            }
                
            var movesNodes = new List<MoveAnalysis>();

            foreach (var cell in _fieldController.Field.AvaliableCellDatas)
            {
                movesNodes.Add(new MoveAnalysis(null, cell.CellPlace, _fieldController.Field.Composition,
                    whosTurn, oppositeTurn, whosTurn, oppositeTurn, 1));
            }

            float maxPoints = float.MinValue;
            CellPlace bestPlace = CellPlace.CenterCenter;
            foreach (var node in movesNodes)
            {
                var nodePoints = node.GetAveragePoints();
                if (maxPoints < nodePoints)
                {
                    bestPlace = node.CellPlace;
                    maxPoints = nodePoints;
                }
            }

            return bestPlace;
        }

        public static int[][] GetSideMarksCountsArray(Field field, GameSides checkSide, GameSides oppositeSide)
        {
            int[][] sideMarksArray = new int[2][];
            sideMarksArray[0] = new int[8];
            sideMarksArray[1] = new int[8];
            int index = 0;
            for (int i = 1; i < 4; i++)
            {
                GetMarksForRow(field, checkSide, oppositeSide, i, out int checkMarksInRow, out int oppositeMarksInRow);
                sideMarksArray[0][index] = checkMarksInRow;
                sideMarksArray[1][index] = oppositeMarksInRow;
                index++;
            }
            for (int i = 1; i < 4; i++)
            {
                GetMarksForColumn(field, checkSide, oppositeSide, i, out int checkMarksInColumn, out int oppositeMarksInColumn);
                sideMarksArray[0][index] = checkMarksInColumn;
                sideMarksArray[1][index] = oppositeMarksInColumn;
                index++;
            }
            GetMarksForDiagonal(field, checkSide, oppositeSide, true, out int sideMarksOnMajor, out int oppositeMarksOnMajor);
            sideMarksArray[0][index] = sideMarksOnMajor;
            sideMarksArray[1][index] = oppositeMarksOnMajor;
            index++;
            GetMarksForDiagonal(field, checkSide, oppositeSide, false, out int sideMarksOnMinor, out int oppositeMarksOnMinor);
            sideMarksArray[0][index] = sideMarksOnMinor;
            sideMarksArray[1][index] = oppositeMarksOnMinor;
            return sideMarksArray;
        }

        public static void GetMarksForRow(Field field, GameSides checkSide, GameSides oppositeSide,
            int row, out int checkSideMarksCount, out int oppositeSideMarksCount)
        {
            var markedCellsInRow = field.MarkedCellDatas.Where(x => x.Row == row).ToArray();
            checkSideMarksCount = markedCellsInRow.Where(x => field.GetSideForCell(x) == checkSide).Count();
            oppositeSideMarksCount = markedCellsInRow.Where(x => field.GetSideForCell(x) == oppositeSide).Count();
        }

        public static void GetMarksForColumn(Field field, GameSides checkSide, GameSides oppositeSide,
            int column, out int checkSideMarksCount, out int oppositeSideMarksCount)
        {
            var markedCellsInColumn = field.MarkedCellDatas.Where(x => x.Column == column).ToArray();
            checkSideMarksCount = markedCellsInColumn.Where(x => field.GetSideForCell(x) == checkSide).Count();
            oppositeSideMarksCount = markedCellsInColumn.Where(x => field.GetSideForCell(x) == oppositeSide).Count();
        }

        public static void GetMarksForDiagonal(Field field, GameSides checkSide, GameSides oppositeSide,
            bool isMajor, out int checkSideMarksCount, out int oppositeSideMarksCount)
        {
            var markedCellsInColumn = field.MarkedCellDatas.Where(x => isMajor ? x.IsOnMajorDiagonal : x.IsOnMinorDiagonal).ToArray();
            checkSideMarksCount = markedCellsInColumn.Where(x => field.GetSideForCell(x) == checkSide).Count();
            oppositeSideMarksCount = markedCellsInColumn.Where(x => field.GetSideForCell(x) == oppositeSide).Count();
        }
        
        public static bool HasPrewin(int[] sideMarksCount, int[] oppositeMarksCount, out WinCombinations[] combinations)
        {
            return HasCombinationForAmount(sideMarksCount, oppositeMarksCount, 2, out combinations);
        }

        public static bool HasWin(int[] sideMarksCount, int[] oppositeMarksCount, out WinCombinations[] combinations)
        {
            return HasCombinationForAmount(sideMarksCount, oppositeMarksCount, 3, out combinations);
        }

        private static bool HasCombinationForAmount(int[] sideMarksCount, int[] oppositeMarksCount, int amount, out WinCombinations[] combinations)
        {
            var combinationsList = new List<WinCombinations>();
            var hasCombinations = false;
            for (int i = 0; i < sideMarksCount.Length; i++)
            {
                if (sideMarksCount[i] == amount && oppositeMarksCount[i] == 0)
                {
                    hasCombinations = true;
                    combinationsList.Add((WinCombinations)(i + 1));
                }
            }
            combinations = combinationsList.ToArray();
            return hasCombinations;
        }

        private static CellPlace GetRestPlaceForWin(Field field, WinCombinations combination)
        {
            foreach (var cell in EndCombinations[combination])
            {
                if (field.GetSideForCell(cell) == GameSides.None) return cell;
            }
            throw new Exception("Cant find rest place for win");
        }

        #endregion


        #region Public Methods

        public CellData GetBestMove(GameSides whosTurn, int turnIndex)
        {
            switch (turnIndex)
            {
                case 0:
                    return _fieldController.AvaliableCellDatas.RandomObject();
                case 1:
                    return GetBestSecondMove(_fieldController.Field.Composition);
                case 2:
                    return GetBestThirdMove(_fieldController.Field.Composition);
                default:
                    return GetBestMove(whosTurn);
            }
        }

        public static bool IsEndGame(GameFieldController fieldControoller, out WinCombinations winCombination, 
            out GameSides winner)
        {
            winCombination = WinCombinations.None;
            winner = GameSides.None;
            var isEndGame = fieldControoller.AvaliableCellDatas.Length == 0;

            foreach (var combinaton in EndCombinations)
            {
                var firstCellMark = fieldControoller.Field.GetSideForCell(combinaton.Value[0]);
                var secondCellMark = fieldControoller.Field.GetSideForCell(combinaton.Value[1]);
                var thirdCellMark = fieldControoller.Field.GetSideForCell(combinaton.Value[2]);

                if (firstCellMark == secondCellMark && secondCellMark == thirdCellMark && firstCellMark != GameSides.None)
                {
                    isEndGame = true;
                    winCombination = combinaton.Key;
                    winner = firstCellMark;
                    break;
                }
            }

            return isEndGame;
        }

        #endregion
    }
}