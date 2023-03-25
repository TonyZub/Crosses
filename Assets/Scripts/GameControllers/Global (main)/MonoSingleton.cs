using UnityEngine;
using System;


namespace Crosses
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
        #region Events

        public event Action OnAfterInit;
        public event Action OnDispose;

        #endregion


        #region Fields

        private static T _instance;

        #endregion


        #region Properties

        public static T Instance => _instance;
        public static bool IsInitialized { get; private set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Initialize();
        }
      
        private void OnDestroy()
        {
            Dispose();
        }

        #endregion


        #region Methods

        protected virtual void Initialize()
        {
            PreventDoubleInitialization();
            TrySetInstance();
            IsInitialized = true;
            OnAfterInit?.Invoke();
        }

        private void PreventDoubleInitialization()
        {
            if (IsInitialized)
            {
                Debug.LogError($"Component {typeof(T)} is already on scene");
                Destroy(this);
            }
        }

        private void TrySetInstance()
        {
            _instance = FindObjectOfType<T>();
            if (_instance == null) throw new NullReferenceException($"Can't find object of type {typeof(T)} on scene");
        }

        protected virtual void Dispose()
        {
            OnDispose?.Invoke();
        }

        #endregion
    }
}