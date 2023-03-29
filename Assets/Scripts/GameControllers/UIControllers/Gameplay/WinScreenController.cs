using DG.Tweening;
using System;


namespace Crosses
{
	public sealed class WinScreenController
	{
        #region Constants

        private const string PLAYER_WON_TEXT = "¬€ ¬€»√–¿À»!";
        private const string COMPUTER_WON_TEXT = "¬€ œŒœ”Ÿ≈Õ€!";
        private const string DRAW_TEXT = "Õ»◊‹ﬂ!";

        private const float MIN_GROUP_ALPHA = 0f;
        private const float MAX_GROUP_ALPHA = 1f;

        #endregion


        #region Events

        public event Action WinScreenOpening;
        public event Action WinScreenOpened;
        public event Action WinScreenClosing;
        public event Action WinScreenClosed;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly RoundController _roundController;

        private Tween _winScreenTween;

        #endregion


        #region Constructor

        public WinScreenController(GameplayCanvasModel canvasModel, RoundController roundController)
        {
            _canvasModel = canvasModel;
            _roundController = roundController;
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            _roundController.PlayerWon += ShowPlayerWinScreen;
            _roundController.ComputerWon += ShowComputerWinScreen;
            _roundController.Draw += ShowDrawScreen;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            _roundController.PlayerWon -= ShowPlayerWinScreen;
            _roundController.ComputerWon -= ShowComputerWinScreen;
            _roundController.Draw -= ShowDrawScreen;
        }

        private void ShowPlayerWinScreen()
        {
            _canvasModel.WinPanelModel.WinTxt.text = PLAYER_WON_TEXT;
            ShowScreen();
        }

        private void ShowComputerWinScreen()
        {
            _canvasModel.WinPanelModel.WinTxt.text = COMPUTER_WON_TEXT;
            ShowScreen();
        }

        private void ShowDrawScreen()
        {
            _canvasModel.WinPanelModel.WinTxt.text = DRAW_TEXT;
            ShowScreen();
        }

        private void ShowScreen()
        {
            _canvasModel.WinPanelModel.gameObject.SetActive(true);
            _winScreenTween = _canvasModel.WinPanelModel.Group.DOFade(MAX_GROUP_ALPHA,
                _canvasModel.WinPanelModel.TimeToAppear).OnComplete(OnScreenShown);
            WinScreenOpening?.Invoke();
        }

        private void OnScreenShown()
        {
            _winScreenTween = DOVirtual.DelayedCall(_canvasModel.WinPanelModel.ShowingTime, HideWinScreen);
            WinScreenOpened?.Invoke();
        }

        private void HideWinScreen()
        {
            _winScreenTween = _canvasModel.WinPanelModel.Group.DOFade(MIN_GROUP_ALPHA,
                _canvasModel.WinPanelModel.TimeToDisappear).OnComplete(OnScreenHidden);
            WinScreenClosing?.Invoke();
        }

        private void OnScreenHidden()
        {
            _canvasModel.WinPanelModel.gameObject.SetActive(false);
            WinScreenClosed?.Invoke();
        }

        #endregion
    }
}