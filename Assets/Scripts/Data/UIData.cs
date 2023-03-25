using UnityEngine;


namespace Crosses
{
	[CreateAssetMenu(fileName = "UIData", menuName = "Data/UIData")]
	public sealed class UIData : ScriptableObject
	{
		#region Fields

		[SerializeField] private SceneLoadingCanvasModel _sceneLoadingCanvasModel;

		#endregion


		#region Properties

		public SceneLoadingCanvasModel SceneLoadingCanvasModel => _sceneLoadingCanvasModel;

		#endregion
	}
}
