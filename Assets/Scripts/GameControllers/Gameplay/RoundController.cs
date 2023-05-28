using DG.Tweening;
using System;
using Random = UnityEngine.Random;


namespace Crosses
{
	public sealed class RoundController
	{
        #region Events

        public event Action RoundStarted;
        public event Action RoundRestarted;
        public event Action PlayerTurnStarted;
        public event Action PlayerTurnEnded;
        public event Action ComputerTurnStarted;
        public event Action ComputerTurnEnded;
        public event Action PlayerWon;
        public event Action ComputerWon;
        public event Action<WinCombinations> GotPlayerWinCombination;
        public event Action<WinCombinations> GotComputerWinCombination;
        public event Action Draw;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly MarkChoiseController _markChoiseController;
        private readonly GameFieldController _gameFieldController;
        private readonly WinScreenController _winScreenController;
        private readonly CheatController _cheatController;
        private readonly ResearchDataService _researchDataService;

        private Tween _roundTween;
        private Tween _cheatMessageTween;

        private DateTime _roundStartTime;

        #endregion


        #region Properties

        public GameSides FirstTurnSide { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public int RoundIndex { get; private set; }
        public int TurnIndex { get; private set; }
        public int PlayerWinAmount { get; private set; }
        public int ComputerWinAmounts { get; private set; }
        public int DrawAmounts { get; private set; }
        public bool IsPlayersTurn { get; private set; }

        #endregion


        #region Constructor

        public RoundController(GameplayCanvasModel canvasModel, MarkChoiseController markChoiseController)
        {
            _canvasModel = canvasModel;
            _markChoiseController = markChoiseController;
            _gameFieldController = new GameFieldController(_canvasModel, _markChoiseController, this);
            _winScreenController = new WinScreenController(_canvasModel, this);
            _cheatController = new CheatController();
            _researchDataService = GlobalContext.Instance.GetDependency<GlobalServices>().ResearchDataService;
            FirstTurnSide = _canvasModel.FirstTurnSide;
            Difficulty = _canvasModel.Difficulty;
            SubcribeEvents();
        }

        #endregion


        #region Methods

        private void SubcribeEvents()
        {
            _canvasModel.GiveUpBtn.onClick.AddListener(Surrender);
            _markChoiseController.MarkChosen += OnMarkChosen;
            _gameFieldController.PlayerChoseCell += OnPlayerTurnEnded;
            _gameFieldController.ComputerChoseCell += OnComputerTurnEnded;
            _winScreenController.WinScreenClosed += StartRound;
            _cheatController.ChangedFirstTurn += OnFirstTurnChangedByCheat;
            _cheatController.RoundRestarted += RestartRound;
            _cheatController.DifficultyChanged += ChangeDifficulty;
            RoundStarted += PrintRoundInfo;
            PlayerTurnStarted += PrintRoundInfo;
            PlayerTurnEnded += PrintRoundInfo;
            ComputerTurnStarted += PrintRoundInfo;
            ComputerTurnEnded += PrintRoundInfo;
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            RoundStarted -= PrintRoundInfo;
            PlayerTurnStarted -= PrintRoundInfo;
            PlayerTurnEnded -= PrintRoundInfo;
            ComputerTurnStarted -= PrintRoundInfo;
            ComputerTurnEnded -= PrintRoundInfo;
            _cheatController.DifficultyChanged -= ChangeDifficulty;
            _cheatController.RoundRestarted -= RestartRound;
            _cheatController.ChangedFirstTurn -= OnFirstTurnChangedByCheat;
            _gameFieldController.PlayerChoseCell -= OnPlayerTurnEnded;
            _gameFieldController.ComputerChoseCell -= OnComputerTurnEnded;
            _winScreenController.WinScreenClosed -= StartRound;
            _canvasModel.GiveUpBtn.onClick.RemoveAllListeners();
        }

        private void OnMarkChosen()
        {
            _markChoiseController.MarkChosen -= OnMarkChosen;
            _roundTween = DOVirtual.DelayedCall(_canvasModel.DelayBeforeFirstStart, StartRound);
        }

        private void OnFirstTurnChangedByCheat(GameSides firstTurn)
        {
            FirstTurnSide = firstTurn;
            var currentMessage = _canvasModel.RoundTxt.text;
            switch (FirstTurnSide)
            {
                case GameSides.None:
                    PrintMessage("Теперь тот кто ходит первым определяется случайно");
                    break;
                case GameSides.Player:
                    PrintMessage("Теперь первым всегда ходит игрок");
                    break;
                case GameSides.Computer:
                    PrintMessage("Теперь первым всегда ходит компьютер");
                    break;
                default:
                    break;
            }
            _cheatMessageTween = DOVirtual.DelayedCall(CheatController.MESSAGE_DELAY_TIME, PrintRoundInfo);
        }

        private void PrintMessage(string message)
        {
            if (_cheatMessageTween != null && _cheatMessageTween.IsPlaying()) _cheatMessageTween.Kill();
            _canvasModel.RoundTxt.text = message;
        }

        private void PrintRoundInfo()
        {
            PrintMessage($"Раунд - {RoundIndex} {(IsPlayersTurn ? "Ваш ход" : "Ход компьютера")} \n" +
                $"Выиграно: {PlayerWinAmount} Проиграно: {ComputerWinAmounts} Ничьих: {DrawAmounts}");
        }

        private void StartRound()
        {
            _roundStartTime = DateTime.Now;
            DecideWhosTurnFirst();
            RoundIndex += 1;
            CheckIfCanSurrender();
            TurnIndex = 0;
            RoundStarted?.Invoke();
            MakeFirstTurn();
        }

        private void CheckIfCanSurrender()
        {
            if(RoundIndex > _canvasModel.MinRounds)
            {
                _canvasModel.GiveUpBtn.gameObject.SetActive(true);
            }
        }

        private void Surrender()
        {
            _researchDataService.CountRoundsInfo();
            if(Random.value > 0.5f)
            {
                SceneStateMachine.Instance.SetState(SceneStateNames.Methodic);
            }
            else
            {
                SceneStateMachine.Instance.SetState(SceneStateNames.Transition);
            }
        }

        private void RestartRound()
        {
            DecideWhosTurnFirst();
            TurnIndex = 0;
            RoundRestarted?.Invoke();
            MakeFirstTurn();
        }

        private void ChangeDifficulty()
        {
            Difficulty = Difficulty == Difficulty.Impossible ? Difficulty.Random : Difficulty.Impossible;
            PrintMessage($"Выбрана сложность: {(Difficulty == Difficulty.Impossible ? "попускательная" : "чиловая")}");
            _cheatMessageTween = DOVirtual.DelayedCall(CheatController.MESSAGE_DELAY_TIME, PrintRoundInfo);
        }

        private void DecideWhosTurnFirst()
        {
            switch (FirstTurnSide)
            {
                case GameSides.None:
                    IsPlayersTurn = UnityEngine.Random.value > 0.5f;
                    break;
                case GameSides.Player:
                    IsPlayersTurn = true;
                    break;
                case GameSides.Computer:
                    IsPlayersTurn = false;
                    break;
                default:
                    break;
            }
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
            if(FieldAnalyzer.IsEndGame(_gameFieldController, out WinCombinations winCombination, out GameSides winner))
            {
                var roundTime = (float)(DateTime.Now - _roundStartTime).TotalSeconds;
                switch (winner)
                {
                    case GameSides.None:
                        _researchDataService.AddRoundInfo(GameSides.None, roundTime);
                        DrawAmounts++;
                        Draw?.Invoke();
                        break;
                    case GameSides.Player:
                        _researchDataService.AddRoundInfo(GameSides.Player, roundTime);
                        PlayerWinAmount++;
                        GotPlayerWinCombination?.Invoke(winCombination);
                        PlayerWon?.Invoke();
                        break;
                    case GameSides.Computer:
                        _researchDataService.AddRoundInfo(GameSides.Computer, roundTime);
                        ComputerWinAmounts++;
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