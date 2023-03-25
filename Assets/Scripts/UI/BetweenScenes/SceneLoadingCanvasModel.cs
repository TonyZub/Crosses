using UnityEngine;
using UnityEngine.UI;


namespace Crosses
{
    [RequireComponent(typeof(CanvasGroup))]
	public sealed class SceneLoadingCanvasModel : MonoBehaviour
	{
        #region Fields

        [SerializeField] private Image _fillableImage;
        [SerializeField] private float _smoothShowHideTime;

        private CanvasGroup _canvasGroup;

		#endregion


		#region Properties

		public Image FillableImage => _fillableImage;
        public CanvasGroup CanvasGroup => _canvasGroup;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            GetDependencies();
            DontDestroyOnLoad(this);
            Hide();
        }

        #endregion


        #region Methods

        private void GetDependencies()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Applies progress value to fillable image and percent text
        /// </summary>
        /// <param name="progressValue">Value from 0 to 1</param>
        public void SetProgress(float progressValue)
        {
            FillableImage.fillAmount = progressValue;
            //ProgressPercent.text = $"{Mathf.Round(progressValue * 10000) / 100}%";
		}

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetCanvasAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        #endregion
    }
}