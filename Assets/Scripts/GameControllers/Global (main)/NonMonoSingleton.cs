using System;


namespace Crosses
{
	public abstract class NonMonoSingleton<T> where T : NonMonoSingleton<T>, new()
	{
        #region Properties

        public static T Instance { get; private set; }

        #endregion


        #region Constructor

        protected NonMonoSingleton()
        {
            if(Instance == null)
            {
                Instance = this as T;
            }          
            else
            {
                throw new InvalidOperationException($"Constructing a { typeof(T).Name}" +
                    $" is already done, there must be not more than 1 class on session");
            }
            SubscribeDispose();
        }

        #endregion


        #region Methods

        private void SubscribeDispose()
        {
            GlobalController.Instance.OnDispose += Dispose;
        }

        private void UnsubscribeDispose()
        {
            GlobalController.Instance.OnDispose -= Dispose;
        }

        protected virtual void Dispose()
        {
            UnsubscribeDispose();
        }

        #endregion
    }
}