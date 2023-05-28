using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;


namespace Crosses
{
	public sealed class TransitionalScreenController
	{
        #region PrivateData

        private enum TransitionStates
        {
            Intro,
            FirstVideo,
            SecondVideo,
            Feedback
        }

        #endregion


        #region Fields

        private readonly VideoCanvasModel _canvasModel;
        private readonly ScreenOrientationService _screenOrientationService;

        private CanvasGroup _closingGroup;
        private CanvasGroup _openingGroup;

        private Sequence _canvasGroupSequence;
        private TransitionStates _currentState;

        #endregion


        #region Constructor

        public TransitionalScreenController(VideoCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            _screenOrientationService = GlobalContext.Instance.GetDependency<GlobalServices>().ScreenOrientationService;
            AdaptContainer(_screenOrientationService.ScreenOrientation);
            SubscribeEvents();
            SetState(TransitionStates.Intro);
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.AddListener(OnMoveNextPressed);
            _screenOrientationService.ScreenOrientationChanged += AdaptContainer;
            GlobalController.Instance.OnUpdate += CheckSkipVideo;
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            DOTween.Kill(this);
            _canvasModel.MoveNextBtn.onClick.RemoveAllListeners();
            _screenOrientationService.ScreenOrientationChanged -= AdaptContainer;
            GlobalController.Instance.OnUpdate -= CheckSkipVideo;
            SceneStateMachine.Instance.OnBeforeStateChange -= UnsubscribeEvents;
        }

        private void AdaptContainer(ScreenTypes screenType)
        {
            if (screenType == ScreenTypes.Horizontal)
            {
                _canvasModel.ContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.x,
                    _canvasModel.ContainerPanel.anchorMin.y);
                _canvasModel.ContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.y,
                    _canvasModel.ContainerPanel.anchorMax.y);
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMin = _canvasModel.InputFeedbackFontSizesForHorizontalScreen.x;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMax = _canvasModel.InputFeedbackFontSizesForHorizontalScreen.y;
                _canvasModel.InputFeedbackTxt.fontSizeMin = _canvasModel.InputFeedbackFontSizesForHorizontalScreen.x;
                _canvasModel.InputFeedbackTxt.fontSizeMax = _canvasModel.InputFeedbackFontSizesForHorizontalScreen.y;
            }
            else
            {
                _canvasModel.ContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.x,
                    _canvasModel.ContainerPanel.anchorMin.y);
                _canvasModel.ContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.y,
                    _canvasModel.ContainerPanel.anchorMax.y);
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMin = _canvasModel.InputFeedbackFontSizesForVerticalScreen.x;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMax = _canvasModel.InputFeedbackFontSizesForVerticalScreen.y;
                _canvasModel.InputFeedbackTxt.fontSizeMin = _canvasModel.InputFeedbackFontSizesForVerticalScreen.x;
                _canvasModel.InputFeedbackTxt.fontSizeMax = _canvasModel.InputFeedbackFontSizesForVerticalScreen.y;
            }
        }

        private void OnMoveNextPressed()
        {
            switch (_currentState)
            {
                case TransitionStates.Intro:
                    SetState(TransitionStates.FirstVideo);
                    break;
                case TransitionStates.Feedback:
                    TryMoveToNextScene();
                    break;
                default:
                    break;
            }
        }

        private void SetState(TransitionStates state)
        {
            _currentState = state;
            switch (_currentState)
            {
                case TransitionStates.Intro:
                    break;
                case TransitionStates.FirstVideo:
                    SwitchCanvasGroups(_canvasModel.IntroCanvasGroup, _canvasModel.VideoCanvasGroup);
                    _canvasModel.VideoPlayer.clip = _canvasModel.FirstVideoClip;
                    _canvasModel.VideoPlayer.Play();
                    _canvasModel.VideoPlayer.loopPointReached += OnFirstVideoEnded;
                    _canvasModel.MoveNextBtn.gameObject.SetActive(false);
                    break;
                case TransitionStates.SecondVideo:
                    _canvasModel.VideoPlayer.Stop();
                    _canvasModel.VideoPlayer.clip = _canvasModel.SecondVideoClip;
                    _canvasModel.VideoPlayer.Play();
                    _canvasModel.VideoPlayer.loopPointReached += OnSecondVideoEnded;
                    break;
                case TransitionStates.Feedback:
                    _canvasModel.VideoPlayer.Stop();
                    _canvasModel.MoveNextBtn.gameObject.SetActive(true);
                    SwitchCanvasGroups(_canvasModel.VideoCanvasGroup, _canvasModel.FeedbackCanvasGroup);
                    break;
                default:
                    break;
            }
        }

        private void OnFirstVideoEnded(VideoPlayer videoPlayer)
        {
            _canvasModel.VideoPlayer.loopPointReached -= OnFirstVideoEnded;
            SetState(TransitionStates.SecondVideo);
        }

        private void OnSecondVideoEnded(VideoPlayer videoPlayer)
        {
            _canvasModel.VideoPlayer.loopPointReached -= OnSecondVideoEnded;
            SetState(TransitionStates.Feedback);
        }

        private void SwitchCanvasGroups(CanvasGroup closingGroup, CanvasGroup openingGroup)
        {
            _closingGroup = closingGroup;
            _openingGroup = openingGroup;
            _openingGroup.gameObject.SetActive(true);
            _canvasGroupSequence = DOTween.Sequence(this);
            _canvasGroupSequence.Append(_closingGroup.DOFade(0f, _canvasModel.PanelsSwitchDuration));
            _canvasGroupSequence.Join(_openingGroup.DOFade(1f, _canvasModel.PanelsSwitchDuration));
            _canvasGroupSequence.OnComplete(OnCanvasGroupSwitched);
        }

        private void OnCanvasGroupSwitched()
        {
            _closingGroup.gameObject.SetActive(false);
        }

        private void TryMoveToNextScene()
        {
            if (IsFeedbackWritten())
            {
                StoreTransitionalData();
                MoveToNextScene();
            }       
        }

        private void MoveToNextScene()
        {
            SceneStateMachine.Instance.SetState(SceneStateNames.Methodic);
        }

        private bool IsFeedbackWritten()
        {
            if (_canvasModel.FeedbackInput.text.Equals(string.Empty))
            {
                _canvasModel.AlertText.text = "ќцените видео";
                return false;
            }
            _canvasModel.AlertText.text = string.Empty;
            return true;
        }

        private void StoreTransitionalData()
        {
            GlobalContext.Instance.GetDependency<GlobalServices>().ResearchDataService.
                SetVideoFeedback(_canvasModel.FeedbackInput.text);
        }

        private void CheckSkipVideo()
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C))
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if(_currentState == TransitionStates.FirstVideo)
                    {
                        SetState(TransitionStates.SecondVideo);
                    }
                    else if(_currentState == TransitionStates.SecondVideo)
                    {
                        SetState(TransitionStates.Feedback);
                    }
                }
            }
        }

        #endregion
    }
}