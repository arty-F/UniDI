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
            injected.Inject<MonoInjected>();

            //var consumer = _consumer.InstantiateResolve();

            var consumer = _consumer.InstantiateResolve<MonoConsumer>();

            Debug.Log(consumer.GetComponent<MonoConsumer>().GetSum());
        }
    }
}
