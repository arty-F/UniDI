using UnityEngine;

namespace Assets.Core
{
    [DefaultExecutionOrder(-1000)]
    public class UniDIContext : MonoBehaviour
    {
        public static UniDIContext Instance;

        private ResolvingStrategy _resolversProvider;
        private InstancesProvider _instancesProvider;

        private void Awake()
        {
            Instance = this;
            _instancesProvider = new InstancesProvider();
            _resolversProvider = new ResolvingStrategy(_instancesProvider);
        }

        public void Resolve(object consumer)
        {
            _resolversProvider.Resolve(consumer);
        }

        public void Inject<I>(I injected)
        {
            _instancesProvider.Store(injected);
        }
    }
}
