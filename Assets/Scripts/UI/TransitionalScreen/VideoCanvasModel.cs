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
		[SerializeField] private string _firstVideoURL;
		[SerializeField] private string _secondVideoURL;

		[SerializeField] private CanvasGroup _introCanvasGroup;
		[SerializeField] private CanvasGroup _videoCanvasGroup;
		[SerializeField] private CanvasGroup _feedBackCanvasGroup;
		[SerializeField] private RectTransform _containerPanel;
		[SerializeField] private TMP_InputField _feedbackinput;
		[SerializeField] private TMP_Text _alertText;
		[SerializeField] private TMP_Text _inputFeedbackPlaceholderTxt;
		[SerializeField] private TMP_Text _inputFeedbackTxt;
		[SerializeField] private Button _moveNextBtn;

		[SerializeField] private Vector2 _containerAnchorsForVerticalScreen;
		[SerializeField] private Vector2 _containerAnchorsForHorizontalScreen;
		[SerializeField] private Vector2 _inputFeedbackFontsSizesForVerticalScreen;
		[SerializeField] private Vector2 _inputFeedbackFontsSizesForHorizontalScreen;
		[SerializeField] private float _panelsSwitchDuration;

		#endregion


		#region Properties

		public VideoPlayer VideoPlayer => _videoPlayer;
		public CanvasGroup IntroCanvasGroup => _introCanvasGroup;
		public CanvasGroup VideoCanvasGroup => _videoCanvasGroup;
		public CanvasGroup FeedbackCanvasGroup => _feedBackCanvasGroup;
		public RectTransform ContainerPanel => _containerPanel;
		public TMP_InputField FeedbackInput => _feedbackinput;
		public TMP_Text AlertText => _alertText;
		public TMP_Text InputFeedbackPlaceholderTxt => _inputFeedbackPlaceholderTxt;
		public TMP_Text InputFeedbackTxt => _inputFeedbackTxt;
		public Button MoveNextBtn => _moveNextBtn;

		public Vector2 ContainerAnchorsForVerticalScreen => _containerAnchorsForVerticalScreen;
		public Vector2 ContainerAnchorsForHorizontalScreen => _containerAnchorsForHorizontalScreen;
		public Vector2 InputFeedbackFontSizesForVerticalScreen => _inputFeedbackFontsSizesForVerticalScreen;
		public Vector2 InputFeedbackFontSizesForHorizontalScreen => _inputFeedbackFontsSizesForHorizontalScreen;
		public string FirstVideoURL => _firstVideoURL;
		public string SecondVideoURL => _secondVideoURL;
		public float PanelsSwitchDuration => _panelsSwitchDuration;

		#endregion
	}
}