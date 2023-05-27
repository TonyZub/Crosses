using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;


namespace Crosses
{
	public sealed class VideoCanvasModel : MonoBehaviour
	{
		#region Fields

		[SerializeField] private VideoPlayer _videoPlayer;
		[SerializeField] private VideoClip _firstVideoClip;
		[SerializeField] private VideoClip _secondVideoClip;

		[SerializeField] private CanvasGroup _introCanvasGroup;
		[SerializeField] private CanvasGroup _videoCanvasGroup;
		[SerializeField] private CanvasGroup _feedBackCanvasGroup;
		[SerializeField] private RectTransform _containerPanel;
		[SerializeField] private TMP_InputField _feedbackinput;
		[SerializeField] private TMP_Text _alertText;
		[SerializeField] private Button _moveNextBtn;

		[SerializeField] private Vector2 _containerAnchorsForVerticalScreen;
		[SerializeField] private Vector2 _containerAnchorsForHorizontalScreen;
		[SerializeField] private float _panelsSwitchDuration;

		#endregion


		#region Properties

		public VideoPlayer VideoPlayer => _videoPlayer;
		public VideoClip FirstVideoClip => _firstVideoClip;
		public VideoClip SecondVideoClip => _secondVideoClip;
		public CanvasGroup IntroCanvasGroup => _introCanvasGroup;
		public CanvasGroup VideoCanvasGroup => _videoCanvasGroup;
		public CanvasGroup FeedbackCanvasGroup => _feedBackCanvasGroup;
		public RectTransform ContainerPanel => _containerPanel;
		public TMP_InputField FeedbackInput => _feedbackinput;
		public TMP_Text AlertText => _alertText;
		public Button MoveNextBtn => _moveNextBtn;

		public Vector2 ContainerAnchorsForVerticalScreen => _containerAnchorsForVerticalScreen;
		public Vector2 ContainerAnchorsForHorizontalScreen => _containerAnchorsForHorizontalScreen;
		public float PanelsSwitchDuration => _panelsSwitchDuration;

		#endregion
	}
}