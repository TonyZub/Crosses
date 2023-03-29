using System.Linq;


namespace Crosses
{
    public sealed class WinCheckerService
    {
        #region Fields

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

        #endregion


        #region Methods

        public static bool IsEndGame(GameFieldController fieldControoller, out CellType[] winCombination,
            out GameSides winner)
        {
            winCombination = null;
            winner = GameSides.None;
            var isEndGame = !fieldControoller.CellDatas.Contains(null);

            foreach (var combinaton in _winCombinations)
            {
                var hasFirstCell = fieldControoller.HasCellData(combinaton[0], out CellData firstCellData);
                var hasSecondCell = fieldControoller.HasCellData(combinaton[1], out CellData secondCellData);
                var hasThirdCell = fieldControoller.HasCellData(combinaton[2], out CellData thirdCellData);

                if (hasFirstCell && hasSecondCell && hasThirdCell && (firstCellData.WhosMark == secondCellData.WhosMark &&
                    secondCellData.WhosMark == thirdCellData.WhosMark))
                {
                    isEndGame = true;
                    winCombination = combinaton;
                    winner = firstCellData.WhosMark;
                    break;
                }
            }

            return isEndGame;
        }

        public static bool IsAlmostWinFor(GameSides side, GameFieldController fieldControoller, out CellType cellLeft)
        {
            cellLeft = CellType.None;
            return false;
        }

        #endregion
    }
}

