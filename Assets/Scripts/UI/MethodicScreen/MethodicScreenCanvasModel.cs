using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Crosses
{
	public sealed class MethodicScreenCanvasModel : MonoBehaviour
	{
		#region Fields

		[SerializeField] private CanvasGroup _instructionCanvasGroup;
		[SerializeField] private CanvasGroup _mainMethodicCanvasGroup;
		[SerializeField] private CanvasGroup _secondMethodicCanvasGroup;
		[SerializeField] private CanvasGroup _inputCanvasGroup;
		[SerializeField] private RectTransform _inputContainerPanel;
		[SerializeField] private Transform _preloaderPanel;
		[SerializeField] private Transform _errorTxtPanel;

		[SerializeField] private Button[] _firstMethodicButtons;
		[SerializeField] private Button[] _secondMethodicButtons;
		[SerializeField] private Button _moveNextBtn;

		[SerializeField] private Image _firstMethodicFillableImg;
		[SerializeField] private Image _secondMethodicFillableImg;

		[SerializeField] private TMP_Text _moveNextBtnTxt;
		[SerializeField] private TMP_Text _firstMethodicHightParameterTxt;
		[SerializeField] private TMP_Text _firstMethodicLowParameterTxt;
		[SerializeField] private TMP_Text _secondMethodicParameterTxt;
		[SerializeField] private TMP_Text _inputEmailPlaceholderTxt;
		[SerializeField] private TMP_Text _inputEmailTxt;
		[SerializeField] private TMP_Text _inputFeedbackPlaceholderTxt;
		[SerializeField] private TMP_Text _inputFeedbackTxt;
		[SerializeField] private TMP_Text _firstMethodicFillableImageTxt;
		[SerializeField] private TMP_Text _secondMethodicFillableImageTxt;

		[SerializeField] private TMP_InputField _emailInput;
		[SerializeField] private TMP_InputField _feedbackInput;

		[SerializeField] private Vector2 _containerAnchorsForVerticalScreen;
		[SerializeField] private Vector2 _containerAnchorsForHorizontalScreen;
		[SerializeField] private Vector2 _inputfontSizesForVerticalScreen;
		[SerializeField] private Vector2 _inputfontSizesForHorizontalScreen;

		[SerializeField] private float _switchScreenDuration;

		#endregion


		#region Properties

		public CanvasGroup InstructionCanvasGroup => _instructionCanvasGroup;
		public CanvasGroup MainMethodicCanvasGroup => _mainMethodicCanvasGroup;
		public CanvasGroup SecondMethodicCanvasGroup => _secondMethodicCanvasGroup;
		public CanvasGroup InputCanvasGroup => _inputCanvasGroup;
		public RectTransform InputContainerPanel => _inputContainerPanel;
		public Transform PreloaderPanel => _preloaderPanel;
		public Transform ErrorTxtPanel => _errorTxtPanel;

		public Button[] FirstMethodicButtons => _firstMethodicButtons;
		public Button[] SecondMethodicButtons => _secondMethodicButtons;
		public Button MoveNextBtn => _moveNextBtn;

		public Image FirstMetodicFillableImg => _firstMethodicFillableImg;
		public Image SecondMetodicFillableImg => _secondMethodicFillableImg;

		public TMP_Text MoveNextBtnTxt => _moveNextBtnTxt;
		public TMP_Text FirstMethodicHightParameterTxt => _firstMethodicHightParameterTxt;
		public TMP_Text FirstMethodicLowParameterTxt => _firstMethodicLowParameterTxt;
		public TMP_Text SecondMethodicParameterTxt => _secondMethodicParameterTxt;
		public TMP_Text InputEmailPlaceholderTxt => _inputEmailPlaceholderTxt;
		public TMP_Text InputEmailTxt => _inputEmailTxt;
		public TMP_Text InputFeedbackPlaceholderTxt => _inputFeedbackPlaceholderTxt;
		public TMP_Text InputFeedbackTxt => _inputFeedbackTxt;
		public TMP_Text FirstMethodicFillableImgTxt => _firstMethodicFillableImageTxt;
		public TMP_Text SecondMethodicFillableImgTxt => _secondMethodicFillableImageTxt;

		public TMP_InputField EmailInput => _emailInput;
		public TMP_InputField FeedbackInput => _feedbackInput;

		public Vector2 ContainerAnchorsForVerticalScreen => _containerAnchorsForVerticalScreen;
		public Vector2 ContainerAnchorsForHorizontalScreen => _containerAnchorsForHorizontalScreen;
		public Vector2 InputfontSizesForVerticalScreen => _inputfontSizesForVerticalScreen;
		public Vector2 InputfontSizesForHorizontalScreen => _inputfontSizesForHorizontalScreen;
		public float SwitchScreenDuration => _switchScreenDuration;

		#endregion
	}
}