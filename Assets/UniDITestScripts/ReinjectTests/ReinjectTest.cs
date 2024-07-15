using UnityEngine;

namespace UniDI.Test
{
    public class ReinjectTest : MonoBehaviour
    {
        private void Start()
        {
            var injected1 = new ReinjectInjected();
            injected1.Name = "1";
            injected1.Inject();

            var consumer1 = new ReinjectConsumer();
            consumer1.Resolve();

            Debug.Log(consumer1.GetNames());

            var injected2 = new ReinjectInjected();
            injected2.Name = "2";
            injected2.Inject();

            var consumer2 = new ReinjectConsumer();
            consumer2.Resolve();

            Debug.Log(consumer2.GetNames());
        }
    }
}
