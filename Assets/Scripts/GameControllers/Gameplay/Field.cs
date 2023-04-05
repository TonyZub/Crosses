using System.Collections.Generic;
using System.Linq;


namespace Crosses
{
    public struct Field
    {
        public Dictionary<CellData, GameSides> FieldCells;
        public GameSides[] CellSides => FieldCells.Values.ToArray();
        public CellData[] AllCellDatas => FieldCells.Select(x => x.Key).ToArray();
        public CellData[] AvaliableCellDatas => FieldCells.Where(x => x.Value == GameSides.None).Select(x => x.Key).ToArray();
        public CellData[] MarkedCellDatas => FieldCells.Where(x => x.Value != GameSides.None).Select(x => x.Key).ToArray();
        public CellData[] PlayerCells => FieldCells.Where(x => x.Value == GameSides.Player).Select(x => x.Key).ToArray();
        public CellData[] ComputerCells => FieldCells.Where(x => x.Value == GameSides.Computer).Select(x => x.Key).ToArray();
        public string Composition => CellSides.Select(x => ((int)x).ToString()).Aggregate((a, b) => a + b);

        public Field(Dictionary<CellData, GameSides> fieldCells)
        {
            FieldCells = fieldCells;
        }

        public Field(string composition)
        {
            FieldCells = FieldComposition.MakeCellDatasFromComposition(composition);
        }

        public GameSides GetSideForCell(CellData cellData)
        {
            return FieldCells[cellData];
        }

        public GameSides GetSideForCell(CellPlace cellPlace)
        {
            return FieldCells[AllCellDatas[(int)cellPlace]];
        }
    }
}