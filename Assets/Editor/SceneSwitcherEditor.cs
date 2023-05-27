#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class SceneSwitcherEditor
{
	private static readonly string SCENES_PATH = "Assets/Scenes/";
	private static readonly string SCENE_EXTENSION = ".unity";

	public static string GetScenePath(string sceneName)
	{
		return $"{SCENES_PATH}{sceneName}{SCENE_EXTENSION}";
	}

	public static void TryOpenScene(string sceneName)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			EditorSceneManager.OpenScene(GetScenePath(sceneName));
		}
	}

	[MenuItem("Scenes/Bootstrap")] public static void OpenBootstrap() { TryOpenScene("Bootstrap"); }
	[MenuItem("Scenes/WelcomeScreen")] public static void OpenWelcomeScreen() { TryOpenScene("WelcomeScreen"); }
	[MenuItem("Scenes/Gameplay")] public static void OpenGameplay() { TryOpenScene("Gameplay"); }
	[MenuItem("Scenes/MetodicScreen")] public static void OpenMetodic() { TryOpenScene("MetodicScreen"); }
	[MenuItem("Scenes/TransitionalScreen")] public static void OpenTransitional() { TryOpenScene("TransitionalScreen"); }
	[MenuItem("Scenes/ThanksScreen")] public static void OpenThanks() { TryOpenScene("ThanksScreen"); }
}
#endif