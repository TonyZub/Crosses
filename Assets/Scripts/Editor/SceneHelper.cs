using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


namespace EditorExtensions
{
	public sealed class SceneHelper
	{
		private static readonly string PLAYER_PREFS_PREVIOUS_SCENE_KEY = "PreviousEditorScene";

		public static string CurrentSceneName => EditorSceneManager.GetActiveScene().name;
		private static string sceneToOpen;
		private static string pathToSceneToOpen;

		public static void StartScene(string sceneName)
		{
			if (EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}
			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		public static void TrySetPreviousScene()
		{
			var previousScenePath = PlayerPrefs.GetString(PLAYER_PREFS_PREVIOUS_SCENE_KEY);
			if (previousScenePath != string.Empty)
			{
				EditorSceneManager.OpenScene(previousScenePath);
				PlayerPrefs.SetString(PLAYER_PREFS_PREVIOUS_SCENE_KEY, string.Empty);
			}
		}

		static void RememberPreviousScene()
		{
			string[] guids = AssetDatabase.FindAssets("t:scene " + CurrentSceneName, null);
			PlayerPrefs.SetString(PLAYER_PREFS_PREVIOUS_SCENE_KEY, guids.Length > 0 ?
				AssetDatabase.GUIDToAssetPath(guids[0]) : string.Empty);
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
				EditorApplication.isPlaying || EditorApplication.isPaused ||
				EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);

				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					RememberPreviousScene();
					pathToSceneToOpen = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(pathToSceneToOpen);
					EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}

		public static bool SaveCurrentScene()
        {
			return EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}

		public static void ReloadCurrentScene()
        {
			EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
		}
	}
}
