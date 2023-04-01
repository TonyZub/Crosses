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
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Image _parentImage;

        #endregion


        #region Properties

        public CellType CellType => _cellType;
		public Button Button => _button;
        public Image Image => _image;
        public Image ParentImage => _parentImage;

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