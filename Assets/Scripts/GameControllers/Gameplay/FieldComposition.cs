using System;
using System.Collections.Generic;
using System.Linq;


namespace Crosses
{
	public class FieldComposition
	{
        #region Fields

        private static readonly Dictionary<CellData, GameSides> _baseCellDatas = new Dictionary<CellData, GameSides>()
        {
            { new CellData(CellPlace.TopLeft,
                new CellPlace[3]{
                    CellPlace.TopCenter,
                    CellPlace.CenterLeft,
                    CellPlace.CenterCenter
                },
                new CellPlace[5]{
                    CellPlace.TopRight,
                    CellPlace.CenterRight,
                    CellPlace.BotLeft,
                    CellPlace.BotCenter,
                    CellPlace.BotRight
                },
                1, 1, true, false), GameSides.None },
            { new CellData(CellPlace.TopCenter,
                new CellPlace[5]{
                    CellPlace.TopLeft,
                    CellPlace.TopRight,
                    CellPlace.CenterLeft,
                    CellPlace.CenterCenter,
                    CellPlace.CenterRight
                },
                new CellPlace[3]{
                    CellPlace.BotLeft,
                    CellPlace.BotCenter,
                    CellPlace.BotRight
                },
                1, 2, false, false), GameSides.None },
            { new CellData(CellPlace.TopRight,
                new CellPlace[5]{
                    CellPlace.TopLeft,
                    CellPlace.TopRight,
                    CellPlace.CenterLeft,
                    CellPlace.CenterCenter,
                    CellPlace.CenterRight
                },
                new CellPlace[3]{
                    CellPlace.BotLeft,
                    CellPlace.BotCenter,
                    CellPlace.BotRight
                },
                1, 3, false, true), GameSides.None },
            { new CellData(CellPlace.CenterLeft,
                new CellPlace[5]{
                    CellPlace.TopLeft,
                    CellPlace.TopCenter,
                    CellPlace.CenterCenter,
                    CellPlace.BotLeft,
                    CellPlace.BotCenter
                },
                new CellPlace[3]{
                    CellPlace.TopRight,
                    CellPlace.CenterRight,
                    CellPlace.BotRight
                },
                2, 1, false, false), GameSides.None },
            { new CellData(CellPlace.CenterCenter,
                new CellPlace[8]{
                    CellPlace.TopLeft,
                    CellPlace.TopCenter,
                    CellPlace.TopRight,
                    CellPlace.CenterLeft,
                    CellPlace.CenterRight,
                    CellPlace.BotLeft,
                    CellPlace.BotCenter,
                    CellPlace.BotRight
                },
                new CellPlace[0]{
                },
                2, 2, true, true), GameSides.None },
            { new CellData(CellPlace.CenterRight,
                new CellPlace[5]{
                    CellPlace.TopCenter,
                    CellPlace.TopRight,
                    CellPlace.CenterCenter,
                    CellPlace.BotCenter,
                    CellPlace.BotRight
                },
                new CellPlace[3]{
                    CellPlace.TopLeft,
                    CellPlace.CenterLeft,
                    CellPlace.BotLeft
                },
                2, 3, false, false), GameSides.None },
            { new CellData(CellPlace.BotLeft,
                new CellPlace[3]{
                    CellPlace.CenterLeft,
                    CellPlace.CenterCenter,
                    CellPlace.BotCenter,
                },
                new CellPlace[5]{
                    CellPlace.TopLeft,
                    CellPlace.TopCenter,
                    CellPlace.TopRight,
                    CellPlace.CenterRight,
                    CellPlace.BotRight
                },
                3, 1, false, true), GameSides.None },
            { new CellData(CellPlace.BotCenter,
                new CellPlace[5]{
                    CellPlace.CenterLeft,
                    CellPlace.CenterCenter,
                    CellPlace.CenterRight,
                    CellPlace.BotLeft,
                    CellPlace.BotRight
                },
                new CellPlace[3]{
                    CellPlace.TopLeft,
                    CellPlace.TopCenter,
                    CellPlace.TopRight
                },
                3, 2, false, false), GameSides.None },
            { new CellData(CellPlace.BotRight,
                new CellPlace[3]{
                    CellPlace.CenterCenter,
                    CellPlace.CenterRight,
                    CellPlace.BotCenter
                },
                new CellPlace[5]{
                    CellPlace.TopLeft,
                    CellPlace.TopCenter,
                    CellPlace.TopRight,
                    CellPlace.CenterLeft,
                    CellPlace.BotLeft
                },
                3, 3, true, false), GameSides.None }
        };

        public static readonly Field DefaultField = new Field(_baseCellDatas);

        #endregion


        #region Methods

        public static string MakeCompositionFromGameSidesArray(GameSides[] fieldCellSides)
        {
            if (fieldCellSides == null) throw new NullReferenceException("Array null");
            if (fieldCellSides.Length != 9) throw new Exception($"array wrong size - {fieldCellSides.Length}");
            return fieldCellSides.Select(x => ((int)x).ToString()).Aggregate((a,b) => a + b);
        }

        public static string MakeCompositionFromCharArray(char[] array)
        {
            if (array == null) throw new NullReferenceException("Array null");
            if (array.Length != 9) throw new Exception($"array wrong size - {array.Length}");
            return array.Select(x => x.ToString()).Aggregate((a, b) => a + b);
        }

        public static string MakeCompositionWithAddedPlace(string composition, CellPlace addedPlace, GameSides sideAdded)
        {
            var chars = composition.ToCharArray();
            chars[(int)addedPlace] = sideAdded == GameSides.Computer ? '2' : '1';
            return MakeCompositionFromCharArray(chars);
        }

        public static Dictionary<CellData, GameSides> MakeCellDatasFromComposition(string composition)
        {
            var chars = composition.ToCharArray();
            var dict = new Dictionary<CellData, GameSides>();

            for (int i = 0; i < chars.Length; i++)
            {
                dict.Add(DefaultField.AllCellDatas[i], GetSideFromChar(chars[i]));
            }

            return dict;
        }

        public static Field MakeFieldFromComposition(string composition)
        {
            return new Field(MakeCellDatasFromComposition(composition));
        }

        public static GameSides GetSideFromChar(char side)
        {
            return side == '0' ? GameSides.None : side == '1' ? GameSides.Player : GameSides.Computer;
        }

        public static char GetCharFromSide(GameSides side)
        {
            return side == GameSides.None ? '0' : side == GameSides.Player ? '1' : '2';
        }

        public static void RotateCombinationRight(ref string combination)
        {
            var chars = combination.ToCharArray();
            var newChars = new char[chars.Length];
            newChars[0] = chars[6];
            newChars[1] = chars[3];
            newChars[2] = chars[0];
            newChars[3] = chars[7];
            newChars[4] = chars[4];
            newChars[5] = chars[1];
            newChars[6] = chars[8];
            newChars[7] = chars[5];
            newChars[8] = chars[2];
            combination = MakeCompositionFromCharArray(newChars);
        }

        public static CellPlace RotatePlaceRight(CellPlace place)
        {
            return place switch
            {
                CellPlace.TopLeft => CellPlace.TopRight,
                CellPlace.TopCenter => CellPlace.CenterRight,
                CellPlace.TopRight => CellPlace.BotRight,
                CellPlace.CenterLeft => CellPlace.TopCenter,
                CellPlace.CenterCenter => CellPlace.CenterCenter,
                CellPlace.CenterRight => CellPlace.BotCenter,
                CellPlace.BotLeft => CellPlace.TopLeft,
                CellPlace.BotCenter => CellPlace.CenterLeft,
                CellPlace.BotRight => CellPlace.BotLeft,
                _ => throw new NotImplementedException(),
            };
        }

        public static CellPlace[] RotatePlacesRight(CellPlace[] places)
        {
            CellPlace[] rotatedPlaces = new CellPlace[places.Length];
            for (int i = 0; i < rotatedPlaces.Length; i++)
            {
                rotatedPlaces[i] = RotatePlaceRight(places[i]);
            }
            return rotatedPlaces;
        }

        #endregion
    }
}