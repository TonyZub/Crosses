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

        private Dictionary<CellType, CellData> _cellDatas;

        #endregion


        #region Properties

        public CellType[] CellDatasKeys => _cellDatas.Keys.ToArray();
        public CellType[] AvaliableCells => _cellDatas.Where(x => x.Value == null).Select(x => x.Key).ToArray();
        public CellData[] CellDatas => _cellDatas.Values.ToArray();
        public CellData[] PlayerCells { get; private set; }
        public CellData[] ComputerCells { get; private set; }

        #endregion


        #region Constructor

        public GameFieldController(GameplayCanvasModel canvasModel, MarkChoiseController markChoiseController, 
            RoundController roundController)
        {
            _canvasModel = canvasModel;
            _markChoiseController = markChoiseController;
            _roundController = roundController;
            _aiController = new AIController(_canvasModel, _roundController, this, _markChoiseController);
            ClearCells();
            SubscribeEvents();
        }

        #endregion


        #region Methods

        private void ClearCellsDictionary()
        {
            _cellDatas = new Dictionary<CellType, CellData>();
            _cellDatas.Add(CellType.TopLeft, null);
            _cellDatas.Add(CellType.TopCenter, null);
            _cellDatas.Add(CellType.TopRight, null);
            _cellDatas.Add(CellType.CenterLeft, null);
            _cellDatas.Add(CellType.CenterCenter, null);
            _cellDatas.Add(CellType.CenterRight, null);
            _cellDatas.Add(CellType.BotLeft, null);
            _cellDatas.Add(CellType.BotCenter, null);
            _cellDatas.Add(CellType.BotRight, null);
        }

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            _roundController.RoundStarted += ClearCells;
            _roundController.ComputerTurnStarted += DisableGridInteraction;
            _roundController.ComputerTurnEnded += EnableGridInteraction;
            _markChoiseController.MarkChosen += OnMarkChosenByPlayer;
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
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Pressed -= OnCellPressedByPlayer;
            }
            _aiController.ComputerChoseCell -= OnCellChosenByComputer;
        }

        private void OnCellPressedByPlayer(GameCellModel cell)
        {
            MessageLogger.Log(_roundController.IsPlayersTurn);
            if (!_roundController.IsPlayersTurn) return;
            _cellDatas[cell.CellType] = new CellData(_canvasModel.Cells.First(x => x.CellType == cell.CellType),
                GameSides.Player, _markChoiseController.PlayerMark);
            RenderCells();
            UpdatePlayersCells();
            PlayerChoseCell?.Invoke();
        }

        private void OnCellChosenByComputer(CellData cell)
        {
            _cellDatas[cell.CellType] = cell;
            RenderCells();
            UpdateComputersCells();
            ComputerChoseCell?.Invoke();
        }

        private void OnMarkChosenByPlayer()
        {
            for (int i = 0; i < CellDatasKeys.Length; i++)
            {
                if (_cellDatas[CellDatasKeys[i]] == null) continue;
                _cellDatas[CellDatasKeys[i]] = new CellData(_cellDatas[CellDatasKeys[i]],
                    _cellDatas[CellDatasKeys[i]].IsPlayersMark ? _markChoiseController.PlayerMark :
                        _markChoiseController.ComputerMark);
            }
            RenderCells();
        }

        private void RenderCells()
        {
            foreach (var cell in _cellDatas)
            {
                var cellInModel = _canvasModel.Cells.First(x => x.CellType == cell.Key);
                cellInModel.Image.sprite = cell.Value == null ? null : cell.Value.CellMark == CellMarks.Cross ? 
                    Data.UIData.CrossSprite : Data.UIData.NoughtSprite;
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
                cell.Button.interactable = doEnable;
            }
        }

        private void UpdatePlayersCells()
        {
            //PlayerCells = CellDatas.Where(x => x.WhosMark == GameSides.Player).ToArray();
        }

        private void UpdateComputersCells()
        {
            //ComputerCells = CellDatas.Where(x => x.WhosMark == GameSides.Computer).ToArray();
        }

        public bool HasCellData(CellType cellType)
        {
            return _cellDatas[cellType] != null;
        }

        public bool HasCellData(CellType cellType, out CellData data)
        {
            data = _cellDatas[cellType];
            return HasCellData(cellType);
        }

        #endregion
    }
}