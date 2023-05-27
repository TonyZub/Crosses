using DG.Tweening;
using UnityEngine;


namespace Crosses
{
    public sealed class WelcomeScreenController
    {
        #region Fields

        private readonly WelcomeScreenCanvasModel _canvasModel;
        private readonly ScreenOrientationService _screenOrientationService;

        private Sequence _switchScreenSequence;
        private bool _isWelcomeTextSkipped;

        #endregion


        #region Properties

        public WelcomeScreenCanvasModel CanvasModel => _canvasModel;

        #endregion


        #region Constructor

        public WelcomeScreenController(WelcomeScreenCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            _screenOrientationService = GlobalContext.Instance.GetDependency<GlobalServices>().ScreenOrientationService;
            AdaptContainer(_screenOrientationService.ScreenOrientation);
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.AddListener(OnMoveNextPressed);
            _canvasModel.MaleToggle.onValueChanged.AddListener(OnMaleToggleValueChanged);
            _canvasModel.FemaleToggle.onValueChanged.AddListener(OnFemaleToggleValueChanged);
            _screenOrientationService.ScreenOrientationChanged += AdaptContainer;
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            DOTween.Kill(this);
            _canvasModel.MaleToggle.onValueChanged.RemoveAllListeners();
            _canvasModel.FemaleToggle.onValueChanged.RemoveAllListeners();
            _canvasModel.MoveNextBtn.onClick.RemoveAllListeners();
            _screenOrientationService.ScreenOrientationChanged -= AdaptContainer;
            SceneStateMachine.Instance.OnBeforeStateChange -= UnsubscribeEvents;
        }

        private void AdaptContainer(ScreenTypes screenType)
        {
            if(screenType == ScreenTypes.Horizontal)
            {
                _canvasModel.ContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.x, 
                    _canvasModel.ContainerPanel.anchorMin.y);
                _canvasModel.ContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForHorizontalScreen.y,
                    _canvasModel.ContainerPanel.anchorMax.y);
            }
            else
            {
                _canvasModel.ContainerPanel.anchorMin = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.x,
                    _canvasModel.ContainerPanel.anchorMin.y);
                _canvasModel.ContainerPanel.anchorMax = new Vector2(_canvasModel.ContainerAnchorsForVerticalScreen.y,
                    _canvasModel.ContainerPanel.anchorMax.y);
            }
        }

        private void OnMaleToggleValueChanged(bool isOn)
        {
            if (isOn) _canvasModel.FemaleToggle.isOn = !isOn;
        }

        private void OnFemaleToggleValueChanged(bool isOn)
        {
            if (isOn) _canvasModel.MaleToggle.isOn = !isOn;
        }

        private void OnMoveNextPressed()
        {
            if (_isWelcomeTextSkipped)
            {
                TrySetNextState();
            }
            else
            {
                SwitchScreens();
            }
        }

        private void TrySetNextState()
        {
            if (AreFieldsValidated())
            {
                _canvasModel.MoveNextBtn.interactable = false;
                SceneStateMachine.Instance.SetState(SceneStateNames.Gameplay);
            }
        }


        private bool AreFieldsValidated()
        {
            if (_canvasModel.NameInput.text.Equals(string.Empty))
            {
                _canvasModel.AlertText.text = "Введите свое имя или ник";
                return false;
            }
            if(!_canvasModel.MaleToggle.isOn && !_canvasModel.FemaleToggle.isOn)
            {
                _canvasModel.AlertText.text = "Выберите пол";
                return false;
            }
            if (_canvasModel.AgeInput.text.Equals(string.Empty)){
                _canvasModel.AlertText.text = "Введите свой возраст";
                return false;
            }
            _canvasModel.AlertText.text = string.Empty;
            return true;
        }

        private void SwitchScreens()
        {
            _canvasModel.MoveNextBtn.interactable = false;
            _canvasModel.DataCanvasGroup.gameObject.SetActive(true);
            _switchScreenSequence = DOTween.Sequence(this);
            _switchScreenSequence.Append(_canvasModel.WelcomeTextCanvasGroup.DOFade(0f, _canvasModel.SwitchScreenDuration));
            _switchScreenSequence.Join(_canvasModel.DataCanvasGroup.DOFade(1f, _canvasModel.SwitchScreenDuration));
            _switchScreenSequence.OnComplete(OnScreensSwitched);
        }

        private void OnScreensSwitched()
        {
            _canvasModel.WelcomeTextCanvasGroup.gameObject.SetActive(false);
            _canvasModel.MoveNextBtn.interactable = true;
            _isWelcomeTextSkipped = true;
        }

        #endregion
    }
}

