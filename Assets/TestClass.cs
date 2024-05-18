namespace Assets
{
    public class TestClass
    {
        [Inject]
        public InjectedClass _field;

        [Inject]
        public InjectedClass Property { get; set; }

        private InjectedClass _injectedFromMethod;

        [Inject]
        public void Method(InjectedClass injected)
        {
            _injectedFromMethod = injected;
        }

        public string GetField() => _field?.Name ?? "empty";
        public string GetProperty() => Property?.Name ?? "empty";
        public string GetMethod() => _injectedFromMethod?.Name ?? "empty";

        public int GetSum() => _field.Value + Property.Value + _injectedFromMethod.Value;
    }
}
