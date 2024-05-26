using MonoInjector.Providers;
using MonoInjector.Strategies;
using UnityEngine.SceneManagement;

namespace MonoInjector
{
    internal sealed class MonoInjectorContext
    {
        private static MonoInjectorContext _instance = null;

        internal static MonoInjectorContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MonoInjectorContext();
                }
                return _instance;
            }
        }

        private ResolvingStrategy _resolvingStrategy;
        private InstancesProvider _instancesProvider;

        private MonoInjectorContext() 
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
