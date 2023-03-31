using System;
using System.Collections.Generic;
using System.Linq;


namespace Crosses
{
	public sealed class GameFieldController
	{
        #region Events

        public event Action PlayerChoseCell;
        public event Action ComputerChoseCell;

        #endregion


        #region Fields

        private readonly GameplayCanvasModel _canvasModel;
        private readonly RoundController _roundController;
        private readonly MarkChoiseController _markChoiseController;
        private readonly AIController _aiController;

        private readonly Dictionary<CellData, GameSides> _cellDatas = new Dictionary<CellData, GameSides>()
        {
            { new CellData(CellType.TopLeft, 1, 1, true, false), GameSides.None },
            { new CellData(CellType.TopCenter, 1, 2, false, false), GameSides.None },
            { new CellData(CellType.TopRight, 1, 3, false, true), GameSides.None },
            { new CellData(CellType.CenterLeft, 2, 1, false, false), GameSides.None },
            { new CellData(CellType.CenterCenter, 2, 2, true, true), GameSides.None },
            { new CellData(CellType.CenterRight, 2, 3, false, false), GameSides.None },
            { new CellData(CellType.BotLeft, 3, 1, false, true), GameSides.None },
            { new CellData(CellType.BotCenter, 3, 2, false, false), GameSides.None },
            { new CellData(CellType.BotRight, 3, 3, true, false), GameSides.None }
        };

        #endregion


        #region Properties

        public CellData[] AllCellDatas => _cellDatas.Select(x => x.Key).ToArray();
        public CellData[] AvaliableCellDatas => _cellDatas.Where(x => x.Value == GameSides.None).Select(x => x.Key).ToArray();
        public CellData[] MarkedCellDatas => _cellDatas.Where(x => x.Value != GameSides.None).Select(x => x.Key).ToArray();
        public CellData[] PlayerCells => _cellDatas.Where(x => x.Value == GameSides.Player).Select(x => x.Key).ToArray();
        public CellData[] ComputerCells => _cellDatas.Where(x => x.Value == GameSides.Computer).Select(x => x.Key).ToArray();

        #endregion


        #region Constructor

        public GameFieldController(GameplayCanvasModel canvasModel, MarkChoiseController markChoiseController, 
            RoundController roundController)
        {
            _canvasModel = canvasModel;
            _markChoiseController = markChoiseController;
            _roundController = roundController;
            _aiController = new AIController(_canvasModel, _roundController, this);
            ClearCells();
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void ClearCellsDictionary()
        {
            foreach (var cellDataKeys in _cellDatas.Keys.ToList())
            {
                _cellDatas[cellDataKeys] = GameSides.None;
            }
        }

        private void SetCellSide(CellType cellType, GameSides side)
        {
            _cellDatas[_cellDatas.Keys.First(x => x.CellType == cellType)] = side;
        }

        private bool IsCellTypeAvaliable(CellType type)
        {
            return AvaliableCellDatas.FirstOrDefault(x => x.CellType == type) != null;
        }

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            _roundController.RoundStarted += ClearCells;
            _roundController.ComputerTurnStarted += DisableGridInteraction;
            _roundController.ComputerTurnEnded += EnableGridInteraction;
            _markChoiseController.MarkChosen += OnMarkChosenByPlayer;
            _roundController.ComputerWon += DisableGridInteraction;
            _roundController.PlayerTurnEnded += DisableGridInteraction;
            _roundController.Draw += DisableGridInteraction;
            _roundController.Draw += RenderDraw;
            _roundController.GotPlayerWinCombination += RenderPlayerWinCombination;
            _roundController.GotComputerWinCombination += RenderComputerWinCombination;
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Pressed += OnCellPressedByPlayer;
            }
            _aiController.ComputerChoseCell += OnCellChosenByComputer;
        }

        private void UnsubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging -= UnsubscribeEvents;
            _roundController.RoundStarted -= ClearCells;
            _roundController.ComputerTurnStarted -= DisableGridInteraction;
            _roundController.ComputerTurnEnded -= EnableGridInteraction;
            _markChoiseController.MarkChosen -= OnMarkChosenByPlayer;
            _roundController.ComputerWon -= DisableGridInteraction;
            _roundController.PlayerTurnEnded -= DisableGridInteraction;
            _roundController.Draw -= DisableGridInteraction;
            _roundController.Draw -= RenderDraw;
            _roundController.GotPlayerWinCombination -= RenderPlayerWinCombination;
            _roundController.GotComputerWinCombination -= RenderComputerWinCombination;
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Pressed -= OnCellPressedByPlayer;
            }
            _aiController.ComputerChoseCell -= OnCellChosenByComputer;
        }

        private void OnCellPressedByPlayer(GameCellModel cell)
        {
            if (!_roundController.IsPlayersTurn) return;
            SetCellSide(cell.CellType, GameSides.Player);
            RenderCells();
            PlayerChoseCell?.Invoke();
        }

        private void OnCellChosenByComputer(CellData cell)
        {
            SetCellSide(cell.CellType, GameSides.Computer);
            RenderCells();
            ComputerChoseCell?.Invoke();
        }

        private void OnMarkChosenByPlayer()
        {
            RenderCells();
        }

        private void RenderCells()
        {
            foreach (var cell in _cellDatas)
            {
                var cellInModel = _canvasModel.Cells.First(x => x.CellType == cell.Key.CellType);

                switch (cell.Value)
                {
                    case GameSides.None:
                        cellInModel.Image.sprite = Data.UIData.GetMark(CellMarks.None);
                        cellInModel.Image.color = _canvasModel.SimpleCellColor;
                        break;
                    case GameSides.Player:
                        cellInModel.Image.sprite = Data.UIData.GetMark(_markChoiseController.PlayerMark);
                        cellInModel.Image.color = _canvasModel.PlayerCellColor;
                        break;
                    case GameSides.Computer:
                        cellInModel.Image.sprite = Data.UIData.GetMark(_markChoiseController.ComputerMark);
                        cellInModel.Image.color = _canvasModel.ComputerCellColor;
                        break;
                    default:
                        break;
                }
            }
        }

        private void RenderPlayerWinCombination(CellType[] combination)
        {
            foreach (var cell in combination)
            {
                _canvasModel.Cells.First(x => x.CellType == cell).Image.color = _canvasModel.PlayerWinCellColor;
            }
        }

        private void RenderComputerWinCombination(CellType[] combination)
        {
            foreach (var cell in combination)
            {
                _canvasModel.Cells.First(x => x.CellType == cell).Image.color = _canvasModel.ComputerWinCellColor;
            }
        }

        private void RenderDraw()
        {
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Image.color = _canvasModel.SimpleCellColor;
            }
        }

        private void ClearCells()
        {
            ClearCellsDictionary();
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Image.sprite = null;
            }
        }

        private void DisableGridInteraction()
        {
            ChangeGridInteraction(false);
        }

        private void EnableGridInteraction()
        {
            ChangeGridInteraction(true);
        }

        private void ChangeGridInteraction(bool doEnable)
        {
            foreach (var cell in _canvasModel.Cells)
            {
                if(doEnable && IsCellTypeAvaliable(cell.CellType) || !doEnable)
                    cell.Button.interactable = doEnable;
            }
        }

        public CellData GetCellByType(CellType type)
        {
            return _cellDatas.First(x => x.Key.CellType == type).Key;
        }

        public GameSides GetMarkForCell(CellData cellData)
        {
            return _cellDatas[cellData];
        }

        public GameSides GetMarkForCell(CellType cellType)
        {
            return _cellDatas[GetCellByType(cellType)];
        }

        #endregion
    }
}