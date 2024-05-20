namespace Assets
{
    public class TestClass
    {
        [Inject]
        private InjectedClass1 _field1;

        [Inject]
        private InjectedClass2 _field2;

        [Inject]
        private InjectedClass3 _field3;

        [Inject]
        public InjectedClass1 Property1 { get; private set; }

        [Inject]
        public InjectedClass2 Property2 { get; private set; }

        [Inject]
        public InjectedClass3 Property3 { get; private set; }

        private InjectedClass1 _injectedFromMethod;

        [Inject]
        public void Method(InjectedClass1 injected)
        {
            _injectedFromMethod = injected;
        }

        public string GetField1() => _field1?.Name ?? "empty";
        public string GetField2() => _field2?.Name ?? "empty";
        public string GetProperty() => Property1?.Name ?? "empty";
        public string GetMethod() => _injectedFromMethod?.Name ?? "empty";

        public int GetSumField() => _field1.Value + _field2.Value + _field3.Value;
        public int GetSumProp() => Property1.Value + Property2.Value + Property3.Value;
    }
}
