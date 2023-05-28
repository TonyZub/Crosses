namespace Crosses
{
    public sealed class MethodicState : BaseSceneState
    {
        #region Constructor

        public MethodicState(SceneStateNames stateName, SceneNames sceneName) : base(stateName, sceneName) { }

        #endregion


        #region Methods

        public override void EnterState()
        {
            //TODO
            base.EnterState();
        }

        protected override void OnSceneLoadingComplete()
        {
            GlobalContext.Instance.RegisterDependency(new MethodicScreenController(ObjectFinder.
                FindObjectOfType<MethodicScreenCanvasModel>(true)));
            base.OnSceneLoadingComplete();
        }

        public override void ExitState()
        {
            GlobalContext.Instance.DisposeAndUnregisterDependency<MethodicScreenController>();
        }

        #endregion
    }
}
