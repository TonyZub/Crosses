namespace Crosses
{
    public sealed class GameplayState : BaseSceneState
    {
        #region Constructor

        public GameplayState(SceneStateNames stateName, SceneNames sceneName) : base(stateName, sceneName) { }

        #endregion


        #region Methods

        public override void EnterState()
        {
            //TODO
            base.EnterState();
        }

        protected override void OnSceneLoadingComplete()
        {
            GlobalContext.Instance.RegisterDependency(new GameplayController(ObjectFinder.
                FindObjectOfType<GameplayCanvasModel>(true)));
            base.OnSceneLoadingComplete();
        }

        public override void ExitState()
        {
            GlobalContext.Instance.UnregisterDependency<GameplayController>();
        }

        #endregion
    }
}
