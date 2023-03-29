namespace Crosses
{
    public sealed class WelcomeScreenState : BaseSceneState
    {
        #region Constructor

        public WelcomeScreenState(SceneStateNames stateName, SceneNames sceneName) : base(stateName, sceneName) { }

        #endregion


        #region Methods

        public override void EnterState()
        {
            //TODO
            base.EnterState();
        }

        protected override void OnSceneLoadingComplete()
        {
            GlobalContext.Instance.RegisterDependency(new WelcomeScreenController(ObjectFinder.
                FindObjectOfType<WelcomeScreenCanvasModel>()));
            base.OnSceneLoadingComplete();
        }

        public override void ExitState()
        {
            GlobalContext.Instance.UnregisterDependency<WelcomeScreenController>();
        }

        #endregion
    }
}
