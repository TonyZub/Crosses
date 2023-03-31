using UnityEngine;


namespace Crosses
{
	[CreateAssetMenu(fileName = "UIData", menuName = "Data/UIData")]
	public sealed class UIData : ScriptableObject
	{
		#region Fields

		[SerializeField] private SceneLoadingCanvasModel _sceneLoadingCanvasModel;
		[SerializeField] private Sprite _crossSprite;
		[SerializeField] private Sprite _noughtSprite;

		#endregion


		#region Properties

		public SceneLoadingCanvasModel SceneLoadingCanvasModel => _sceneLoadingCanvasModel;
		public Sprite CrossSprite => _crossSprite;
		public Sprite NoughtSprite => _noughtSprite;

        #endregion


        #region Methods

		public Sprite GetMark(CellMarks mark)
        {
			return mark == CellMarks.Cross ? CrossSprite : mark == CellMarks.Nought ? NoughtSprite : null;
        }

        #endregion
    }
}
