using System;
using UniDI.Providers;
using UniDI.Strategies;
using UnityEngine.SceneManagement;

namespace UniDI
{
    internal class UniDIContext
    {
        private static UniDIContext _instance = null;

        internal static UniDIContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UniDIContext();
                }
                return _instance;
            }
        }

        private ResolvingStrategy _resolvingStrategy;
        private InstancesProvider _instancesProvider;

        private UniDIContext() 
        {
            _instancesProvider = new InstancesProvider();
            _resolvingStrategy = new ResolvingStrategy(_instancesProvider);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        internal void Resolve(object consumer)
        {
            _resolvingStrategy.Resolve(consumer);
        }

        internal void Resolve(object consumer, int id)
        {
            _resolvingStrategy.Resolve(consumer, id);
        }

        internal void Inject<I>(I injected, Lifetime lifetime)
        {
            _instancesProvider.Store(injected, lifetime);
        }

        internal void Inject<T>(T injected, int id, Lifetime lifetime)
        {
            _instancesProvider.Store(injected, id, lifetime);
        }

        internal void ReleaseDependency(Type type)
        {
            _instancesProvider.ClearInstances(type);
        }

        internal void ReleaseDependency(Type type, int id, bool clearFullScope)
        {
            _instancesProvider.ClearInstancesById(type, id, clearFullScope);
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            _instancesProvider.ClearSceneInstances();
        }
    }
}
