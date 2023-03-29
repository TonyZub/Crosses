using UnityEngine;
using UnityEngine.UI;
using System;


namespace Crosses
{
	[RequireComponent(typeof(Button))]
	public sealed class GameCellModel : MonoBehaviour
	{
		#region Events

		public event Action<GameCellModel> Pressed;

        #endregion


        #region Fields

        [SerializeField] private CellType _cellType;
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private bool _isOnMajorDiagonal;
        [SerializeField] private bool _isOnMinorDiagonal;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        #endregion


        #region Properties

        public CellType CellType => _cellType;
		public Button Button => _button;
        public Image Image => _image;
        public int Row => _row;
        public int Column => _column;
        public bool IsOnMajorDiagonal => _isOnMajorDiagonal;
        public bool IsOnMinorDiagonal => _isOnMinorDiagonal;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            SubscribeEvent();
        }

        private void OnDestroy()
        {
            UnsubscribeEvent();
        }

        #endregion


        #region Methods

        private void SubscribeEvent()
        {
            _button.onClick.AddListener(ButtonPressHandler);
        }

        private void UnsubscribeEvent()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void ButtonPressHandler()
        {
            Pressed?.Invoke(this);
        }

        #endregion
    }
}