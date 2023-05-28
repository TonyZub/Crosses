using UnityEngine;


namespace Crosses
{
	[CreateAssetMenu(fileName = "MethodicData", menuName = "Data/MethodicData")]
	public class MethodicData : ScriptableObject
	{
		#region Fields

#if UNITY_EDITOR
		[ArrayElementTitle("_highParameter")]
#endif
		[SerializeField] private MethodicElement[] _methodicElements;
		[SerializeField] private string[] _methodicQuestions;

		#endregion


		#region Properties

		public MethodicElement[] MethodicElements => _methodicElements;
		public string[] MethodicQuestions => _methodicQuestions;

		#endregion
	}
}