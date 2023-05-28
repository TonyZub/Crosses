using UnityEngine;
using DG.Tweening;


namespace Crosses
{
	public sealed class MethodicScreenController
	{
        #region PrivateData

        private enum MethodicStates
        {
            Instruction,
            FirstMetodic,
            SecondMetodic,
            DataInput,
            DataRequestCall
        }

        #endregion


        #region Fields

        private readonly MethodicScreenCanvasModel _canvasModel;
        private readonly ScreenOrientationService _screenOrientationService;
        private readonly ResearchDataService _researchDataService;

        private CanvasGroup _closingGroup;
        private CanvasGroup _openingGroup;

        private Sequence _canvasGroupSequence;
        private MethodicStates _currentState;

        private int _mainMethodicQuestion;
        private int _secondMethodicQuestion;

        #endregion


        #region Constructor

        public MethodicScreenController(MethodicScreenCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            var globalServices = GlobalContext.Instance.GetDependency<GlobalServices>();
            _screenOrientationService = globalServices.ScreenOrientationService;
            _researchDataService = globalServices.ResearchDataService;
            AdaptContainer(_screenOrientationService.ScreenOrientation);
            SubscribeEvents();
            SetState(MethodicStates.Instruction);
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.AddListener(OnMoveNextPressed);
            _screenOrientationService.ScreenOrientationChanged += AdaptContainer;
            for (int i = 0; i < _canvasModel.FirstMethodicButtons.Length; i++)
            {
                var index = i;
                _canvasModel.FirstMethodicButtons[i].onClick.AddListener(() => OnMainMethodicButtonPressed(index));
            }
            for (int i = 0; i < _canvasModel.SecondMethodicButtons.Length; i++)
            {
                var index = i;
                _canvasModel.SecondMethodicButtons[i].onClick.AddListener(() => OnSecondMethodicButtonPressed(index));
            }
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            DOTween.Kill(this);
            _canvasModel.MoveNextBtn.onClick.RemoveAllListeners();
            _screenOrientationService.ScreenOrientationChanged -= AdaptContainer;
            for (int i = 0; i < _canvasModel.FirstMethodicButtons.Length; i++)
            {
                _canvasModel.FirstMethodicButtons[i].onClick.RemoveAllListeners();
            }
            for (int i = 0; i < _canvasModel.SecondMethodicButtons.Length; i++)
            {
                _canvasModel.SecondMethodicButtons[i].onClick.RemoveAllListeners();
            }
            SceneStateMachine.Instance.OnBeforeStateChange -= UnsubscribeEvents;
        }

        private void AdaptContainer(ScreenTypes screenType)
        {
            if (screenType == ScreenTypes.Horizontal)
            {
                _canvasModel.InputContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.x,
                    _canvasModel.InputContainerPanel.anchorMin.y);
                _canvasModel.InputContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.y,
                    _canvasModel.InputContainerPanel.anchorMax.y);
                _canvasModel.InputEmailPlaceholderTxt.fontSizeMin = _canvasModel.InputfontSizesForHorizontalScreen.x;
                _canvasModel.InputEmailPlaceholderTxt.fontSizeMax = _canvasModel.InputfontSizesForHorizontalScreen.y;
                _canvasModel.InputEmailTxt.fontSizeMin = _canvasModel.InputfontSizesForHorizontalScreen.x;
                _canvasModel.InputEmailTxt.fontSizeMax = _canvasModel.InputfontSizesForHorizontalScreen.y;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMin = _canvasModel.InputfontSizesForHorizontalScreen.x;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMax = _canvasModel.InputfontSizesForHorizontalScreen.y;
                _canvasModel.InputFeedbackTxt.fontSizeMin = _canvasModel.InputfontSizesForHorizontalScreen.x;
                _canvasModel.InputFeedbackTxt.fontSizeMax = _canvasModel.InputfontSizesForHorizontalScreen.y;
            }
            else
            {
                _canvasModel.InputContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.x,
                    _canvasModel.InputContainerPanel.anchorMin.y);
                _canvasModel.InputContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.y,
                    _canvasModel.InputContainerPanel.anchorMax.y);
                _canvasModel.InputEmailPlaceholderTxt.fontSizeMin = _canvasModel.InputfontSizesForVerticalScreen.x;
                _canvasModel.InputEmailPlaceholderTxt.fontSizeMax = _canvasModel.InputfontSizesForVerticalScreen.y;
                _canvasModel.InputEmailTxt.fontSizeMin = _canvasModel.InputfontSizesForVerticalScreen.x;
                _canvasModel.InputEmailTxt.fontSizeMax = _canvasModel.InputfontSizesForVerticalScreen.y;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMin = _canvasModel.InputfontSizesForVerticalScreen.x;
                _canvasModel.InputFeedbackPlaceholderTxt.fontSizeMax = _canvasModel.InputfontSizesForVerticalScreen.y;
                _canvasModel.InputFeedbackTxt.fontSizeMin = _canvasModel.InputfontSizesForVerticalScreen.x;
                _canvasModel.InputFeedbackTxt.fontSizeMax = _canvasModel.InputfontSizesForVerticalScreen.y;
            }
        }

        private void OnMoveNextPressed()
        {
            switch (_currentState)
            {
                case MethodicStates.Instruction:
                    SetState(MethodicStates.FirstMetodic);
                    break;
                case MethodicStates.DataInput:
                    SetState(MethodicStates.DataRequestCall);
                    break;
            }
        }

        private void SetState(MethodicStates state)
        {
            _currentState = state;
            switch (_currentState)
            {
                case MethodicStates.Instruction:
                    break;
                case MethodicStates.FirstMetodic:
                    _canvasModel.MoveNextBtn.gameObject.SetActive(false);
                    _mainMethodicQuestion = 0;
                    SetMainMethodicQuestion(_mainMethodicQuestion);
                    UpdateFirstMethodicFillable();
                    SwitchCanvasGroups(_canvasModel.InstructionCanvasGroup, _canvasModel.MainMethodicCanvasGroup);
                    break;
                case MethodicStates.SecondMetodic:
                    _secondMethodicQuestion = 0;
                    SetSecondMethodicQuestion(_secondMethodicQuestion);
                    UpdateSecondMethodicFillable();
                    SwitchCanvasGroups(_canvasModel.MainMethodicCanvasGroup, _canvasModel.SecondMethodicCanvasGroup);
                    break;
                case MethodicStates.DataInput:
                    SwitchCanvasGroups(_canvasModel.SecondMethodicCanvasGroup, _canvasModel.InputCanvasGroup);
                    _canvasModel.MoveNextBtnTxt.text = "ÇÀÂÅÐØÈÒÜ";
                    _canvasModel.MoveNextBtn.gameObject.SetActive(true);
                    break;
                case MethodicStates.DataRequestCall:
                    MakeRequestCall();
                    break;
            }
        }

        private void SetMainMethodicQuestion(int questionIndex)
        {
            _canvasModel.FirstMethodicHightParameterTxt.text = Data.MethodicData.MethodicElements[questionIndex].HightParameter;
            _canvasModel.FirstMethodicLowParameterTxt.text = Data.MethodicData.MethodicElements[questionIndex].LowParameter;
        }

        private void SetSecondMethodicQuestion(int questionIndex)
        {
            _canvasModel.SecondMethodicParameterTxt.text = Data.MethodicData.MethodicQuestions[questionIndex];
        }

        private void UpdateFirstMethodicFillable()
        {
            _canvasModel.FirstMetodicFillableImg.fillAmount =
                (float)(((float)(_mainMethodicQuestion + 1) / (float)Data.MethodicData.MethodicElements.Length));
            _canvasModel.FirstMethodicFillableImgTxt.text =
                $"Âîïðîñ {_mainMethodicQuestion + 1} / {Data.MethodicData.MethodicElements.Length}";
        }

        private void UpdateSecondMethodicFillable()
        {
            _canvasModel.SecondMetodicFillableImg.fillAmount =
                (float)((float)(_secondMethodicQuestion + 1) / (float)Data.MethodicData.MethodicQuestions.Length);
            _canvasModel.SecondMethodicFillableImgTxt.text =
                $"Âîïðîñ {_secondMethodicQuestion + 1} / {Data.MethodicData.MethodicQuestions.Length}";
        }

        private void OnMainMethodicButtonPressed(int points)
        {
            Debug.Log("main pressed " + points);
            if (_currentState != MethodicStates.FirstMetodic) return;
            if (_mainMethodicQuestion >= Data.MethodicData.MethodicElements.Length - 1)
            {
                SetState(MethodicStates.SecondMetodic);
            }
            else
            {
                SetMainMethodicQuestion(_mainMethodicQuestion);
                _mainMethodicQuestion++;
                UpdateFirstMethodicFillable();
            }
        }

        private void OnSecondMethodicButtonPressed(int points)
        {
            Debug.Log("second pressed " + points);
            if (_currentState != MethodicStates.SecondMetodic) return;
            if (_secondMethodicQuestion >= Data.MethodicData.MethodicQuestions.Length - 1)
            {
                SetState(MethodicStates.DataInput);
            }
            else
            {
                SetSecondMethodicQuestion(_secondMethodicQuestion);
                _secondMethodicQuestion++;
                UpdateSecondMethodicFillable();
            }
        }

        private void SwitchCanvasGroups(CanvasGroup closingGroup, CanvasGroup openingGroup)
        {
            _closingGroup = closingGroup;
            _openingGroup = openingGroup;
            _openingGroup.gameObject.SetActive(true);
            _canvasGroupSequence = DOTween.Sequence(this);
            _canvasGroupSequence.Append(_closingGroup.DOFade(0f, _canvasModel.SwitchScreenDuration));
            _canvasGroupSequence.Join(_openingGroup.DOFade(1f, _canvasModel.SwitchScreenDuration));
            _canvasGroupSequence.OnComplete(OnCanvasGroupSwitched);
        }

        private void OnCanvasGroupSwitched()
        {
            _closingGroup.gameObject.SetActive(false);
        }

        private void MakeRequestCall()
        {
            _canvasModel.PreloaderPanel.gameObject.SetActive(true);
            DOVirtual.DelayedCall(5f, MoveToNextScene);
        }

        private void OnRequestCallback()
        {

        }

        private void MoveToNextScene()
        {
            SceneStateMachine.Instance.SetState(SceneStateNames.Thanks);
        }

        #endregion
    }
}