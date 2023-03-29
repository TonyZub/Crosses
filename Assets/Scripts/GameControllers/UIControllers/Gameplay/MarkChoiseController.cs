using DG.Tweening;
using System;


namespace Crosses
{
    public sealed class MarkChoiseController
    {
        #region PrivateData

        private const float MIN_GROUP_ALPHA = 0f;
        private const float MAX_GROUP_ALPHA = 1f;

        #endregion


        #region Events

        public event Action MarkChosen;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;

        private Tween _markChoisePanelTween;

        #endregion


        #region Properties

        public CellMarks PlayerMark { get; private set; } = CellMarks.None;
        public CellMarks ComputerMark { get; private set; } = CellMarks.None;
        public bool IsMarkChoisePanelShown { get; private set; }

        #endregion


        #region Constructor

        public MarkChoiseController(GameplayCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            GetMarks();
            SubscribeEvents();
            ShowMarkChoisePanel();
        }

        #endregion


        #region Methods

        private void GetMarks()
        {
            var playerMark = PlayerDataSaver.GetSavedPlayerMark();
            if (playerMark != CellMarks.None)
            {
                PlayerMark = playerMark;
                ComputerMark = PlayerMark == CellMarks.Cross ? CellMarks.None : CellMarks.Cross;
            }
        }

        private void SubscribeEvents()
        {
            MessageLogger.Log("subcribing mark controller");
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
            _canvasModel.MarkChoisePanelModel.MarkChosen += OnPlayerMarkChose;
            _canvasModel.ChooseMarkBtn.onClick.AddListener(OnChooseMarkPressed);
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.OnBeforeStateChange -= UnsubscribeEvents;
            _canvasModel.MarkChoisePanelModel.MarkChosen -= OnPlayerMarkChose;
            _canvasModel.ChooseMarkBtn.onClick.RemoveAllListeners();
        }

        private void OnChooseMarkPressed()
        {
            ShowMarkChoisePanel();
        }

        private void OnPlayerMarkChose(CellMarks mark)
        {
            PlayerDataSaver.SavePlayerMarkChoise(mark);
            PlayerMark = mark;
            ComputerMark = PlayerMark == CellMarks.Cross ? CellMarks.None : CellMarks.Cross;
            MarkChosen?.Invoke();
            HideMarkChoisePanel();
        }

        public void ShowMarkChoisePanel()
        {
            _canvasModel.MarkChoisePanelModel.gameObject.SetActive(true);
            _markChoisePanelTween = DOVirtual.Float(_canvasModel.MarkChoisePanelModel.Group.alpha, MAX_GROUP_ALPHA,
                _canvasModel.MarkChoisePanelModel.ShowingTime, SetMarkChoisePanelAlpha);
            IsMarkChoisePanelShown = true;
        }

        public void HideMarkChoisePanel()
        {
            _markChoisePanelTween = DOVirtual.Float(_canvasModel.MarkChoisePanelModel.Group.alpha, MIN_GROUP_ALPHA,
                _canvasModel.MarkChoisePanelModel.ShowingTime, SetMarkChoisePanelAlpha).
                    OnComplete(() => _canvasModel.MarkChoisePanelModel.Group.gameObject.SetActive(false));
            IsMarkChoisePanelShown = false;
        }

        private void SetMarkChoisePanelAlpha(float value)
        {
            _canvasModel.MarkChoisePanelModel.Group.alpha = value;
        }

        #endregion
    }
}

