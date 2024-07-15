namespace UniDI.Test
{
    public class ReinjectConsumer
    {
        [Inject] private ReinjectInjected _field;

        [Inject] public ReinjectInjected Property { get; private set; }

        private ReinjectInjected _method;

        [Inject]
        private void Init(ReinjectInjected method)
        {
            _method = method;
        }

        public string GetNames()
        {
            return $"{_field.Name} {Property.Name} {_method.Name}";
        }
    }
}
