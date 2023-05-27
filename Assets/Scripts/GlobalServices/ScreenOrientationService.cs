using UnityEngine;
using System;


namespace Crosses
{
	public sealed class ScreenOrientationService : IDispose
	{
        #region Events

        public event Action<ScreenTypes> ScreenOrientationChanged;

        #endregion


        #region Fields

        private ScreenTypes _screenOrientation;

        #endregion


        #region Properties

        public ScreenTypes ScreenOrientation
        {
            get => _screenOrientation;
            private set
            {
                if(_screenOrientation != value)
                {
                    _screenOrientation = value;
                    ScreenOrientationChanged?.Invoke(ScreenOrientation);
                }
            }
        }

        #endregion


        #region Constructor

        public ScreenOrientationService()
        {
            CheckScreenOrientation();
            SubscribeEvents();
        }

        #endregion


        #region IDispose

        public void OnDispose()
        {
            UnsubscribeEvents();
        }

        #endregion


        #region Methods

        private void SubscribeEvents()
        {
            GlobalController.Instance.OnUpdate += CheckScreenOrientation;
        }

        private void UnsubscribeEvents()
        {
            GlobalController.Instance.OnUpdate -= CheckScreenOrientation;
        }

        private void CheckScreenOrientation()
        {
            ScreenOrientation = Screen.width > Screen.height ? ScreenTypes.Horizontal : ScreenTypes.Vertical;
        }

        #endregion
    }
}