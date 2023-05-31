using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Crosses
{
	public sealed class MethodicScreenController
	{
        #region PrivateData

        private const string REQUEST_URL = "https://script.google.com/macros/s/AKfycbxenGhlPVwbgWQqMW3rzN8jIs5TuWVVUePvT0W2cR1SH-sPkOvBUhhUxDF-OAmVh2LS/exec?func=AddRecord";
        private const int MAX_METHODIC_POINT = 8;

        private enum MethodicStates
        {
            Instruction,
            FirstMetodic,
            SecondMetodic,
            DataInput,
            DataRequestCall
        }

        private class ObjectResponse
        {
            public bool isSuccess;
        }

        private struct MethodicPart
        {
            public MethodicElementsParts Part;
            public int Point;
            public bool IsReversed;

            public MethodicPart(MethodicElementsParts part, int point, bool isReversed)
            {
                Part = part;
                IsReversed = isReversed;
                Point = IsReversed ? MAX_METHODIC_POINT - point : point;
            }
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

        private List<MethodicPart> _mainMethodicAnswers;

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
            _mainMethodicAnswers = new List<MethodicPart>();
            AdaptContainer(_screenOrientationService.ScreenOrientation);
            SubscribeEvents();
            SetState(MethodicStates.Instruction);
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.AddListener(OnMoveNextPressed);
            _canvasModel.RequestResponse += OnResponse;
            _canvasModel.RequestError += ShowError;
            _screenOrientationService.ScreenOrientationChanged += AdaptContainer;
            for (int i = 0; i < _canvasModel.FirstMethodicButtons.Length; i++)
            {
                var index = _canvasModel.FirstMethodicButtons.Length - i;
                _canvasModel.FirstMethodicButtons[i].onClick.AddListener(() => OnMainMethodicButtonPressed(index));
            }
            for (int i = 0; i < _canvasModel.SecondMethodicButtons.Length; i++)
            {
                var index = _canvasModel.SecondMethodicButtons.Length - i - 1;
                _canvasModel.SecondMethodicButtons[i].onClick.AddListener(() => OnSecondMethodicButtonPressed(index));
            }
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            DOTween.Kill(this);
            _canvasModel.MoveNextBtn.onClick.RemoveAllListeners();
            _canvasModel.RequestResponse -= OnResponse;
            _canvasModel.RequestError -= ShowError;
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
                case MethodicStates.DataRequestCall:
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
                    StoreTotalForMainMethodic();
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
                    MakeDataRequest();
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
            if (_currentState != MethodicStates.FirstMetodic) return;
            _mainMethodicAnswers.Add(new MethodicPart(Data.MethodicData.MethodicElements[_mainMethodicQuestion].ElementPart,
                points, Data.MethodicData.MethodicElements[_mainMethodicQuestion].IsReversed));
            _researchDataService.AddFirstMethodicAnswer(points);
            if (_mainMethodicQuestion >= Data.MethodicData.MethodicElements.Length - 1)
            {
                SetState(MethodicStates.SecondMetodic);
            }
            else
            {
                _mainMethodicQuestion++;
                SetMainMethodicQuestion(_mainMethodicQuestion);
                UpdateFirstMethodicFillable();
            }
        }

        private float CountTotalForPart(MethodicElementsParts part)
        {
            var elements = _mainMethodicAnswers.Where(x => x.Part == part).ToArray();
            float totalPoints = 0f;
            for (int i = 0; i < elements.Length; i++)
            {
                totalPoints += elements[i].Point;
            }
            return totalPoints / 10f;
        }

        private void StoreTotalForMainMethodic()
        {
            _researchDataService.SetFirstMethodicResults(CountTotalForPart(MethodicElementsParts.Health),
                CountTotalForPart(MethodicElementsParts.Activity), CountTotalForPart(MethodicElementsParts.Mood));
        }

        private void OnSecondMethodicButtonPressed(int points)
        {
            if (_currentState != MethodicStates.SecondMetodic) return;
            _researchDataService.AddSecondMethodicAnswer(points);
            if (_secondMethodicQuestion >= Data.MethodicData.MethodicQuestions.Length - 1)
            {
                SetState(MethodicStates.DataInput);
            }
            else
            {
                _secondMethodicQuestion++;
                SetSecondMethodicQuestion(_secondMethodicQuestion);
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

        private void MakeDataRequest()
        {
            _canvasModel.PreloaderPanel.gameObject.SetActive(true);
            _canvasModel.GenerateRequest(REQUEST_URL, _researchDataService.GetDataJSON());
        }

        private void OnResponse(string jsonResponse)
        {
            try
            {
                var parsedResponse = JsonUtility.FromJson<ObjectResponse>(jsonResponse);
                if (parsedResponse.isSuccess)
                {
                    MoveToNextScene();
                }
                else
                {
                    ShowError(string.Empty);
                }
            }
            catch (Exception e)
            {
                ShowError(string.Empty);
            }
        }

        private void MoveToNextScene()
        {
            _canvasModel.ErrorTxtPanel.gameObject.SetActive(false);
            SceneStateMachine.Instance.SetState(SceneStateNames.Thanks);
        }

        private void ShowError(string message)
        {
            _canvasModel.ErrorTxtPanel.gameObject.SetActive(true);
            _canvasModel.PreloaderPanel.gameObject.SetActive(false);
        }

        #endregion
    }
}