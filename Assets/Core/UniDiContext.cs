using UnityEngine;

namespace Assets.Core
{
    [DefaultExecutionOrder(-1000)]
    public class UniDIContext : MonoBehaviour
    {
        public static UniDIContext Instance;

        private ResolvingStrategy _resolvingStrategy;
        private InstancesProvider _instancesProvider;

        private void Awake()
        {
            Instance = this;
            _instancesProvider = new InstancesProvider();
            _resolvingStrategy = new ResolvingStrategy(_instancesProvider);
        }

        public void Resolve(object consumer)
        {
            _resolvingStrategy.Resolve(consumer);
        }

        public void Inject<I>(I injected)
        {
            _instancesProvider.Store(injected);
        }
    }
}
