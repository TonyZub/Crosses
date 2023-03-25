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
            //TODO
            base.OnSceneLoadingComplete();
        }

        public override void ExitState()
        {
            //TODO
        }

        #endregion
    }
}
