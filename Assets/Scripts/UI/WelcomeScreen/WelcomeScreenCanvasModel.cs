using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Crosses
{
    public sealed class WelcomeScreenCanvasModel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CanvasGroup _welcomeTextCanvasGroup;
        [SerializeField] private CanvasGroup _dataCanvasGroup;
        [SerializeField] private RectTransform _containerPanel;
        [SerializeField] private TMP_Text _alertText;
        [SerializeField] private Button _moveNextBtn;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _ageInput;
        [SerializeField] private Toggle _maleToggle;
        [SerializeField] private Toggle _femaleToggle;
        [SerializeField] private Vector2 _containerAnchorsForVerticalScreen;
        [SerializeField] private Vector2 _containerAnchorsForHorizontalScreen;
        [SerializeField] private float _switchScreenDuration;

        #endregion


        #region Properties

        public CanvasGroup WelcomeTextCanvasGroup => _welcomeTextCanvasGroup;
        public CanvasGroup DataCanvasGroup => _dataCanvasGroup;
        public RectTransform ContainerPanel => _containerPanel;
        public TMP_Text AlertText => _alertText;
        public Button MoveNextBtn => _moveNextBtn;
        public TMP_InputField NameInput => _nameInput;
        public TMP_InputField AgeInput => _ageInput;
        public Toggle MaleToggle => _maleToggle;
        public Toggle FemaleToggle => _femaleToggle;
        public Vector2 ContainerAnchorsForVerticalScreen => _containerAnchorsForVerticalScreen;
        public Vector2 ContainerAnchorsForHorizontalScreen => _containerAnchorsForHorizontalScreen;
        public float SwitchScreenDuration => _switchScreenDuration;

        #endregion
    }
}

