using UniDI.Providers;
using UniDI.Strategies;
using UnityEngine.SceneManagement;

namespace UniDI
{
    internal sealed class UniDIContext
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

        internal void Inject<I>(I injected, Lifetime lifetime)
        {
            _instancesProvider.Store(injected, lifetime);
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            _instancesProvider.ClearSceneInstances();
        }
    }
}
