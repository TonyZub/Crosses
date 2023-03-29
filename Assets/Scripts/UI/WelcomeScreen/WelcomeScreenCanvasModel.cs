using UnityEngine;
using UnityEngine.UI;


namespace Crosses
{
    public sealed class WelcomeScreenCanvasModel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _moveNextBtn;

        #endregion


        #region Properties

        public Button MoveNextBtn => _moveNextBtn;

        #endregion
    }
}

