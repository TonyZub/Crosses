using System;
using Extensions;
using DG.Tweening;
#if UNITY_EDITOR
using EditorExtensions;
#endif


namespace Crosses
{
	public sealed class SceneStateMachine : NonMonoSingleton<SceneStateMachine>
    {
        #region Constants

        private const float TIME_BEFORE_START_GAME = 0f;

        #endregion


        #region Events

        public event Action OnBeforeStateChange;
        public event Action SceneStateChanging;
        public event Action OnSceneStateChanged;

        #endregion


        #region Fields

        private SceneLoadingCanvasController _sceneLoadingCanvasController;
        private BaseSceneState[] _sceneStates;

        #endregion


        #region Properties

        public BaseSceneState CurrentSceneState { get; private set; }
        public SceneLoadingCanvasController SceneLoadingCanvasController => _sceneLoadingCanvasController;
        public bool IsSceneStatesArrayComplete => _sceneStates != null && _sceneStates.Length > 0;

        #endregion


        #region Constuctor

        public SceneStateMachine() : base()
        {
            CreateSceneStates();
            CreateSceneLoadingCanvasController();
            DOVirtual.DelayedCall(TIME_BEFORE_START_GAME, () => SetStartState());
        }

        #endregion


        #region Public Methods

        public void SetState(SceneStateNames stateName)
        {
            CheckIfStatesArrayComplete();
            CheckIfCurrentStateIsNotNull();
            OnBeforeStateChange?.Invoke();
            CurrentSceneState.ExitState();
            InitSceneSwitch(stateName);
        }

        #endregion


        #region Private Methods

        private void CreateSceneStates()
        {
            _sceneStates = new BaseSceneState[6]
            {
                new BootstrapSceneState(SceneStateNames.Bootstrap, SceneNames.Bootstrap),
                new WelcomeScreenState(SceneStateNames.Welcome, SceneNames.WelcomeScreen),
                new GameplayState(SceneStateNames.Gameplay, SceneNames.Gameplay),
                new TransitionState(SceneStateNames.Transition, SceneNames.TransitionalScreen),
                new MetodicState(SceneStateNames.Metodic, SceneNames.MetodicScreen),
                new ThanksState(SceneStateNames.Thanks, SceneNames.ThanksScreen),
            };
        }

        private void CreateSceneLoadingCanvasController()
        {
            _sceneLoadingCanvasController = new SceneLoadingCanvasController(this);
        }

        private void SetStartState()
        {
            CheckIfStatesArrayComplete();
#if UNITY_EDITOR
            if (LevelSelectionHelper.IsLevelSelected)
            {
                InitSceneSwitch((SceneStateNames)(LevelSelectionHelper.SelectedLevelIndex + 2)); // 2 - difference in indexes between array an enum of names  
            }
            else
            {
                InitSceneSwitch(SceneStateNames.Transition);
            }
#else
            InitSceneSwitch(SceneStateNames.Welcome);
#endif
        }

        private void InitSceneSwitch(SceneStateNames stateName)
        {
            SceneStateChanging?.Invoke();
            CurrentSceneState = GetSceneStateFromArrayByName(stateName);
            OnSceneStateChanged?.Invoke();
            CurrentSceneState.EnterState();
        }

        private void CheckIfStatesArrayComplete()
        {
            if (!IsSceneStatesArrayComplete) throw new NullReferenceException("Scene states array is not complete");
        }

        private void CheckIfCurrentStateIsNotNull()
        {
            if (CurrentSceneState == null) throw new NullReferenceException("Current scene state is null");
        }

        private BaseSceneState GetSceneStateFromArrayByName(SceneStateNames stateName)
        {
            return _sceneStates.FirstWhich(x => x.StateName == stateName);
        }

        protected override void Dispose()
        {
            base.Dispose();
            CurrentSceneState?.ExitState();
        }

#endregion
    }
}