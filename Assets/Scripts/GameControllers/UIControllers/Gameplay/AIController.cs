using System;
using Extensions;
using System.Linq;
using DG.Tweening;
using Random = UnityEngine.Random;


namespace Crosses
{
    public sealed class AIController
    {
        #region PrivateData

        private const float MIN_COMPUTER_THINKING_TIME = 1f;
        private const float MAX_COMPUTER_THINKING_TIME = 5f;

        private enum FirstTurn
        {
            Center,
            Diagonal,
            Side
        }

        #endregion


        #region Events

        public event Action<CellData> ComputerChoseCell;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly RoundController _roundController;
        private readonly GameFieldController _gameFieldController;
        private readonly FieldAnalyzer _analyzer;

        #endregion


        #region Constructor

        public AIController(GameplayCanvasModel canvasModel, RoundController roundController, 
            GameFieldController gameFieldController)
        {
            _canvasModel = canvasModel;
            _roundController = roundController;
            _gameFieldController = gameFieldController;
            _analyzer = new FieldAnalyzer(_gameFieldController);
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            _roundController.ComputerTurnStarted += OnComputerTurnStarted;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            _roundController.ComputerTurnStarted -= OnComputerTurnStarted;
        }

        private void OnComputerTurnStarted()
        {
            switch (_canvasModel.Difficulty)
            {
                case Difficulty.None:
                    break;
                case Difficulty.Random:
                    DOVirtual.DelayedCall(GetThinkingTime(), MakeTurnEasyDifficulty);
                    break;
                case Difficulty.Impossible:
                    DOVirtual.DelayedCall(GetThinkingTime(), MakeTurnImpossibleDifficulty);
                    break;
                default:
                    break;
            }
        }

        private void MakeTurnEasyDifficulty()
        {
            ComputerChoseCell?.Invoke(_gameFieldController.AvaliableCellDatas.RandomObject());
        }

        private void MakeTurnImpossibleDifficulty()
        {     
            ComputerChoseCell?.Invoke(_analyzer.GetBestMove(_gameFieldController, GameSides.Computer, 
                _roundController.TurnIndex));
        }

        private float GetThinkingTime()
        {
            return Random.Range(MIN_COMPUTER_THINKING_TIME, MAX_COMPUTER_THINKING_TIME);
        }

        #endregion
    }
}