using UnityEngine;

namespace MonoInjector.Test
{
    public class MonoBehaviourInjectionTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject _injected;
        [SerializeField]
        private GameObject _consumer;

        private void Start()
        {
            var injected = Instantiate(_injected);
            injected.Inject();

            var consumer = _consumer.InstantiateResolve();
            var c = consumer.GetComponent<MonoConsumer>();
            Debug.Log(c.GetSum());
        }
    }
}
