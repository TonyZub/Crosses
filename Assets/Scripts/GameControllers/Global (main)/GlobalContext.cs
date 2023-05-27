using System;
using System.Collections.Generic;


namespace Crosses
{
	public sealed class GlobalContext : NonMonoSingleton<GlobalContext>
	{
        #region Fields

        private SceneStateMachine _sceneStateMachine;
        private Dictionary<Type, object> _diContainer;

        #endregion


        #region Properties

        public SceneStateMachine SceneStateMachine => _sceneStateMachine;

        #endregion


        #region Constuctor

        public GlobalContext() : base()
        {
            CreateDIContainer();
            CreateGlobalServices();
            CreateSceneStateMachine();
        }

        #endregion


        #region Methods

        private void CreateDIContainer()
        {
            _diContainer = new Dictionary<Type, object>();
        }

        private void CreateGlobalServices()
        {
            RegisterDependency(new GlobalServices());
        }

        private void CreateSceneStateMachine()
        {
            _sceneStateMachine = new SceneStateMachine();
        }

        public void RegisterDependency<T>(T obj)
        {
            if (_diContainer.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"type {typeof(T)} is already in DI container");
            }
            _diContainer.Add(typeof(T), obj);
        }

        public void UnregisterDependency<T>()
        {
            if (!_diContainer.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"type {typeof(T)} is missing in DI container");
            }
            _diContainer.Remove(typeof(T));
        }

        public T GetDependency<T>()
        {
            if(_diContainer.TryGetValue(typeof(T), out object dependecy))
            {
                return (T)dependecy;
            }
            throw new ArgumentException($"type {typeof(T)} is missing in DI container");
        }

        public void DisposeAndUnregisterDependency<T>()
        {
            var dependency = GetDependency<T>();
            if(dependency is IDispose)
            {
                (dependency as IDispose).OnDispose();
            }
            UnregisterDependency<T>();
        }

        protected override void Dispose()
        {
            DisposeAndUnregisterDependency<GlobalServices>();
            base.Dispose();
        }

        #endregion
    }
}