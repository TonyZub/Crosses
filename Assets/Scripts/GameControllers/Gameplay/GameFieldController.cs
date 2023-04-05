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
        private readonly Dictionary<CellData, GameSides> _cellDatas = FieldComposition.DefaultField.FieldCells;

        #endregion


        #region Properties

        public Field Field { get; private set; }
        public GameSides[] CellSides => Field.CellSides;
        public CellData[] AllCellDatas => Field.AllCellDatas;
        public CellData[] AvaliableCellDatas => Field.AvaliableCellDatas;
        public CellData[] MarkedCellDatas => Field.MarkedCellDatas;
        public CellData[] PlayerCells => Field.PlayerCells;
        public CellData[] ComputerCells => Field.ComputerCells;

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
            UpdateField();
        }

        private void SetCellSide(CellData cell, GameSides side)
        {
            _cellDatas[cell] = side;
            UpdateField();
        }

        private void UpdateField()
        {
            Field = new Field(_cellDatas);
        }

        private bool IsCellTypeAvaliable(CellPlace place)
        {
            return AvaliableCellDatas.FirstOrDefault(x => x.CellPlace == place) != null;
        }

        private void SubscribeEvents()
        {
            SceneStateMachine.Instance.SceneStateChanging += UnsubscribeEvents;
            _roundController.RoundStarted += ClearCells;
            _roundController.PlayerTurnStarted += EnableGridInteraction;
            _roundController.PlayerTurnEnded += DisableGridInteraction;
            _markChoiseController.MarkChosen += OnMarkChosenByPlayer;
            _roundController.ComputerWon += DisableGridInteraction;
            _roundController.Draw += DisableGridInteraction;
            _roundController.Draw += RenderDraw;
            _roundController.GotPlayerWinCombination += RenderPlayerWinCombination;
            _roundController.GotComputerWinCombination += RenderComputerWinCombination;
            _roundController.RoundRestarted += ClearCells;
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
            _roundController.PlayerTurnStarted -= EnableGridInteraction;
            _roundController.PlayerTurnEnded -= DisableGridInteraction;
            _markChoiseController.MarkChosen -= OnMarkChosenByPlayer;
            _roundController.Draw -= DisableGridInteraction;
            _roundController.Draw -= RenderDraw;
            _roundController.GotPlayerWinCombination -= RenderPlayerWinCombination;
            _roundController.GotComputerWinCombination -= RenderComputerWinCombination;
            _roundController.RoundRestarted -= ClearCells;
            foreach (var cell in _canvasModel.Cells)
            {
                cell.Pressed -= OnCellPressedByPlayer;
            }
            _aiController.ComputerChoseCell -= OnCellChosenByComputer;
        }

        private void OnCellPressedByPlayer(GameCellModel cell)
        {
            if (!_roundController.IsPlayersTurn) return;
            SetCellSide(AllCellDatas[(int)cell.CellPlace], GameSides.Player);
            RenderCells();
            PlayerChoseCell?.Invoke();
        }

        private void OnCellChosenByComputer(CellData cell)
        {
            SetCellSide(cell, GameSides.Computer);
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
                var cellInModel = _canvasModel.Cells.First(x => x.CellPlace == cell.Key.CellPlace);

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

        private void RenderPlayerWinCombination(WinCombinations combination)
        {
            foreach (var cell in FieldAnalyzer.EndCombinations[combination])
            {
                _canvasModel.Cells.First(x => x.CellPlace == cell).Image.color = _canvasModel.PlayerWinCellColor;
            }
        }

        private void RenderComputerWinCombination(WinCombinations combination)
        {
            foreach (var cell in FieldAnalyzer.EndCombinations[combination])
            {
                _canvasModel.Cells.First(x => x.CellPlace == cell).Image.color = _canvasModel.ComputerWinCellColor;
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
                cell.Image.color = _canvasModel.SimpleCellColor;
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
                if(doEnable && IsCellTypeAvaliable(cell.CellPlace) || !doEnable)
                    cell.Button.interactable = doEnable;
            }
        }

        #endregion
    }
}