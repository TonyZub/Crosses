namespace Crosses
{
    public sealed class WelcomeScreenController
    {
        #region Fields

        private readonly WelcomeScreenCanvasModel _canvasModel;

        #endregion


        #region Properties

        public WelcomeScreenCanvasModel CanvasModel => _canvasModel;

        #endregion


        #region Constructor

        public WelcomeScreenController(WelcomeScreenCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.AddListener(OnMoveNextPressed);
            SceneStateMachine.Instance.OnBeforeStateChange += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            _canvasModel.MoveNextBtn.onClick.RemoveAllListeners();
            SceneStateMachine.Instance.OnBeforeStateChange -= UnsubscribeEvents;
        }

        private void OnMoveNextPressed()
        {
            SceneStateMachine.Instance.SetState(SceneStateNames.Gameplay);
        }

        #endregion
    }
}

