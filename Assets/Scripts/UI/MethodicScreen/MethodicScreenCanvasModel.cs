using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace Crosses
{
	public sealed class MethodicScreenCanvasModel : MonoBehaviour
	{
		#region Events

		public event Action<string> RequestResponse;
		public event Action<string> RequestError;

        #endregion


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

		UnityWebRequest _request;

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


        #region UnityMethods

        private void OnDestroy()
        {
			_request.Dispose();
		}

		#endregion


#region Methods

#if UNITY_WEBGL_API && !UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void MakePostRequest (string url, string data);
#endif

		public void GenerateRequest(string url, string json)
		{
#if UNITY_WEBGL_API && !UNITY_EDITOR
			MakePostRequest(url, json);
#endif
			//StartCoroutine(ProcessRequest(url, json));
		}

		//private IEnumerator ProcessRequest(string uri, string json)
		//{
		//	_request = new UnityWebRequest(uri, "POST");
		//	byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		//	_request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		//	_request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		//	_request.SetRequestHeader("Content-Type", "application/json");
		//	_request.SetRequestHeader("Access-Control-Allow-Origin", "*");
		//	_request.SetRequestHeader("Access-Control-Allow-Credentials", "false");

		//	yield return _request.SendWebRequest();

		//	if (_request.result == UnityWebRequest.Result.ConnectionError)
		//	{
		//		RequestError?.Invoke(_request.error);
		//	}
		//	else
		//	{
		//		RequestResponse?.Invoke(_request.downloadHandler.text);
		//	}
		//}

		public void OnRequestNetError()
        {
			RequestError?.Invoke(string.Empty);
        }

		public void OnRequestResponse(string message)
        {
			_errorTxtPanel.GetComponentInChildren<TMP_Text>().text = message;
			RequestResponse?.Invoke(message);
        }

#endregion
	}
}