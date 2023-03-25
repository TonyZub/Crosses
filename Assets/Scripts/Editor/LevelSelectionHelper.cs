using UnityEngine;
using UnityEditor;


namespace EditorExtensions
{
	public sealed class LevelSelectionHelper
	{
		private static readonly string PLAYER_PREFS_SELECTED_LEVEL_MODE_KEY = "EditorSelectedLevel";
		private static readonly string PLAYER_PREFS_IS_LEVEL_SELECTED_KEY = "EditorIsLevelSelected";
		public static string[] LevelsArray => new string[6] { "1", "2", "3", "4", "5", "T" };
		public static int SelectedLevelIndex => PlayerPrefs.GetInt(PLAYER_PREFS_SELECTED_LEVEL_MODE_KEY);
		public static bool IsLevelSelected => PlayerPrefs.GetInt(PLAYER_PREFS_IS_LEVEL_SELECTED_KEY) == 1;

		public static void SetLevelSelected(bool isLevelSelected)
        {
			PlayerPrefs.SetInt(PLAYER_PREFS_IS_LEVEL_SELECTED_KEY, isLevelSelected ? 1 : 0);
		}

		public static void SelectLevel(int inputModeIndex)
		{
			if (PlayerPrefs.HasKey(PLAYER_PREFS_SELECTED_LEVEL_MODE_KEY) &&
				(inputModeIndex == SelectedLevelIndex || EditorApplication.isPlaying)) return;
			PlayerPrefs.SetInt(PLAYER_PREFS_SELECTED_LEVEL_MODE_KEY, inputModeIndex);
			Debug.Log($"Selected {(inputModeIndex == 5 ? "test" : inputModeIndex + 1)} level");			
		}
	}
}

