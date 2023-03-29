namespace Crosses
{
	public sealed class GameplayController
    {
        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly MarkChoiseController _markChoiseController;
        private readonly RoundController _roundController;

        #endregion


        #region Properties

        public GameplayCanvasModel CanvasModel => _canvasModel;
        public MarkChoiseController MarkChoiseController => _markChoiseController;
        public RoundController RoundController => _roundController;

        #endregion


        #region Constructor

        public GameplayController(GameplayCanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            _markChoiseController = new MarkChoiseController(_canvasModel);
            _roundController = new RoundController(_canvasModel, _markChoiseController);
        }

        #endregion
    }
}