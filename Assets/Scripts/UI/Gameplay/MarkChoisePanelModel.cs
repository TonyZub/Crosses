using UnityEngine;
using UnityEngine.UI;
using System;


namespace Crosses
{
    [RequireComponent(typeof(CanvasGroup))]
	public sealed class MarkChoisePanelModel : MonoBehaviour
	{
        #region Events

        public event Action<CellMarks> MarkChosen;

        #endregion


        #region Fields

        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Button _crossBtn;
		[SerializeField] private Button _noughtBtn;
        [SerializeField] private float _showingTime;

        #endregion


        #region Properties

        public CanvasGroup Group => _group;
		public Button CrossBtn => _crossBtn;
		public Button NoughtBtn => _noughtBtn;
        public float ShowingTime => _showingTime;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            _crossBtn.onClick.AddListener(OnCrossesPressed);
            _noughtBtn.onClick.AddListener(OnNoughtsPressed);
        }

        private void UnsubscribeEvents()
        {
            _crossBtn.onClick.RemoveAllListeners();
            _noughtBtn.onClick.RemoveAllListeners();
        }

        private void OnCrossesPressed()
        {
            MarkChosen?.Invoke(CellMarks.Cross);
        }

        private void OnNoughtsPressed()
        {
            MarkChosen?.Invoke(CellMarks.Nought);
        }

        #endregion
    }
}