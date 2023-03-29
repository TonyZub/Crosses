using UnityEngine;
using TMPro;


namespace Crosses
{
	public sealed class WinPanelModel : MonoBehaviour
	{
        #region Fields

        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TMP_Text _winTxt;
        [SerializeField] private float _timeToAppear;
        [SerializeField] private float _showingTime;
        [SerializeField] private float _timeToDisappear;

        #endregion


        #region Properties

        public CanvasGroup Group => _group;
        public TMP_Text WinTxt => _winTxt;
        public float TimeToAppear => _timeToAppear;
        public float ShowingTime => _showingTime;
        public float TimeToDisappear => _timeToDisappear;

        #endregion
    }
}