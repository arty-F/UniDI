using UnityEngine;

namespace MonoInjector.Test
{
    public class MonoConsumer : MonoBehaviour
    {
        [Inject] private MonoInjected _field;

        [Inject] public MonoInjected Property { get; private set; }


        private MonoInjected _injectedFromMethod;

        [Inject]
        public void Method(MonoInjected injected)
        {
            _injectedFromMethod = injected;
        }

        public int GetSum()
        {
            var field = _field.Value;
            var property = Property.Value;
            var method = _injectedFromMethod.Value;

            return field + property + method;
        }
    }
}
