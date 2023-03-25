using UnityEditor;
using UnityEngine;


namespace EditorExtensions
{
	[InitializeOnLoad]
	public sealed class EditorToolbarExtensionsController
	{
		static EditorToolbarExtensionsController()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
			ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
			EditorApplication.playModeStateChanged += OnPlaymodeChanged;
		}

		static void OnRightToolbarGUI()
        {
			UpdateClearSaveButton();
			GUILayout.FlexibleSpace();
		}

		static void OnLeftToolbarGUI()
		{
			GUILayout.FlexibleSpace();
			UpdateStartMainMenuButton();
			//UpdateStartGameButton();
			//UpdateLevelSelectionTooltip();
		}

		static void UpdateClearSaveButton()
		{
			if (GUILayout.Button(new GUIContent("CLEAR PREFS", "Clear player prefs"), ToolbarStyles.topTooltipButtonStyle))
			{
				PlayerPrefs.DeleteAll();
				LevelSelectionHelper.SelectLevel(0);
				LevelSelectionHelper.SetLevelSelected(false);
				Debug.Log("Player prefs were cleared");
			}
		}

		static void UpdateStartMainMenuButton()
        {
			if (GUILayout.Button(new GUIContent("START", "Start welcome screen scene"), ToolbarStyles.topTooltipButtonStyle))
			{
				LevelSelectionHelper.SetLevelSelected(false);
				SceneHelper.StartScene("Bootstrap");
			}
		}

		//static void UpdateStartGameButton()
		//{
		//	if (GUILayout.Button(new GUIContent("GAME LEVEL", "Start game level scene"), ToolbarStyles.topTooltipButtonStyle))
		//	{
		//		LevelSelectionHelper.SetLevelSelected(true);
		//		SceneHelper.StartScene("Bootstrap");
		//	}
		//}

		//static void UpdateLevelSelectionTooltip()
		//{
		//	LevelSelectionHelper.SelectLevel(GUILayout.Toolbar(LevelSelectionHelper.SelectedLevelIndex,
		//		LevelSelectionHelper.LevelsArray));
		//}

		static void OnPlaymodeChanged(PlayModeStateChange currentState)
		{
			if (currentState == PlayModeStateChange.EnteredEditMode)
			{
				SceneHelper.TrySetPreviousScene();
			}
		}
	}
}