using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Crosses
{
	public abstract class BaseSceneState
	{
		#region Constants

		private const float SCENE_MAX_PROGRESS = 0.9f;

        #endregion


        #region Events

        public event Action OnSceneLoadingStarted;
		public event Action OnSceneLoadingEnded;

		#endregion


		#region Fields

		private AsyncOperation _sceneLoadingOperation;
		private readonly SceneStateNames _stateName;
		private readonly SceneNames _sceneName;

		#endregion


		#region Properties

		public SceneStateNames StateName => _stateName;
		public SceneNames SceneName => _sceneName;
		public float SceneLoadingProgress => _sceneLoadingOperation.progress / SCENE_MAX_PROGRESS;
		public bool IsActiveState { get; private set; }

        #endregion


        #region Constuctor

		public BaseSceneState(SceneStateNames stateName, SceneNames sceneName)
        {
			_stateName = stateName;
			_sceneName = sceneName;
		}

        #endregion


        #region Methods

		private void SubscribeEvents()
        {
			SceneStateMachine.Instance.SceneLoadingCanvasController.MinTimeToLoadSceneEnded += OnMinTimeToLoadSceneEnded;
		}

		private void UnsubscribeEvents()
        {
			SceneStateMachine.Instance.SceneLoadingCanvasController.MinTimeToLoadSceneEnded -= OnMinTimeToLoadSceneEnded;
		}

        private void LoadSceneAsync()
        {
			OnSceneLoadingStarted?.Invoke();
			_sceneLoadingOperation = SceneManager.LoadSceneAsync((int)SceneName);
			_sceneLoadingOperation.allowSceneActivation = false;
			_sceneLoadingOperation.completed += OnAfterSceneLoadingEnded;
		}

		private void OnMinTimeToLoadSceneEnded()
        {
			if(IsActiveState) _sceneLoadingOperation.allowSceneActivation = true;
		} 

		private void OnAfterSceneLoadingEnded(AsyncOperation operation)
        {
			operation.completed -= OnAfterSceneLoadingEnded;
			OnSceneLoadingComplete();
		}

		protected virtual void OnSceneLoadingComplete()
        {
			OnSceneLoadingEnded?.Invoke();
        }

		public virtual void EnterState()
        {
			IsActiveState = true;
			SubscribeEvents();
			LoadSceneAsync();
		}

		public virtual void ExitState()
        {
			UnsubscribeEvents();
			IsActiveState = false;
        }

		#endregion
	}
}