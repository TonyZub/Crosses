namespace Crosses
{
    public sealed class TransitionState : BaseSceneState
    {
        #region Constructor

        public TransitionState(SceneStateNames stateName, SceneNames sceneName) : base(stateName, sceneName) { }

        #endregion


        #region Methods

        public override void EnterState()
        {
            //TODO
            base.EnterState();
        }

        protected override void OnSceneLoadingComplete()
        {
            GlobalContext.Instance.RegisterDependency(new TransitionalScreenController(ObjectFinder.
                FindObjectOfType<VideoCanvasModel>(true)));
            base.OnSceneLoadingComplete();
        }

        public override void ExitState()
        {
            GlobalContext.Instance.DisposeAndUnregisterDependency<TransitionalScreenController>();
        }

        #endregion
    }
}
