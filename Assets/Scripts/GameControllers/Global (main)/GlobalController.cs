using System;
using UnityEngine;


namespace Crosses
{
    public sealed class GlobalController : MonoSingleton<GlobalController>
    {
        #region Events

        public event Action OnStart;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        #endregion


        #region Fields

        private GlobalContext _globalContext;

        #endregion


        #region Properties

        public GlobalContext GlobalContext => _globalContext;

        #endregion


        #region MonoSingleton

        protected override void Initialize()
        {
            base.Initialize();
            InitDoTween();
            KeepAliveDuringGame();
            CreateGlobalContext();
        }

        protected override void Dispose()
        {
            //TODO - manually dispose something if needed
            base.Dispose();
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            OnStart?.Invoke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        #endregion


        #region Methods

        private void InitDoTween()
        {
            DG.Tweening.DOTween.Init(false, false);
        }

        private void KeepAliveDuringGame()
        {
            DontDestroyOnLoad(this);
        }

        private void CreateGlobalContext()
        {
            _globalContext = new GlobalContext();
        }

        #endregion
    }
}
