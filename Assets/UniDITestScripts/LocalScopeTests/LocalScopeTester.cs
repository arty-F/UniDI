using UnityEngine;

namespace UniDI.Test
{
    public class LocalScopeTester : MonoBehaviour
    {
        private void Start()
        {
            var injected1 = new LocalInjectedClass(1);
            var injected2 = new LocalInjectedClass(2);
            injected1.Inject(1);
            injected2.Inject(2);

            var consumer1 = new LocalConsumerClass();
            var consumer2 = new LocalConsumerClass();
            consumer1.Resolve(1);
            consumer2.Resolve(2);

            Debug.Log($"consumer1: {consumer1.LocalInjectedClass.Value}");
            Debug.Log($"consumer2: {consumer2.LocalInjectedClass.Value}");
        }
    }
}
