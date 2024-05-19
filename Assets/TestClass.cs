namespace Assets
{
    public class TestClass
    {
        [Inject]
        private InjectedClass1 _field1;

        [Inject]
        private InjectedClass2 _field2;

        [Inject]
        public InjectedClass1 Property { get; set; }

        private InjectedClass1 _injectedFromMethod;

        [Inject]
        public void Method(InjectedClass1 injected)
        {
            _injectedFromMethod = injected;
        }

        public string GetField1() => _field1?.Name ?? "empty";
        public string GetField2() => _field2?.Name ?? "empty";
        public string GetProperty() => Property?.Name ?? "empty";
        public string GetMethod() => _injectedFromMethod?.Name ?? "empty";

        public int GetSum() => _field1.Value + _field2.Value;
    }
}
