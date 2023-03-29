using UnityEngine;


namespace Crosses
{
	public static class PlayerDataSaver
	{
        #region Constants

        private const string PLAYER_MARK_KEY = "PlayerMark";

        #endregion


        #region Methods

        public static void SavePlayerMarkChoise(CellMarks mark)
        {
            PlayerPrefs.SetInt(PLAYER_MARK_KEY, (int)mark);
        }

        public static CellMarks GetSavedPlayerMark()
        {
            return PlayerPrefs.HasKey(PLAYER_MARK_KEY) ? (CellMarks)PlayerPrefs.GetInt(PLAYER_MARK_KEY) : CellMarks.None;
        }

        #endregion
    }
}