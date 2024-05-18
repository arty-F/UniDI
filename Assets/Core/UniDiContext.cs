using UnityEngine;

namespace Assets.Core
{
    [DefaultExecutionOrder(-1000)]
    public class UniDiContext : MonoBehaviour
    {
        public static UniDiContext Instance;

        private DelegatesResolver _resolver;

        private void Awake()
        {
            Instance = this;
            _resolver= new DelegatesResolver();
        }

        public void Resolve(object consumer)
        {
            _resolver.Resolve(consumer);
        }

        public void Inject<I>(I injected)
        {
            _resolver.Inject(injected);
        }
    }
}
