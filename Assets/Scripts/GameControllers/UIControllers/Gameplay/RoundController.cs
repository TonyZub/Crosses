using DG.Tweening;
using System;


namespace Crosses
{
	public sealed class RoundController
	{
        #region PrivateData

        private const float DELAY_BEFORE_FIRST_START = 2f;

        #endregion


        #region Events

        public event Action RoundStarted;
        public event Action PlayerTurnStarted;
        public event Action PlayerTurnEnded;
        public event Action ComputerTurnStarted;
        public event Action ComputerTurnEnded;
        public event Action PlayerWon;
        public event Action ComputerWon;
        public event Action<CellType[]> GotPlayerWinCombination;
        public event Action<CellType[]> GotComputerWinCombination;
        public event Action Draw;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly MarkChoiseController _markChoiseController;
        private readonly GameFieldController _gameFieldController;
        private readonly WinScreenController _winScreenController;

        private Tween _roundTween;

        #endregion


        #region Properties

        public int RoundIndex { get; private set; }
        public int TurnIndex { get; private set; }
        public bool IsPlayersTurn { get; private set; }

        #endregion


        #region Constructor

        public RoundController(GameplayCanvasModel canvasModel, MarkChoiseController markChoiseController)
        {
            _canvasModel = canvasModel;
            _markChoiseController = markChoiseController;
            _gameFieldController = new GameFieldController(_canvasModel, _markChoiseController, this);
            _winScreenController = new WinScreenController(_canvasModel, this);
            SubcribeEvents();
        }

        #endregion


        #region Methods

        private void SubcribeEvents()
        {
            _markChoiseController.MarkChosen += OnMarkChosen;
            _gameFieldController.PlayerChoseCell += OnPlayerTurnEnded;
            _gameFieldController.ComputerChoseCell += OnComputerTurnEnded;
            _winScreenController.WinScreenClosed += StartRound;
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            _gameFieldController.PlayerChoseCell -= OnPlayerTurnEnded;
            _gameFieldController.ComputerChoseCell -= OnComputerTurnEnded;
            _winScreenController.WinScreenClosed -= StartRound;
        }

        private void OnMarkChosen()
        {
            _markChoiseController.MarkChosen -= OnMarkChosen;
            _roundTween = DOVirtual.DelayedCall(DELAY_BEFORE_FIRST_START, StartRound);
        }

        private void StartRound()
        {
            IsPlayersTurn = !_canvasModel.IsComputerFirstTurn;
            RoundIndex += 1;
            TurnIndex = 1;
            RoundStarted?.Invoke();
            MakeFirstTurn();
        }

        private void StartComputerTurn()
        {
            ComputerTurnStarted?.Invoke();
        }

        private void OnComputerTurnEnded()
        {
            ComputerTurnEnded?.Invoke();
            ContinueRound();
        }

        private void StartPlayerTurn()
        {
            PlayerTurnStarted?.Invoke();
        }

        private void OnPlayerTurnEnded()
        {
            PlayerTurnEnded?.Invoke();
            ContinueRound();
        }

        private void ContinueRound()
        {
            if(WinCheckerService.IsEndGame(_gameFieldController, out CellType[] winCombination, out GameSides winner))
            {
                switch (winner)
                {
                    case GameSides.None:
                        Draw?.Invoke();
                        break;
                    case GameSides.Player:
                        GotPlayerWinCombination?.Invoke(winCombination);
                        PlayerWon?.Invoke();
                        break;
                    case GameSides.Computer:
                        GotComputerWinCombination?.Invoke(winCombination);
                        ComputerWon?.Invoke();
                        break;
                    default:
                        break;
                }            
                return;
            }
            MakeNextTurn();
        }

        private void MakeFirstTurn()
        {
            DecideTurn();
        }

        private void MakeNextTurn()
        {
            TurnIndex++;
            IsPlayersTurn = !IsPlayersTurn;
            DecideTurn();
        }

        private void DecideTurn()
        {
            if (IsPlayersTurn)
            {
                StartPlayerTurn();
            }
            else
            {
                StartComputerTurn();
            }
        }

        #endregion
    }
}