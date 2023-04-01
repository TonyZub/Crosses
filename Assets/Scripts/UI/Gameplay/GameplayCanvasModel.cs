using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Crosses
{
	public sealed class GameplayCanvasModel : MonoBehaviour
	{
        #region Fields

        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private float _computerMinThinkingTime;
        [SerializeField] private float _computerMaxThinkingTime;
        [SerializeField] private float _delayBeforeFirstStart;
        [SerializeField] private int _minRounds;
        [SerializeField] private GameSides _firstTurnSide;
        [SerializeField] private MarkChoisePanelModel _markChoisePanelModel;
        [SerializeField] private WinPanelModel _winPanelModel;
        [SerializeField] private Button _chooseMarkBtn;
        [SerializeField] private Button _giveUpBtn;
        [SerializeField] private TMP_Text _roundTxt;
        [SerializeField] private Color _simpleCellColor;
        [SerializeField] private Color _playerCellColor;
        [SerializeField] private Color _computerCellColor;
        [SerializeField] private Color _playerWinCellColor;
        [SerializeField] private Color _computerWinCellColor;
		[SerializeField] private GameCellModel[] _cells;

        #endregion


        #region Properties

        public Difficulty Difficulty => _difficulty;
        public float ComputerMinThinkingTime => _computerMinThinkingTime;
        public float ComputerMaxThinkingTime => _computerMaxThinkingTime;
        public float DelayBeforeFirstStart => _delayBeforeFirstStart;
        public int MinRounds => _minRounds;
        public GameSides FirstTurnSide => _firstTurnSide;
        public MarkChoisePanelModel MarkChoisePanelModel => _markChoisePanelModel;
        public WinPanelModel WinPanelModel => _winPanelModel;
        public Button ChooseMarkBtn => _chooseMarkBtn;
        public Button GiveUpBtn => _giveUpBtn;
        public TMP_Text RoundTxt => _roundTxt;
        public Color SimpleCellColor => _simpleCellColor;
        public Color PlayerCellColor => _playerCellColor;
        public Color ComputerCellColor => _computerCellColor;
        public Color PlayerWinCellColor => _playerWinCellColor;
        public Color ComputerWinCellColor => _computerWinCellColor;
        public GameCellModel[] Cells => _cells;

        #endregion
    }
}